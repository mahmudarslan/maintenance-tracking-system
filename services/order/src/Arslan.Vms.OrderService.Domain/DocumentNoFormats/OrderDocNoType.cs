using System.ComponentModel.DataAnnotations;

namespace Arslan.Vms.OrderService.DocumentNoFormats
{
    public enum OrderDocNoType
    {
        [Display(Name = "Sales Order")]
        SalesOrder = 1,

        [Display(Name = "Sales Quote")]
        SalesQuote = 2,

        [Display(Name = "Purchase Order")]
        PurchaseOrder = 3,
    }
}