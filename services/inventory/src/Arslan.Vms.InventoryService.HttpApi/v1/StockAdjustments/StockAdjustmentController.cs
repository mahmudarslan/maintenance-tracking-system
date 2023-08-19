using Arslan.Vms.Inventory;
using Arslan.Vms.InventoryService.v1;
using Arslan.Vms.InventoryService.v1.CurrentStocks.Dtos;
using Arslan.Vms.InventoryService.v1.StockAdjustments.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace Arslan.Vms.InventoryService.v1.StockAdjustments
{
    [ApiController]
    //[RemoteService(Name = "ArslanVmsInventory")]
    [Area("Inventory")]
    [ControllerName("StockAdjustment")]
    [Route("rest/api/latest/vms/inventory/stockadjustment")]
    //[ApiVersion("1.0")]
    public class StockAdjustmentController : InventoryController, IStockAdjustmentAppService
    {
        protected IStockAdjustmentAppService _stockAdjustmentAppService { get; }

        public StockAdjustmentController(IStockAdjustmentAppService stockAdjustmentAppService)
        {
            _stockAdjustmentAppService = stockAdjustmentAppService;
        }

        [HttpPost]
        public virtual Task<StockAdjustmentDto> CreateAsync(CreateStockAdjustmentDto input)
        {
            ValidateModel();
            return _stockAdjustmentAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<StockAdjustmentDto> UpdateAsync(Guid id, UpdateStockAdjustmentDto input)
        {
            return _stockAdjustmentAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public virtual Task DeleteAsync([FromForm(Name = "key")] string key)
        {
            return _stockAdjustmentAppService.DeleteAsync(key);
        }

        [HttpPost]
        [Route("Undo")]
        public virtual Task UndoAsync(Guid id)
        {
            return _stockAdjustmentAppService.UndoAsync(id);
        }

        [HttpGet]
        [Route("{id}/{isDeleted}")]
        public virtual Task<StockAdjustmentDto> GetAsync(Guid id, bool isDeleted = false)
        {
            return _stockAdjustmentAppService.GetAsync(id, isDeleted);
        }

        [HttpGet]
        public virtual Task<LoadResult> GetListAsync(DataSourceLoadOptions input)
        {
            return _stockAdjustmentAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("CurrentStock")]
        public virtual Task<List<CurrentStockDto>> GetCurrentStockAsync(Guid Id)
        {
            return _stockAdjustmentAppService.GetCurrentStockAsync(Id);
        }

        [HttpGet]
        [Route("CurrentStockWithLocation")]
        public virtual Task<CurrentStockDto> GetCurrentStockAsync(Guid Id, Guid locationId)
        {
            return _stockAdjustmentAppService.GetCurrentStockAsync(Id, locationId);
        }

        [HttpGet]
        [Route("CurrentStocks")]
        public virtual Task<LoadResult> GetCurrentStockListAsync(DataSourceLoadOptions loadOptions)
        {
            return _stockAdjustmentAppService.GetCurrentStockListAsync(loadOptions);
        }
    }
}