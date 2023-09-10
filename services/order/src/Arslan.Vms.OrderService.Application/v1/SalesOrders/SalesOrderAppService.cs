using Arslan.Vms.OrderService.DocumentNoFormats;
using Arslan.Vms.OrderService.Files;
using Arslan.Vms.OrderService.Identity;
using Arslan.Vms.OrderService.Inventory;
using Arslan.Vms.OrderService.Localization;
using Arslan.Vms.OrderService.Permissions;
using Arslan.Vms.OrderService.SalesOrders;
using Arslan.Vms.OrderService.SalseOrders;
using Arslan.Vms.OrderService.Taxes;
using Arslan.Vms.OrderService.v1.Dtos;
using Arslan.Vms.OrderService.v1.Files.Dtos;
using Arslan.Vms.OrderService.v1.SalesOrders.Dtos;
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

namespace Arslan.Vms.OrderService.v1.SalesOrders
{
    [Authorize(OrderServicePermissions.SalesOrder.Default)]
    public class SalesOrderAppService : OrderServiceAppService, ISalesOrderAppService
    {
        #region Fields
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly SalesOrderManager _salesOrderManager;
        private readonly IRepository<WorkOrderType, Guid> _workorderTypeRepository;
        private readonly IRepository<OrderProduct, Guid> _productRepository;
        private readonly IRepository<AppUser, Guid> _userRepository;
        private readonly IRepository<TaxingScheme, Guid> _taxSchemeRepository;
        private readonly IRepository<IdVehicleType, Guid> _vehicleTypeRepository;
        private readonly IRepository<OrderAttachment, Guid> _fileAttachmentRepository;
        private readonly DocNoFormatManager _docNoFormatManager;
        private readonly IStringLocalizer<OrderServiceResource> _localizer;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IDataFilter _dataFilter;
        #endregion

        #region Ctor
        public SalesOrderAppService(
     ISalesOrderRepository salesOrderRepository,
     IRepository<AppUser, Guid> userRepository,
     IRepository<TaxingScheme, Guid> taxSchemeRepository,
     IRepository<OrderProduct, Guid> productRepository,
     IRepository<OrderAttachment, Guid> fileAttachmentRepository,
     IRepository<IdVehicleType, Guid> vehicleTypeRepository,
     IRepository<WorkOrderType, Guid> workorderTypeRepository,
     ICurrentTenant currentTenant,
     IDataFilter dataFilter,
     IGuidGenerator guidGenerator,
     IStringLocalizer<OrderServiceResource> localizer,
     DocNoFormatManager docNoFormatManager,
     SalesOrderManager salesOrderManager
    )
        {
            _salesOrderRepository = salesOrderRepository;
            _docNoFormatManager = docNoFormatManager;
            _currentTenant = currentTenant;
            _productRepository = productRepository;
            _fileAttachmentRepository = fileAttachmentRepository;
            _dataFilter = dataFilter;
            _guidGenerator = guidGenerator;
            _localizer = localizer;
            _userRepository = userRepository;
            _taxSchemeRepository = taxSchemeRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
            _workorderTypeRepository = workorderTypeRepository;
            _salesOrderManager = salesOrderManager;
        }
        #endregion

