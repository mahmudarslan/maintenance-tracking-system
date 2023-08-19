using Arslan.Vms.OrderService.DocumentNoFormats;
using Arslan.Vms.OrderService.Files;
using Arslan.Vms.OrderService.Inventory;
using Arslan.Vms.OrderService.PurchaseOrders;
using Arslan.Vms.OrderService.PurchaseOrders.Versions;
using Arslan.Vms.OrderService.SalesOrders;
using Arslan.Vms.OrderService.SalesOrders.Versions;
using Arslan.Vms.OrderService.Taxes;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.OrderService.EntityFrameworkCore;

[ConnectionStringName(OrderServiceDbProperties.ConnectionStringName)]
public class OrderServiceDbContext : AbpDbContext<OrderServiceDbContext>, IOrderServiceDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    #region Purchase Order
    public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
    public DbSet<PurchaseOrderVersion> PurchaseOrderVersion { get; set; }
    public DbSet<PurchaseOrderLine> PurchaseOrderLine { get; set; }
    public DbSet<PurchaseOrderLineVersion> PurchaseOrderLineVersion { get; set; }
    public DbSet<PurchaseOrderReceiveLine> PurchaseOrderReceiveLine { get; set; }
    public DbSet<PurchaseOrderReceiveLineVersion> PurchaseOrderReceiveLineVersion { get; set; }
    public DbSet<PurchaseOrderPayment> PurchaseOrderPayment { get; set; }
    public DbSet<PurchaseOrderAttachment> PurchaseOrderAttachment { get; set; }
    #endregion

    #region Sales Order
    public DbSet<SalesOrder> SalesOrder { get; set; }
    public DbSet<SalesOrderVersion> SalesOrderVersion { get; set; }
    public DbSet<SalesOrderLine> SalesOrderLine { get; set; }
    public DbSet<SalesOrderLineVersion> SalesOrderLineVersion { get; set; }
    public DbSet<SalesOrderPickLine> SalesOrderPickLine { get; set; }
    public DbSet<SalesOrderPickLineVersion> SalesOrderPickLineVersion { get; set; }
    public DbSet<SalesOrderAttachment> SalesOrderAttachment { get; set; }
    public DbSet<SalesOrderPayment> SalesOrderPayment { get; set; }

    public DbSet<WorkOrderType> WorkorderType { get; set; }
    #endregion

    #region Attachment
    public DbSet<OrderAttachment> OrderAttachments { get; set; }
    #endregion    

    #region Inventory
    public DbSet<OrderProduct> OrderProduct { get; set; }
    #endregion

    #region InvDocNoFormat
    public DbSet<OrderDocNoFormat> OrderDocNoFormat { get; set; }
    #endregion

    public DbSet<TaxingScheme> TaxingSchemes { get; set; }
    public DbSet<TaxCode> TaxCodes { get; set; }

    public OrderServiceDbContext(DbContextOptions<OrderServiceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureOrderService();
    }
}
