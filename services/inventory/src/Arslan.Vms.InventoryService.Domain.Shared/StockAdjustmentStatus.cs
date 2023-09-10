using System.ComponentModel;

namespace Arslan.Vms.InventoryService
{
    public enum StockAdjustmentStatus
    {
        [Description("Completed")]
        Complated = 1,
        [Description("Cancelled")]
        Cancelled = 2
    }
}