        [Authorize(OrderServicePermissions.SalesOrder.Create)]
        public async Task<SalesOrderDto> CreateAsync(CreateSalesOrderDto input)
        {
            var fileAttachmentRepository = await _fileAttachmentRepository.GetQueryableAsync();

            //Create Sales Order
            var so = new SalesOrder(_guidGenerator.Create(), _currentTenant.Id, input.OrderDate, input.CustomerId,
                input.LocationId, input.PaymentStatus, input.InventoryStatus, input.HeadTechnicianId,
                input.VehicleId, input.WorkorderTypeId, input.VehicleReceiveDate,
                input.Description, input.Notes, input.Kilometrage, input.VehicleReceiveFrom, _docNoFormatManager,
                input.TaxingSchemeId, input.CurrencyId);

            so.AmountPaid = input.AmountPaid;

            var tax = _taxSchemeRepository.WithDetails(w => w.TaxCodes).FirstOrDefault(f => f.Id == input.TaxingSchemeId);
            var defaultTaxCode = tax.TaxCodes.FirstOrDefault(f => f.Id == tax.DefaultTaxCodeId);

            //Create Product Lines
            foreach (var inputLine in input.ProductLines ?? Enumerable.Empty<CreateSalesOrderProductLineDto>())
            {
                var product = await _productRepository.GetAsync(inputLine.ProductId);
                var taxCode = inputLine.TaxCodeId == Guid.Empty ? defaultTaxCode : tax.TaxCodes.FirstOrDefault(f => f.Id == inputLine.TaxCodeId);

                so.AddPickLine(_guidGenerator.Create(), product, inputLine.LineNum, inputLine.Description, inputLine.Quantity,
                  inputLine.Discount, inputLine.DiscountIsPercent, inputLine.LocationId, inputLine.ProductId, inputLine.PickDate, inputLine.UnitPrice, taxCode);
            }

            //Create Serivce Lines
            foreach (var inputLine in input.ServiceLines ?? Enumerable.Empty<CreateSalesOrderServiceLineDto>())
            {
                var product = await _productRepository.GetAsync(inputLine.ProductId);
                var taxCode = inputLine.TaxCodeId == Guid.Empty ? defaultTaxCode : tax.TaxCodes.FirstOrDefault(f => f.Id == inputLine.TaxCodeId);

                so.AddOrderLine(_guidGenerator.Create(), product, inputLine.LineNum, inputLine.Description, inputLine.Quantity,
                    inputLine.UnitPrice, inputLine.Discount, inputLine.DiscountIsPercent, inputLine.ProductId, inputLine.TechnicianId, taxCode);
            }

            //Create File Attachments
            var files = fileAttachmentRepository.Where(w => w.DownloadGuid == input.FakeId).ToList();

            foreach (var file in files ?? Enumerable.Empty<OrderService.Files.OrderAttachment>())
            {
                so.AddAttachment(_guidGenerator.Create(), file.Id);
            }

            await _salesOrderManager.CreateAsync(so);

            var result = ObjectMapper.Map<SalesOrder, SalesOrderDto>(so);


            foreach (var item in result.ServiceLines.Where(w => w.LineNum > 1000))
            {
                var f = result.ProductLines.FirstOrDefault(f => f.LineNum == item.LineNum);
                f.UnitPrice = item.UnitPrice;
                f.Discount = item.Discount;
                f.DiscountIsPercent = item.DiscountIsPercent;
                f.TaxCodeId = item.TaxCodeId;
            }

            result.ServiceLines.RemoveAll(r => result.ProductLines.Select(s => s.LineNum).Contains(r.LineNum));

            return result;
        }

