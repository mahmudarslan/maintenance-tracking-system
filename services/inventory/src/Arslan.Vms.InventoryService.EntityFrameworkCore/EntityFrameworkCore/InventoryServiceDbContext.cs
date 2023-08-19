using Arslan.Vms.InventoryService.DocumentNoFormats;
using Arslan.Vms.InventoryService.FileAttachments;
using Arslan.Vms.InventoryService.StockAdjustments;
using Arslan.Vms.InventoryService.StockAdjustments.Versions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.InventoryService.EntityFrameworkCore;

[ConnectionStringName(InventoryServiceDbProperties.ConnectionStringName)]
public class InventoryServiceDbContext : AbpDbContext<InventoryServiceDbContext>, IInventoryServiceDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    #region StockAdjustment
    public DbSet<StockAdjustment> StockAdjustment { get; set; }
    public DbSet<StockAdjustmentAttachment> StockAdjustmentAttachment { get; set; }
    public DbSet<StockAdjustmentLine> StockAdjustmentLine { get; set; }
    public DbSet<StockAdjustmentLineVersion> StockAdjustmentLineVersion { get; set; }
    public DbSet<StockAdjustmentVersion> StockAdjustmentVersion { get; set; }
    #endregion

    #region Attachment
    public DbSet<InventoryAttachment> FileAttachment { get; set; }
    #endregion

    #region InvDocNoFormat
    public DbSet<InvDocNoFormat> InvDocNoFormat { get; set; }
    #endregion

    public InventoryServiceDbContext(DbContextOptions<InventoryServiceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureInventoryService();
    }
}
