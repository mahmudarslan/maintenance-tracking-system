using System.ComponentModel;

namespace Arslan.Vms.OrderService.Accounts.Transactions
{
    public enum OrderTransactionType
    {
        [Description("Purchase Order")]
        PurchaseOrder = 600,
        [Description("Purchase Order Receive")]
        PurchaseOrderReceive = 601,
        [Description("Purchase Order Invoice")]
        PurchaseOrderInvoice = 602,
        [Description("Purchase Order Payment")]
        PurchaseOrderPayments = 603,
        [Description("Purchase Order Unstock")]
        PurchaseOrderUnstock = 605,
        [Description("Purchase Order Service")]
        PurchaseOrderServiceDone = 606,

        [Description("Sales Order")]
        SalesOrder = 800,
        [Description("Sales Order Service")]
        SalesOrderServiceDone = 801,
        [Description("Sales Order Picking")]
        SalesOrderPicking = 802,
        [Description("Sales Order Fulfillment")]
        SalesOrderFulfillment = 804,
        [Description("Sales Order Invoice")]
        SalesOrderInvoice = 805,
        [Description("Sales Order Payment")]
        SalesOrderPayments = 806,
    }
}