        [Authorize(OrderServicePermissions.SalesOrder.Update)]
        public async Task<SalesOrderDto> UpdateAsync(Guid id, UpdateSalesOrderDto input)
        {
            var fileAttachmentRepository = await _fileAttachmentRepository.GetQueryableAsync();

            var so = await _salesOrderRepository.GetAsync(f => f.Id == id);

            //Update SO
            so.Description = input.Description;
            so.Kilometrage = input.Kilometrage;
            so.VehicleReceiveFrom = input.VehicleReceiveFrom;
            so.VehicleReceiveDate = input.VehicleReceiveDate;
            so.Notes = input.Notes;
            so.HeadTechnicianId = input.HeadTechnicianId;
            so.WorkorderTypeId = input.WorkorderTypeId;
            so.Description = input.Description;
            so.VehicleId = input.VehicleId;
            so.AmountPaid = input.AmountPaid;
            so.ChangeCustomer(input.CustomerId);
            so.ChangeInventoryStatus(input.InventoryStatus);
            so.ChangePaymentStatus(input.PaymentStatus);
            so.ChangeOrderDate(input.OrderDate);

            var tax = _taxSchemeRepository.WithDetails(w => w.TaxCodes).FirstOrDefault(f => f.Id == input.TaxingSchemeId);
            var defaultTaxCode = tax.TaxCodes.FirstOrDefault(f => f.Id == tax.DefaultTaxCodeId);
            so.SetBalance(input.AmountPaid, tax);


            var aa = input.ProductLines.Select(s => s.PickLineId).ToList();
            var cc = so.PickLines.Where(w => !aa.Contains(w.Id)).ToList();
            foreach (var item in cc)
            {
                so.RemovePickLine(item);
            }

            var aa1 = input.ServiceLines.Select(s => s.Id).ToList();
            var cc1 = so.OrderLines.Where(w => !aa.Contains(w.Id)).ToList();
            foreach (var item in cc1)
            {
                so.RemoveOrderLine(item);
            }

            //Update Product Lines
            foreach (var inputLine in input.ProductLines ?? Enumerable.Empty<UpdateSalesOrderProductLineDto>())
            {
                var taxCode = inputLine.TaxCodeId == Guid.Empty ? defaultTaxCode : tax.TaxCodes.FirstOrDefault(f => f.Id == inputLine.TaxCodeId);

                //Create Line
                if (!so.PickLines.Any(a => a.Id == inputLine.PickLineId))
                {
                    var product = await _productRepository.GetAsync(inputLine.ProductId);

                    so.AddPickLine(_guidGenerator.Create(), product, inputLine.LineNum, inputLine.Description, inputLine.Quantity,
                        inputLine.Discount, inputLine.DiscountIsPercent, inputLine.LocationId, inputLine.ProductId, inputLine.PickDate, inputLine.UnitPrice, taxCode);
                }
                //Update Line
                else
                {
                    so.SetPickLine(inputLine.PickLineId, inputLine.OrderLineId, inputLine.LineNum, inputLine.Description,
                        inputLine.Quantity, inputLine.Discount, inputLine.DiscountIsPercent, inputLine.LocationId, inputLine.UnitPrice, taxCode);
                    var orderLine = so.PickLines.FirstOrDefault(f => f.Id == inputLine.PickLineId);

                    orderLine.PickDate = inputLine.PickDate;
                }
            }

            //Update Service Lines
            foreach (var inputLine in input.ServiceLines ?? Enumerable.Empty<UpdateSalesOrderServiceLineDto>())
            {
                var taxCode = inputLine.TaxCodeId == Guid.Empty ? defaultTaxCode : tax.TaxCodes.FirstOrDefault(f => f.Id == inputLine.TaxCodeId);

                //Create Line
                if (!so.OrderLines.Any(a => a.Id == inputLine.Id))
                {
                    var product = await _productRepository.GetAsync(inputLine.ProductId);

                    so.AddOrderLine(_guidGenerator.Create(), product, inputLine.LineNum, inputLine.Description,
                        inputLine.Quantity, inputLine.UnitPrice, inputLine.Discount, inputLine.DiscountIsPercent, inputLine.ProductId, inputLine.TechnicianId, taxCode);
                }
                //Update Line
                else
                {
                    so.SetOrderLine(inputLine.Id, inputLine.LineNum, inputLine.Description,
                      inputLine.Quantity, inputLine.Discount, inputLine.DiscountIsPercent, inputLine.UnitPrice, taxCode);
                }
            }

            #region  Attachment
            //Insert Attachment Files
            var files = fileAttachmentRepository.Where(w => w.DownloadGuid == input.FakeId).ToList();

            foreach (var file in files ?? Enumerable.Empty<OrderService.Files.OrderAttachment>())
            {
                so.AddAttachment(_guidGenerator.Create(), file.Id);
            }

            //Update Attachment Files
            foreach (var Line in input.Files?.Where(w => w.IsDeleted == true) ?? Enumerable.Empty<FileAttachmentDto>())
            {
                var file = fileAttachmentRepository.Where(w => w.Id == Line.Id).FirstOrDefault();
                file.IsDeleted = true;
                await _fileAttachmentRepository.UpdateAsync(file);
            }
            #endregion

            await _salesOrderManager.UpdateAsync(so);

            var result = ObjectMapper.Map<SalesOrder, SalesOrderDto>(so);

            foreach (var item in result.ServiceLines.Where(w => w.LineNum > 1000))
            {
                var f = result.ProductLines.FirstOrDefault(f => f.LineNum == item.LineNum);
                f.UnitPrice = item.UnitPrice;
                f.Discount = item.Discount;
                f.DiscountIsPercent = item.DiscountIsPercent;
                f.TaxCodeId = item.TaxCodeId;
            }

            result.ServiceLines.RemoveAll(r => result.ProductLines.Select(s => s.LineNum).Contains(r.LineNum));

            return result;
        }

