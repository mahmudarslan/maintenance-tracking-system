//using Arslan.Vms.Base.Products;
using Arslan.Vms.InventoryService.Accounts;
using Arslan.Vms.InventoryService.DocumentNoFormats;
using Arslan.Vms.InventoryService.Localization;
using Arslan.Vms.InventoryService.Permissions;
using Arslan.Vms.InventoryService.StockAdjustments;
using Arslan.Vms.InventoryService.v1.CurrentStocks.Dtos;
using Arslan.Vms.InventoryService.v1.StockAdjustments.Dtos;
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
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.InventoryService.v1.StockAdjustments
{
    [Authorize(InventoryServicePermissions.AdjustStock.Default)]
    public class StockAdjustmentAppService : InventoryServiceAppService, IStockAdjustmentAppService
    {
        private readonly DocNoFormatManager _docNoFormatManager;
        private readonly StockAdjustmentManager _stockAdjustmentManager;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IStockAdjustmentRepository _stockAdjustmentRepository;
        private readonly IRepository<Account, Guid> _accountRepository;
        private readonly IRepository<Balances, Guid> _balanceRepository;
        //private readonly IRepository<Product, Guid> _productRepository;
        private readonly IObjectMapper _objectMapper;
        private readonly IDataFilter _dataFilter;
        private readonly IStringLocalizer<InventoryServiceResource> _localizer;

        public StockAdjustmentAppService(IStockAdjustmentRepository stockAdjustmentRepository,
            //IRepository<Product, Guid> productRepository,
            IRepository<Account, Guid> accountRepository,
            IRepository<Balances, Guid> balanceRepository,
            StockAdjustmentManager stockAdjustmentManager,
            DocNoFormatManager docNoFormatManager,
            IStringLocalizer<InventoryServiceResource> localizer,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator,
            IObjectMapper objectMapper,
            IDataFilter dataFilter)
        {
            _stockAdjustmentManager = stockAdjustmentManager;
            _stockAdjustmentRepository = stockAdjustmentRepository;
            //_productRepository = productRepository;
            _docNoFormatManager = docNoFormatManager;
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            _objectMapper = objectMapper;
            _accountRepository = accountRepository;
            _balanceRepository = balanceRepository;
            _localizer = localizer;
            _dataFilter = dataFilter;
        }

        [Authorize(InventoryServicePermissions.AdjustStock.Create)]
        public async Task<StockAdjustmentDto> CreateAsync(CreateStockAdjustmentDto input)
        {
            if (input.Lines == null || input.Lines.Count <= 0)
            {
                throw new UserFriendlyException("StockAdjustment items not null", "", "", null, Microsoft.Extensions.Logging.LogLevel.Error);
            }

            //var productTags = _productTagRepository.Where(w => input.Lines.Select(s => s.ProductId).Contains(w.ProductId)).ToList();

            var stockAdjustment = new StockAdjustment(_guidGenerator.Create(), _currentTenant.Id, _docNoFormatManager, input.Remarks);

            foreach (var inputLine in input.Lines)
            {
                stockAdjustment.AddLine(_guidGenerator.Create(), inputLine.ProductId, inputLine.LocationId, inputLine.QuantityBefore, inputLine.QuantityAfter);
            }

            await _stockAdjustmentManager.CreateAsync(stockAdjustment);

            return ObjectMapper.Map<StockAdjustment, StockAdjustmentDto>(stockAdjustment);
        }

        [Authorize(InventoryServicePermissions.AdjustStock.Update)]
        public async Task<StockAdjustmentDto> UpdateAsync(Guid id, UpdateStockAdjustmentDto input)
        {
            //TODO isdeleted not action
            if (input.Lines == null || input.Lines.Count <= 0)
            {
                throw new UserFriendlyException("StockAdjustment items not null", "", "", null, Microsoft.Extensions.Logging.LogLevel.Error);
            }

            var stockAdjustment = _stockAdjustmentRepository.WithDetails().FirstOrDefault(f => f.Id == id);

            //Update
            stockAdjustment.Remarks = input.Remarks;

            foreach (var inputLine in input.Lines)//TODO:: Deleted Items
            {
                if (inputLine.Id == Guid.Empty)
                {
                    stockAdjustment.AddLine(_guidGenerator.Create(), inputLine.ProductId, inputLine.LocationId, inputLine.QuantityBefore, inputLine.QuantityAfter);
                }
                else
                {
                    var selectedStockAdjustmentLine = stockAdjustment.Lines.FirstOrDefault(f => f.Id == inputLine.Id);
                    selectedStockAdjustmentLine.SetQuantity(inputLine.QuantityBefore, inputLine.QuantityAfter);
                    //selectedStockAdjustmentLine.IsDeleted = inputLine.IsDeleted;
                }
            }

            await _stockAdjustmentManager.UpdateAsync(stockAdjustment);

            return ObjectMapper.Map<StockAdjustment, StockAdjustmentDto>(stockAdjustment);
        }

        [Authorize(InventoryServicePermissions.AdjustStock.Delete)]
        public async Task DeleteAsync(string key)
        {
            await _stockAdjustmentManager.DeleteAsync(key);
        }

        public async Task UndoAsync(Guid id)
        {
            await _stockAdjustmentManager.UndoAsync(id);
        }

        public async Task<StockAdjustmentDto> GetAsync(Guid id, bool isDeleted)
        {
            var result = new StockAdjustmentDto();

            using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
            {
                var dbData = _stockAdjustmentRepository.WithDetails().Where(w => w.Id == id).FirstOrDefault();

                result = new StockAdjustmentDto
                {
                    AdjustmentNumber = dbData.AdjustmentNumber,
                    CreationTime = dbData.CreationTime,
                    Id = dbData.Id,
                    Remarks = dbData.Remarks,
                    IsCancelled = dbData.IsCancelled,
                    IsDeleted = dbData.IsDeleted,
                };

                if (result == null)
                {
                    return null;
                }

                result.Lines = dbData.Lines.Select(s => new StockAdjustmentLineDto
                {
                    Description = s.Description,
                    Difference = s.Difference,
                    Id = s.Id,
                    //IsDeleted = s.IsDeleted,
                    LocationId = s.LocationId,
                    ProductId = s.ProductId,
                    QuantityAfter = s.QuantityAfter,
                    QuantityBefore = s.QuantityBefore,
                    StockAdjustmentId = s.StockAdjustmentId
                }).ToList();


                if (result.IsDeleted == true)
                {
                    result.Status = _localizer["Status:" + Enum.GetName(typeof(StockAdjustmentStatus), StockAdjustmentStatus.Cancelled)].Value;
                }
                else
                {
                    result.Status = _localizer["Status:" + Enum.GetName(typeof(StockAdjustmentStatus), StockAdjustmentStatus.Complated)].Value;
                }

                return await Task.FromResult(result);
            }
        }

        [Authorize(InventoryServicePermissions.AdjustStock.List)]
        public async Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                loadOptions.PrimaryKey = new[] { "Id" };

                var stockAdjustmentRepository = await _stockAdjustmentRepository.GetQueryableAsync();

                var ls = from stock in stockAdjustmentRepository
                         select new StockAdjustmentDto
                         {
                             Id = stock.Id,
                             CreationTime = stock.CreationTime,
                             AdjustmentNumber = stock.AdjustmentNumber,
                             Remarks = stock.Remarks,
                             IsDeleted = stock.IsDeleted
                         };

                return await DataSourceLoader.LoadAsync(ls, loadOptions);
            }
        }

        public async Task<List<CurrentStockDto>> GetCurrentStockAsync(Guid productId)
        {
            //var productRepository = await _productRepository.GetQueryableAsync();
            var accountRepository = await _accountRepository.GetQueryableAsync();
            var balanceRepository = await _balanceRepository.GetQueryableAsync();

            //var data = (from product in productRepository
            //                //from productTag in _productTagRepository
            //            from account in accountRepository
            //            from balance in balanceRepository
            //            where
            //            product.Id == account.ProductId
            //            //&& account.ProductTagId == productTag.Id
            //            && account.AccountType == (int)InventoryAccountType.InvOnHand
            //            && balance.AccountId == account.Id
            //            && balance.Balance != 0
            //            //&& product.TrackSerials == false
            //            && product.ProductType == ProductType.StockedProduct
            //            && product.Id == productId
            //            select new CurrentStockDto
            //            {
            //                Id = product.Id,
            //                Name = product.Name,
            //                LocationId = account.Attr1.Value,
            //                ProductCategoryId = product.ProductCategoryId,
            //                Quantity = balance.Balance
            //            }).ToList();

            return null;//await Task.FromResult(data);
        }

        public async Task<CurrentStockDto> GetCurrentStockAsync(Guid productId, Guid locationId)
        {
            //var productRepository = await _productRepository.GetQueryableAsync();
            //var accountRepository = await _accountRepository.GetQueryableAsync();
            //var balanceRepository = await _balanceRepository.GetQueryableAsync();

            //var data = (from product in productRepository
            //                //from productTag in _productTagRepository
            //            from account in accountRepository
            //            from balance in balanceRepository
            //            where
            //            product.Id == account.ProductId
            //            //&& account.ProductTagId == productTag.Id
            //            && account.AccountType == (int)InventoryAccountType.InvOnHand
            //            && balance.AccountId == account.Id
            //            && balance.Balance != 0
            //            //&& product.TrackSerials == false
            //            && product.ProductType == ProductType.StockedProduct
            //            && product.Id == productId
            //            && account.Attr1.Value == locationId
            //            select new CurrentStockDto
            //            {
            //                Id = product.Id,
            //                Name = product.Name,
            //                LocationId = account.Attr1.Value,
            //                ProductCategoryId = product.ProductCategoryId,
            //                Quantity = balance.Balance
            //            }).FirstOrDefault();

            //return await Task.FromResult(data);

            return null;
        }

        public async Task<LoadResult> GetCurrentStockListAsync(DataSourceLoadOptions loadOptions)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                //var productRepository = await _productRepository.GetQueryableAsync();
                //var accountRepository = await _accountRepository.GetQueryableAsync();
                //var balanceRepository = await _balanceRepository.GetQueryableAsync();

                //var ls = (from product in productRepository
                //              //from productTag in _productTagRepository
                //          from account in accountRepository
                //          from balance in balanceRepository
                //          where
                //          product.Id == account.ProductId
                //          //&& account.ProductTagId == productTag.Id
                //          && account.AccountType == (int)InventoryAccountType.InvOnHand
                //          && balance.AccountId == account.Id
                //          && balance.Balance != 0
                //          //&& product.TrackSerials == false
                //          && product.ProductType == ProductType.StockedProduct
                //          select new CurrentStockDto
                //          {
                //              Id = product.Id,
                //              Name = product.Name,
                //              LocationId = account.Attr1.Value,
                //              ProductCategoryId = product.ProductCategoryId,
                //              Quantity = balance != null ? balance.Balance : 0
                //          });

                //return await DataSourceLoader.LoadAsync(ls, loadOptions);

                return null;
            }
        }

        //public async Task<List<StockAdjustmentsStatusDto>> GetStatusAsync()
        //{
        //    var data = new List<StockAdjustmentsStatusDto>();

        //    foreach (var item in Enum.GetValues(typeof(StockAdjustmentStatus)))
        //    {
        //        data.Add(new StockAdjustmentsStatusDto { Id = (int)item, Name = _localizer["Status:" + item.ToString()].Value });
        //    }

        //    return await Task.FromResult(data);
        //}
    }
}