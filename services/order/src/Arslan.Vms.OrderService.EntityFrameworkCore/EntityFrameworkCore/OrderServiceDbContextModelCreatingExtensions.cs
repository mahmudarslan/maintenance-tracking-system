using Arslan.Vms.OrderService.PurchaseOrders.Versions;
using Arslan.Vms.OrderService.PurchaseOrders;
using Arslan.Vms.OrderService.SalesOrders.Versions;
using Arslan.Vms.OrderService.SalesOrders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Arslan.Vms.OrderService.DocumentNoFormats;

namespace Arslan.Vms.OrderService.EntityFrameworkCore;

public static class OrderServiceDbContextModelCreatingExtensions
{
    public static void ConfigureOrderService(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(OrderServiceDbProperties.DbTablePrefix + "Questions", OrderServiceDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */

        #region Purchase Order
        builder.Entity<PurchaseOrder>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "PurchaseOrders", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<PurchaseOrderAttachment>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "PurchaseOrderAttachments", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<PurchaseOrderLine>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "PurchaseOrderLines", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<PurchaseOrderLineVersion>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "PurchaseOrderLineVersions", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<PurchaseOrderReceiveLine>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "PurchaseOrderReceiveLines", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<PurchaseOrderReceiveLineVersion>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "PurchaseOrderReceiveLineVersions", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<PurchaseOrderPayment>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "PurchaseOrderPayments", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<PurchaseOrderVersion>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "PurchaseOrderVersions", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        #endregion

        #region Sales Order
        builder.Entity<SalesOrder>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "SalesOrders", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<SalesOrderAttachment>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "SalesOrderAttachments", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<SalesOrderLine>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "SalesOrderLines", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<SalesOrderLineVersion>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "SalesOrderLineVersions", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<SalesOrderPickLine>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "SalesOrderPickLines", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<SalesOrderPickLineVersion>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "SalesOrderPickLineVersions", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<SalesOrderPayment>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "SalesOrderPayments", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<SalesOrderVersion>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "SalesOrderVersions", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });

        builder.Entity<WorkOrderType>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "WorkorderTypes", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });

        builder.Entity<OrderDocNoFormat>(b =>
        {
            b.ToTable(OrdersDbProperties.DbTablePrefix + "OrderDocNoFormats", OrdersDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        #endregion
    }
}
