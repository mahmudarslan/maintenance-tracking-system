using System;
using Arslan.Vms.ProductService.Pricing;

namespace Arslan.Vms.ProductService.v1.Products.Dtos
{
    public class ProductPriceDto
    {
        public Guid Id { get; set; }
        public Guid? PricingSchemeId { get; set; }
        public PriceType ItemPriceType { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal FixedMarkup { get; set; }
        public int Version { get; set; }
    }
}