using System.ComponentModel;

namespace Arslan.Vms.OrderService.PurchaseOrders
{
    public enum PurchaseOrderPaymentStatus
    {
        [Description("Unpaid")]
        Unpaid = 3,
        [Description("Partial")]
        Partial = 4,
        [Description("Paid")]
        Paid = 5,
        [Description("Owing")]
        Owing = 6
    }
}