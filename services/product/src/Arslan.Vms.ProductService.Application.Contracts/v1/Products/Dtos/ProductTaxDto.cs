using System;
using Volo.Abp.Application.Dtos;

namespace Arslan.Vms.ProductService.v1.Products.Dtos
{
    public class CreateUpdateTaxPriceDto : EntityDto<Guid>
    {
        public decimal UnitPrice { get; set; }
    }
}