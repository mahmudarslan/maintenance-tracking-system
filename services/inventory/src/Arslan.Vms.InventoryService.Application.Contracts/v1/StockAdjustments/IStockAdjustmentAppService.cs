using Arslan.Vms.InventoryService.v1.CurrentStocks.Dtos;
using Arslan.Vms.InventoryService.v1.StockAdjustments.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.InventoryService.v1.StockAdjustments
{
	public interface IStockAdjustmentAppService
    {
        Task<StockAdjustmentDto> CreateAsync(CreateStockAdjustmentDto input);
        Task<StockAdjustmentDto> UpdateAsync(Guid id, UpdateStockAdjustmentDto input);
        Task DeleteAsync(  string key);
        Task UndoAsync(Guid id);
        Task<StockAdjustmentDto> GetAsync(Guid id, bool isDeleted = false);
        Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions);
        Task<LoadResult> GetCurrentStockListAsync(DataSourceLoadOptions loadOptions);
        Task<List<CurrentStockDto>> GetCurrentStockAsync(Guid productId);
        Task<CurrentStockDto> GetCurrentStockAsync(Guid productId, Guid locationId);
    }
}