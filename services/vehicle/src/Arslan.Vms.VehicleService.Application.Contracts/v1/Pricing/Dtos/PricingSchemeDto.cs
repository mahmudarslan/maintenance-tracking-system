using System;

namespace Arslan.Vms.VehicleService.v1.Pricing.Dtos
{
    public class PricingSchemeDto
    {
        public Guid Id { get; set; }
        public Guid CurrencyId { get; set; }
        public string Name { get; set; }
        public bool IsTaxInclusive { get; set; }
    }
}