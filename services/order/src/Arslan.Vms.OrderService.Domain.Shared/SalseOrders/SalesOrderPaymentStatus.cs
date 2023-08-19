using System.ComponentModel;

namespace Arslan.Vms.OrderService.SalseOrders
{
    public enum SalesOrderPaymentStatus
    {
        [Description("Quote")]
        Quote = 1,
        [Description("Uninvoiced")]
        Uninvoiced = 2,
        [Description("Invoiced")]
        Invoiced = 3,
        [Description("Partial")]
        Partial = 4,
        [Description("Paid")]
        Paid = 5,
        [Description("Owing")]
        Owing = 6
    }
}