        [Authorize(OrderServicePermissions.SalesOrder.Delete)]
        public async Task DeleteAsync(string key)
        {
            await _salesOrderManager.DeleteAsync(key);
        }

        [Authorize(OrderServicePermissions.SalesOrder.Undo)]
        public async Task UndoAsync(Guid id)
        {
            await _salesOrderManager.UndoAsync(id);
        }

        public async Task<SalesOrderDto> GetAsync(Guid id, bool isDeleted = false)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var salesOrderRepository = await _salesOrderRepository.GetQueryableAsync();
                var productRepository = await _productRepository.GetQueryableAsync();
                var fileAttachmentRepository = await _fileAttachmentRepository.GetQueryableAsync();

                var so = (from s in salesOrderRepository
                          where s.IsDeleted == isDeleted
                          select new SalesOrderDto
                          {
                              Id = s.Id,
                              CustomerId = s.CustomerId,
                              OrderNumber = s.OrderNumber,
                              OrderDate = s.OrderDate,
                              InventoryStatus = s.InventoryStatus,
                              PaymentStatus = s.PaymentStatus,
                              OrderRemarks = s.OrderRemarks,
                              Total = s.Total,
                              VehicleId = s.VehicleId,
                              WorkorderTypeId = s.WorkorderTypeId,
                              VehicleReceiveDate = s.VehicleReceiveDate,
                              Kilometrage = s.Kilometrage,
                              Description = s.Description,
                              HeadTechnicianId = s.HeadTechnicianId,
                              VehicleReceiveFrom = s.VehicleReceiveFrom,
                              Notes = s.Notes,
                              AmountPaid = s.AmountPaid,
                              Balance = s.Balance,
                              TaxingSchemeId = s.TaxingSchemeId,
                              CurrencyId = s.CurrencyId,
                              LocationId = s.LocationId,
                              IsDeleted = s.IsDeleted,
                              Version = s.Version
                          }).FirstOrDefault(w => w.Id == id);

                //so.ProductLines = (from so1 in salesOrderRepository
                //                   from poLine in so1.OrderLines
                //                   from poReceiveLine in so1.PickLines
                //                   from product in productRepository.Where(w => w.Id == poLine.ProductId)
                //                   where so1.Id == id
                //                   && product.ProductType != ProductType.Service && poLine.LineNum == poReceiveLine.LineNum
                //                   select new SalesOrderProductLineDto
                //                   {
                //                       Id = poReceiveLine.Id,
                //                       OrderLineId = poLine.Id,
                //                       PickLineId = poReceiveLine.Id,
                //                       ProductId = poLine.ProductId,
                //                       ProductName = product.Name,
                //                       ProductIsDeleted = product.IsDeleted,
                //                       Description = poLine.Description,
                //                       LocationId = poReceiveLine.LocationId,
                //                       Quantity = poReceiveLine.Quantity,
                //                       UnitPrice = poLine.UnitPrice,
                //                       SubTotal = poLine.SubTotal,
                //                       LineNum = poLine.LineNum,
                //                       PickDate = poReceiveLine.PickDate,
                //                       Discount = poLine.Discount,
                //                       DiscountIsPercent = poLine.DiscountIsPercent,
                //                       TaxCodeId = poLine.TaxCodeId
                //                   }).Distinct().OrderBy(o => o.LineNum).ToList();

                //so.ServiceLines = (from so1 in salesOrderRepository
                //                   from soLine in so1.OrderLines
                //                   from product in productRepository.Where(w => w.Id == soLine.ProductId)
                //                   where so1.Id == id && product.ProductType == ProductType.Service
                //                   select new SalesOrderServiceLineDto
                //                   {
                //                       Id = soLine.Id,
                //                       ProductId = soLine.ProductId,
                //                       ProductName = product.Name,
                //                       ProductIsDeleted = product.IsDeleted,
                //                       TechnicianId = soLine.TechnicianId,
                //                       Description = soLine.Description,
                //                       UnitPrice = soLine.UnitPrice,
                //                       SubTotal = soLine.SubTotal,
                //                       LineNum = soLine.LineNum,
                //                       Quantity = soLine.Quantity,
                //                       Discount = soLine.Discount,
                //                       DiscountIsPercent = soLine.DiscountIsPercent,
                //                       TaxCodeId = soLine.TaxCodeId
                //                   }).ToList();

