using System.ComponentModel;

namespace Arslan.Vms.OrderService.SalseOrders
{
    public enum SalesOrderInventoryStatus
    {
        [Description("Quote")]
        Quote = 1,
        [Description("Unfulfilled")]
        Unfulfilled = 2,
        [Description("Started")]
        Started = 3,
        [Description("Fulfilled")]
        Fulfilled = 4
    }
}