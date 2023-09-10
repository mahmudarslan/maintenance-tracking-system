using System.ComponentModel;

namespace Arslan.Vms.ProductService.Pricing
{
    public enum PriceType
    {
        [Description("FixedPrice")]
        FixedPrice = 1,
        [Description("FixedMarkup")]
        FixedMarkup = 2
    }
}