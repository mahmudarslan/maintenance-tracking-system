using Arslan.Vms.Inventory;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Arslan.Vms.InventoryService.StockAdjustments.Versions;
using Arslan.Vms.InventoryService.StockAdjustments;

namespace Arslan.Vms.InventoryService.EntityFrameworkCore;

public static class InventoryServiceDbContextModelCreatingExtensions
{
    public static void ConfigureInventoryService(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(InventoryServiceDbProperties.DbTablePrefix + "Questions", InventoryServiceDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */

        #region StockAdjustment 
        builder.Entity<StockAdjustment>(b =>
        {
            b.ToTable(InventoryServiceDbProperties.DbTablePrefix + "StockAdjustments", InventoryServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<StockAdjustmentAttachment>(b =>
        {
            b.ToTable(InventoryServiceDbProperties.DbTablePrefix + "StockAdjustmentAttachments", InventoryServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<StockAdjustmentLine>(b =>
        {
            b.ToTable(InventoryServiceDbProperties.DbTablePrefix + "StockAdjustmentLines", InventoryServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<StockAdjustmentLineVersion>(b =>
        {
            b.ToTable(InventoryServiceDbProperties.DbTablePrefix + "StockAdjustmentLineVersions", InventoryServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<StockAdjustmentVersion>(b =>
        {
            b.ToTable(InventoryServiceDbProperties.DbTablePrefix + "StockAdjustmentVersions", InventoryServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        #endregion
    }
}
