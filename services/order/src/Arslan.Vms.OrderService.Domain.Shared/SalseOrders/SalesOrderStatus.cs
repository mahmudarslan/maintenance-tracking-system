using System.ComponentModel;

namespace Arslan.Vms.OrderService.SalseOrders
{
    public enum SalesOrderStatus
    {
        [Description("Quote")]
        Quote = 1,
        [Description("Open")]
        Open = 2,
        [Description("In Progress")]
        InProgress = 3,
        [Description("Invoiced")]
        Invoiced = 5,
        [Description("Paid")]
        Paid = 6,
        [Description("Cancelled")]
        Cancelled = 10
    }
}