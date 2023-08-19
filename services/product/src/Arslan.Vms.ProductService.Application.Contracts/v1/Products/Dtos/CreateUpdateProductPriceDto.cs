using System;
using System.ComponentModel.DataAnnotations;
using Arslan.Vms.ProductService.Pricing;
using Volo.Abp.Application.Dtos;

namespace Arslan.Vms.ProductService.v1.Products.Dtos
{
    public class CreateUpdateProductPriceDto : EntityDto<Guid>
    {
        [Required]
        public Guid PricingSchemeId { get; set; }
        [Required]
        public PriceType ItemPriceType { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal FixedMarkup { get; set; }
    }
}