                so.Files = (from so1 in salesOrderRepository
                            from file in fileAttachmentRepository
                            from rfile in so1.Attachments.Where(w => w.FileAttachmentId == file.Id)
                            where so1.Id == id && rfile.SalesOrderId == so.Id
                            select new FileAttachmentDto
                            {
                                Id = file.Id,
                                FileName = file.FileName,
                                Extension = file.Extension,
                                DownloadUrl = $"{file.DownloadUrl}/{file.Id}"
                            }
                        ).ToList();

                return await Task.FromResult(so);
            }
        }

        [Authorize(OrderServicePermissions.SalesOrder.List)]
        public async Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            loadOptions.PrimaryKey = new[] { "Id" };

            using (_dataFilter.Disable<ISoftDelete>())
            {
                var salesOrderRepository = await _salesOrderRepository.GetQueryableAsync();
                var userRepository = await _userRepository.GetQueryableAsync();
                var vehicleTypeRepository = await _vehicleTypeRepository.GetQueryableAsync();

                var query = 
                   from so in salesOrderRepository
                   from user in userRepository.Where(w => w.Id == so.CustomerId)
                   from uv in user.UserVehicles.Where(w => w.VehicleId == so.VehicleId)
                   from vehicleModel in vehicleTypeRepository.Where(w => w.Id == uv.Vehicle.ModelId)
                   from vehicleBrand in vehicleTypeRepository.Where(w => w.Id == vehicleModel.ParentId)

                   select new SalesOrderSearchDto
                   {
                       CreationTime = so.CreationTime,
                       Id = so.Id,
                       VehicleReceiveDate = so.VehicleReceiveDate,
                       OrderNumber = so.OrderNumber,
                       OrderDate = so.OrderDate,
                       Description = so.Description,
                       Name = user.Name + " " + user.Surname,
                       PhoneNumber = user.PhoneNumber,
                       Surname = user.Surname,
                       VehicleModelId = vehicleModel.Id,
                       VehicleBrandId = vehicleModel.ParentId.Value,
                       ModelName = vehicleModel.Name,
                       BrandName = vehicleBrand.Name,
                       PlateNo = uv.Vehicle.Plate,
                       InventoryStatus = so.InventoryStatus,
                       PaymentStatus = so.PaymentStatus,
                       LocationId = so.LocationId,
                       Total = so.Total,
                       AmountPaid = so.AmountPaid,
                       Balance = so.Balance,
                       IsDeleted = so.IsDeleted,
                       Version = so.Version
                   };

                return await DataSourceLoader.LoadAsync(query, loadOptions);
            }
        }

        public async Task<List<WorkStatusDto>> GetWorkorderTypesListAsync()
        {
            var workorderTypeRepository = await _workorderTypeRepository.GetQueryableAsync();

            var result = workorderTypeRepository.Select(s => new WorkStatusDto
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return await Task.FromResult(result);
        }

        public async Task<List<StatusDto>> GetStatusListAsync()
        {
            var data = new List<StatusDto>();

            foreach (var Line in Enum.GetValues(typeof(SalesOrderStatus)))
            {
                data.Add(new StatusDto { Id = (int)Line, Name = _localizer["Status:" + Line.ToString()].Value });
            }

            return await Task.FromResult(data);
        }

        public async Task<List<StatusDto>> GetInventoryStatusListAsync()
        {
            var data = new List<StatusDto>();

            foreach (var line in Enum.GetValues(typeof(SalesOrderInventoryStatus)))
            {
                data.Add(new StatusDto { Id = (int)line, Name = _localizer["Status:" + line.ToString()].Value });
            }

            return await Task.FromResult(data);
        }

        public async Task<List<StatusDto>> GetPaymentStatusListAsync()
        {
            var data = new List<StatusDto>();

            foreach (var line in Enum.GetValues(typeof(SalesOrderPaymentStatus)))
            {
                data.Add(new StatusDto { Id = (int)line, Name = _localizer["Status:" + line.ToString()].Value });
            }

            return await Task.FromResult(data);
        }
    }
}