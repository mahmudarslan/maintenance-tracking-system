using System.ComponentModel.DataAnnotations;

namespace Arslan.Vms.ProductService.Products
{
    public enum ProductType
    {
        [Display(Name = "Stockable")]
        StockedProduct = 1,

        [Display(Name = "Non-Stocked")]
        NonStockedProduct = 2,

        [Display(Name = "Service")]
        Service = 3
    }
}