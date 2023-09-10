using System.ComponentModel.DataAnnotations;

namespace Arslan.Vms.InventoryService.DocumentNoFormats
{
    public enum InvDocNoType
    {
        [Display(Name = "Adjust Stock")]
        StockAdjust = 6,
    }
}