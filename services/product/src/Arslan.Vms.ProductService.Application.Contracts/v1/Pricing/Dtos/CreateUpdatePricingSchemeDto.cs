using System;

namespace Arslan.Vms.ProductService.v1.Pricing.Dtos
{
    public class CreateUpdatePricingSchemeDto
    {
        public Guid CurrencyId { get; set; }
        public string Name { get; set; }
        public bool IsTaxInclusive { get; set; }
    }
}