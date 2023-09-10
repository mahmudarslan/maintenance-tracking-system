using Arslan.Vms.OrderService.DocumentNoFormats;
using Arslan.Vms.OrderService.Identity;
using Arslan.Vms.OrderService.Inventory;
using Arslan.Vms.OrderService.Localization;
using Arslan.Vms.OrderService.Permissions;
using Arslan.Vms.OrderService.PurchaseOrders;
using Arslan.Vms.OrderService.Taxes;
using Arslan.Vms.OrderService.v1.Dtos;
using Arslan.Vms.OrderService.v1.PurchaseOrders.Dtos;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.v1.PurchaseOrders
{
    [Authorize(OrderServicePermissions.PurchaseOrder.Default)]
    public class PurchaseOrderAppService : OrderServiceAppService, IPurchaseOrderAppService
    {
        #region Fields
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly PurchaseOrderManager _purchaseOrderManager;
        private readonly IRepository<OrderProduct, Guid> _productRepository;
        private readonly IRepository<AppUser, Guid> _userRepository;
        private readonly IRepository<TaxingScheme, Guid> _taxSchemeRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IDataFilter _dataFilter;
        private readonly IStringLocalizer<OrderServiceResource> _localizer;
        private readonly DocNoFormatManager _docNoFormatManager;
        private readonly PurchaseOrderTransactionManager _transactionManager;
        #endregion

        #region Ctor
        public PurchaseOrderAppService(
     IPurchaseOrderRepository purchaseOrderRepository,
     PurchaseOrderManager purchaseOrderManager,
     IRepository<AppUser, Guid> userRepository,
     IRepository<OrderProduct, Guid> productRepository,
     IRepository<TaxingScheme, Guid> taxSchemeRepository,
     ICurrentTenant currentTenant,
     IDataFilter dataFilter,
     IGuidGenerator guidGenerator,
     IStringLocalizer<OrderServiceResource> localizer,
     DocNoFormatManager docNoFormatManager,
     PurchaseOrderTransactionManager transactionManager
    )
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _docNoFormatManager = docNoFormatManager;
            _currentTenant = currentTenant;
            _productRepository = productRepository;
            _dataFilter = dataFilter;
            _guidGenerator = guidGenerator;
            _localizer = localizer;
            _userRepository = userRepository;
            _transactionManager = transactionManager;
            _purchaseOrderManager = purchaseOrderManager;
            _taxSchemeRepository = taxSchemeRepository;
        }
        #endregion

        [Authorize(OrderServicePermissions.PurchaseOrder.Create)]
        public async Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderDto input)
        {
            var po = new PurchaseOrder(_guidGenerator.Create(), _currentTenant.Id, input.OrderDate, input.VendorId,
                input.PaymentStatus, input.InventoryStatus,
                input.TaxSchemeId, input.CurrencyId, input.AmountPaid, _docNoFormatManager)
            {
                OrderRemarks = input.OrderRemarks,
                LocationId = input.LocationId,
            };

            var tax = _taxSchemeRepository.WithDetails(w => w.TaxCodes).FirstOrDefault(f => f.Id == input.TaxSchemeId);
            var defaultTaxCode = tax.TaxCodes.FirstOrDefault(f => f.Id == tax.DefaultTaxCodeId);

            foreach (var inputLine in input.ProductLines ?? Enumerable.Empty<CreatePurchaseOrderProductLineDto>())
            {
                var taxCode = inputLine.TaxCodeId == Guid.Empty ? defaultTaxCode : tax.TaxCodes.FirstOrDefault(f => f.Id == inputLine.TaxCodeId);
                po.AddReceiveLine(_guidGenerator.Create(), inputLine.LineNum, inputLine.Description, inputLine.Quantity,
                inputLine.Discount, inputLine.DiscountIsPercent, inputLine.LocationId, inputLine.ProductId, DateTime.Now, inputLine.UnitPrice, taxCode);
            }

            foreach (var inputLine in input.ServiceLines ?? Enumerable.Empty<CreatePurchaseOrderServiceLineDto>())
            {
                var taxCode = inputLine.TaxCodeId == Guid.Empty ? defaultTaxCode : tax.TaxCodes.FirstOrDefault(f => f.Id == inputLine.TaxCodeId);
                po.AddOrderLine(_guidGenerator.Create(), inputLine.LineNum, inputLine.Description, inputLine.Quantity,
                    inputLine.UnitPrice, inputLine.Discount, inputLine.DiscountIsPercent, inputLine.ProductId, taxCode);
            }

            await _purchaseOrderManager.CreateAsync(po);

            var result = ObjectMapper.Map<PurchaseOrder, PurchaseOrderDto>(po);

            result.ServiceLines.RemoveAll(r => result.ProductLines.Select(s => s.LineNum).Contains(r.LineNum));

            var user = await _userRepository.FirstOrDefaultAsync(f => f.Id == input.VendorId);
            result.VendorName = user.Name + " " + user.Surname;

            foreach (var item in result.ProductLines)
            {
                item.ReceviceLineId = item.Id;

                var product = await _productRepository.FirstOrDefaultAsync(f => f.Id == item.ProductId);
                item.ProductName = product.Name;

                var o = po.OrderLines.FirstOrDefault(f => f.LineNum == item.LineNum);
                item.OrderLineId = o.Id;
                item.UnitPrice = o.UnitPrice;
                item.SubTotal = o.SubTotal;
                item.Discount = o.Discount;
            }

            foreach (var item in result.ServiceLines)
            {
                item.ProductName = (await _productRepository.FirstOrDefaultAsync(f => f.Id == item.ProductId)).Name;
            }

            return result;
        }

        [Authorize(OrderServicePermissions.PurchaseOrder.Update)]
        public async Task<PurchaseOrderDto> UpdateAsync(Guid id, UpdatePurchaseOrderDto input)
        {
            var po = _purchaseOrderRepository.WithDetails().FirstOrDefault(f => f.Id == id);

            //Update PO
            po.OrderRemarks = input.OrderRemarks;
            po.InventoryStatus = input.InventoryStatus;
            po.PaymentStatus = input.PaymentStatus;


            var tax = _taxSchemeRepository.WithDetails(w => w.TaxCodes).FirstOrDefault(f => f.Id == input.TaxSchemeId);
            var defaultTaxCode = tax.TaxCodes.FirstOrDefault(f => f.Id == tax.DefaultTaxCodeId);
            po.SetBalance(input.AmountPaid, tax);

            //Update Product Lines
            foreach (var inputLine in input.ProductLines ?? Enumerable.Empty<UpdatePurchaseOrderProductLineDto>())
            {
                //Create Line
                if (inputLine.IsDeleted != true && !po.ReceiveLines.Any(a => a.Id == inputLine.ReceviceLineId))
                {
                    var taxCode = inputLine.TaxCodeId == Guid.Empty ? defaultTaxCode : tax.TaxCodes.FirstOrDefault(f => f.Id == inputLine.TaxCodeId);
                    po.AddReceiveLine(_guidGenerator.Create(), inputLine.LineNum, inputLine.Description, inputLine.Quantity, inputLine.Discount, inputLine.DiscountIsPercent,
                       inputLine.LocationId, inputLine.ProductId, DateTime.Now, inputLine.UnitPrice, taxCode);
                }
                //Update Line
                else
                {
                    po.SetReceiveLine(inputLine.ReceviceLineId, inputLine.OrderLineId, inputLine.LineNum, inputLine.Description,
                          inputLine.Quantity, inputLine.Discount, inputLine.DiscountIsPercent, inputLine.LocationId, inputLine.UnitPrice);
                    var r = po.ReceiveLines.FirstOrDefault(f => f.Id == inputLine.ReceviceLineId);


                    if (inputLine.IsDeleted)
                    {
                        po.RemoveReceiveLine(inputLine.ReceviceLineId, inputLine.OrderLineId);
                    }
                }
            }

            //Update Service Lines
            foreach (var inputLine in input.ServiceLines ?? Enumerable.Empty<UpdatePurchaseOrderServiceLineDto>())
            {
                //Create Line
                if (inputLine.IsDeleted != true && !po.OrderLines.Any(a => a.Id == inputLine.Id))
                {
                    var taxCode = inputLine.TaxCodeId == Guid.Empty ? defaultTaxCode : tax.TaxCodes.FirstOrDefault(f => f.Id == inputLine.TaxCodeId);
                    po.AddOrderLine(_guidGenerator.Create(), inputLine.LineNum, inputLine.Description,
                                      inputLine.Quantity, inputLine.UnitPrice, inputLine.Discount, inputLine.DiscountIsPercent, inputLine.ProductId, taxCode);
                }
                //Update Line
                else
                {
                    po.SetOrderLine(inputLine.Id, inputLine.LineNum, inputLine.Description,
                    inputLine.Quantity, inputLine.Discount, inputLine.DiscountIsPercent, inputLine.UnitPrice);

                    if (inputLine.IsDeleted)
                    {
                        po.RemoveOrderLine(inputLine.Id);
                    }
                }
            }

            await _purchaseOrderManager.UpdateAsync(po);

            var result = ObjectMapper.Map<PurchaseOrder, PurchaseOrderDto>(po);

            result.ServiceLines.RemoveAll(r => result.ProductLines.Select(s => s.LineNum).Contains(r.LineNum));

            var user = await _userRepository.FirstOrDefaultAsync(f => f.Id == input.VendorId);
            result.VendorName = user.Name + " " + user.Surname;

            foreach (var item in result.ProductLines)
            {
                item.ReceviceLineId = item.Id;

                var product = await _productRepository.FirstOrDefaultAsync(f => f.Id == item.ProductId);
                item.ProductName = product.Name;

                var o = po.OrderLines.FirstOrDefault(f => f.LineNum == item.LineNum);
                item.OrderLineId = o.Id;
                item.UnitPrice = o.UnitPrice;
                item.SubTotal = o.SubTotal;
                item.Discount = o.Discount;
            }

            foreach (var item in result.ServiceLines)
            {
                item.ProductName = (await _productRepository.FirstOrDefaultAsync(f => f.Id == item.ProductId)).Name;
            }

            return result;
        }

        [Authorize(OrderServicePermissions.PurchaseOrder.Delete)]
        public async Task DeleteAsync(string key)
        {
            await _purchaseOrderManager.DeleteAsync(key);
        }

        [Authorize(OrderServicePermissions.PurchaseOrder.Undo)]
        public async Task UndoAsync(Guid id)
        {
            await _purchaseOrderManager.UndoAsync(id);
        }

        public async Task<PurchaseOrderDto> GetAsync(Guid id, bool isDeleted)
        {
            var po = new PurchaseOrderDto();

            var purchaseOrderRepository = await _purchaseOrderRepository.GetQueryableAsync();
            var productRepository = await _productRepository.GetQueryableAsync();
            var userRepository = await _userRepository.GetQueryableAsync();

            using (_dataFilter.Disable<ISoftDelete>())
            {
                po = (from s in purchaseOrderRepository
                      from u in userRepository.Where(w => w.Id == s.VendorId)
                      where s.IsDeleted == isDeleted
                      select new PurchaseOrderDto
                      {
                          Id = s.Id,
                          VendorId = s.VendorId,
                          OrderNumber = s.OrderNumber,
                          LocationId = s.LocationId,
                          AmountPaid = s.AmountPaid,
                          Balance = s.Balance,
                          CreationTime = s.CreationTime,
                          VendorName = u.Name + " " + u.Surname,
                          OrderDate = s.OrderDate,
                          InventoryStatus = s.InventoryStatus,
                          PaymentStatus = s.PaymentStatus,
                          OrderRemarks = s.OrderRemarks,
                          Total = s.Total,
                          //IsCancelled = s.IsDeleted,
                          IsDeleted = s.IsDeleted
                      }).FirstOrDefault(w => w.Id == id);

                #region Lines
                //var productLines = (from po1 in purchaseOrderRepository
                //                    from poLine in po1.OrderLines.Where(w => w.PurchaseOrderId == id)
                //                    from poReceiveLine in po1.ReceiveLines.Where(w => w.PurchaseOrderId == id)
                //                    from product in productRepository.Where(w => w.Id == poLine.ProductId)
                //                    where product.ProductType != ProductType.Service && poLine.LineNum == poReceiveLine.LineNum
                //                    select new PurchaseOrderProductLineDto
                //                    {
                //                        Id = poLine.Id,
                //                        OrderLineId = poLine.Id,
                //                        ReceviceLineId = poReceiveLine.Id,
                //                        ProductId = poLine.ProductId,
                //                        ProductName = product.Name,
                //                        ProductIsDeleted = product.IsDeleted,
                //                        Description = poLine.Description,
                //                        LocationId = poReceiveLine.LocationId,
                //                        Quantity = poReceiveLine.Quantity,
                //                        UnitPrice = poLine.UnitPrice,
                //                        SubTotal = poLine.SubTotal,
                //                        LineNum = poLine.LineNum,
                //                        Discount = poLine.Discount,
                //                        //ServiceCompleted = poLine.ServiceCompleted
                //                        //IsDeleted = poLine.IsDeleted
                //                    }).OrderBy(o => o.LineNum).ToList();

                //var serviceLines = (from po1 in purchaseOrderRepository
                //                    from soLine in po1.OrderLines.Where(w => w.PurchaseOrderId == id)
                //                    from product in productRepository.Where(w => w.Id == soLine.ProductId)
                //                    where product.ProductType == ProductType.Service
                //                    select new PurchaseOrderServiceLineDto
                //                    {
                //                        Id = soLine.Id,
                //                        ProductId = soLine.ProductId,
                //                        ProductName = product.Name,
                //                        //ProductIsDeleted = product.IsDeleted,
                //                        Description = soLine.Description,
                //                        UnitPrice = soLine.UnitPrice,
                //                        SubTotal = soLine.SubTotal,
                //                        LineNum = soLine.LineNum,
                //                        Quantity = soLine.Quantity,
                //                        Discount = soLine.Discount,
                //                        ServiceCompleted = soLine.ServiceCompleted
                //                        //IsDeleted = soLine.IsDeleted
                //                    }).ToList();

                //po.ServiceLines = serviceLines;
                //po.ProductLines = productLines;
                #endregion
            }
            return await Task.FromResult(po);
        }

        [Authorize(OrderServicePermissions.PurchaseOrder.List)]
        public async Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            var purchaseOrderRepository = await _purchaseOrderRepository.GetQueryableAsync();
            var userRepository = await _userRepository.GetQueryableAsync();

            loadOptions.PrimaryKey = new[] { "Id" };

            using (_dataFilter.Disable<ISoftDelete>())
            {
                var query = 
                   from po in purchaseOrderRepository
                   from vendor in userRepository.Where(w => w.Id == po.VendorId)
                   select new PurchaseOrderListDto
                   {
                       CreationTime = po.CreationTime,
                       Id = po.Id,
                       OrderDate = po.OrderDate,
                       OrderNumber = po.OrderNumber,
                       InventoryStatus = po.InventoryStatus,
                       PaymentStatus = po.PaymentStatus,
                       LocationId = po.LocationId,
                       Total = po.Total,
                       AmountPaid = po.AmountPaid,
                       Balance = po.Balance,
                       VendorName = vendor.Name + " " + vendor.Surname,
                       VendorId = po.VendorId,
                       OrderRemarks = po.OrderRemarks,
                       IsDeleted = po.IsDeleted
                   };

                return await DataSourceLoader.LoadAsync(query, loadOptions);
            }
        }

        public async Task<List<StatusDto>> GetStatusListAsync()
        {
            var data = new List<StatusDto>();

            foreach (var line in Enum.GetValues(typeof(PurchaseOrderStatus)))
            {
                data.Add(new StatusDto { Id = (int)line, Name = _localizer["Status:" + line.ToString()].Value });
            }

            return await Task.FromResult(data);
        }

        public async Task<List<StatusDto>> GetInventoryStatusListAsync()
        {
            var data = new List<StatusDto>();

            foreach (var line in Enum.GetValues(typeof(PurchaseOrderInventoryStatus)))
            {
                data.Add(new StatusDto { Id = (int)line, Name = _localizer["Status:" + line.ToString()].Value });
            }

            return await Task.FromResult(data);
        }

        public async Task<List<StatusDto>> GetPaymentStatusListAsync()
        {
            var data = new List<StatusDto>();

            foreach (var line in Enum.GetValues(typeof(PurchaseOrderPaymentStatus)))
            {
                data.Add(new StatusDto { Id = (int)line, Name = _localizer["Status:" + line.ToString()].Value });
            }

            return await Task.FromResult(data);
        }
    }
}