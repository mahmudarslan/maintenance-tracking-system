using System;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.InventoryService.StockAdjustments
{
    public interface IStockAdjustmentRepository : IRepository<StockAdjustment, Guid>
    {
    }
}