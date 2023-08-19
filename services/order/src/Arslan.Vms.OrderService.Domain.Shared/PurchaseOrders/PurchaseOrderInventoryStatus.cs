using System.ComponentModel;

namespace Arslan.Vms.OrderService.PurchaseOrders
{
    public enum PurchaseOrderInventoryStatus
    {
        [Description("Unfulfilled")]
        Unfulfilled = 2,
        [Description("Started")]
        Started = 3,
        [Description("Fulfilled")]
        Fulfilled = 4
    }
}