using System;
using Volo.Abp.Application.Dtos;

namespace Arslan.Vms.ProductService.v1.Products.Dtos
{
    public class CreateUpdateProductTaxDto : EntityDto<Guid>
    {
        public Guid TaxingSchemeId { get; set; }
        public Guid? TaxCodeId { get; set; }
    }
}