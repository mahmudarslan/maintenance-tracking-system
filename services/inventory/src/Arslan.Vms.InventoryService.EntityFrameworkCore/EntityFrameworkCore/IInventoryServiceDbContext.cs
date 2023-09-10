using Arslan.Vms.InventoryService.StockAdjustments;
using Arslan.Vms.InventoryService.StockAdjustments.Versions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.InventoryService.EntityFrameworkCore;

[ConnectionStringName(InventoryServiceDbProperties.ConnectionStringName)]
public interface IInventoryServiceDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */

    #region StockAdjustment
    DbSet<StockAdjustment> StockAdjustment { get; set; }
    DbSet<StockAdjustmentAttachment> StockAdjustmentAttachment { get; set; }
    DbSet<StockAdjustmentLine> StockAdjustmentLine { get; set; }
    DbSet<StockAdjustmentLineVersion> StockAdjustmentLineVersion { get; set; }
    DbSet<StockAdjustmentVersion> StockAdjustmentVersion { get; set; }
    #endregion
}
