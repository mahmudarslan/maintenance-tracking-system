using Arslan.Vms.OrderService.PurchaseOrders.Versions;
using Arslan.Vms.OrderService.PurchaseOrders;
using Arslan.Vms.OrderService.SalesOrders.Versions;
using Arslan.Vms.OrderService.SalesOrders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Arslan.Vms.OrderService.DocumentNoFormats;

namespace Arslan.Vms.OrderService.EntityFrameworkCore;

[ConnectionStringName(OrderServiceDbProperties.ConnectionStringName)]
public interface IOrderServiceDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */

    #region Purchase Order
    DbSet<PurchaseOrder> PurchaseOrder { get; set; }
    DbSet<PurchaseOrderVersion> PurchaseOrderVersion { get; set; }
    DbSet<PurchaseOrderLine> PurchaseOrderLine { get; set; }
    DbSet<PurchaseOrderLineVersion> PurchaseOrderLineVersion { get; set; }
    DbSet<PurchaseOrderReceiveLine> PurchaseOrderReceiveLine { get; set; }
    DbSet<PurchaseOrderReceiveLineVersion> PurchaseOrderReceiveLineVersion { get; set; }
    DbSet<PurchaseOrderPayment> PurchaseOrderPayment { get; set; }
    DbSet<PurchaseOrderAttachment> PurchaseOrderAttachment { get; set; }
    #endregion

    #region Sales Order
    DbSet<SalesOrder> SalesOrder { get; set; }
    DbSet<SalesOrderVersion> SalesOrderVersion { get; set; }
    DbSet<SalesOrderLine> SalesOrderLine { get; set; }
    DbSet<SalesOrderLineVersion> SalesOrderLineVersion { get; set; }
    DbSet<SalesOrderPickLine> SalesOrderPickLine { get; set; }
    DbSet<SalesOrderPickLineVersion> SalesOrderPickLineVersion { get; set; }
    DbSet<SalesOrderAttachment> SalesOrderAttachment { get; set; }
    DbSet<SalesOrderPayment> SalesOrderPayment { get; set; }

    DbSet<WorkOrderType> WorkorderType { get; set; }
    #endregion

    public DbSet<OrderDocNoFormat> OrderDocNoFormat { get; set; }
}
