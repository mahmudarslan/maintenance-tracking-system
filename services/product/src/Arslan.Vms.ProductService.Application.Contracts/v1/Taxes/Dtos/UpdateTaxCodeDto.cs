using System;

namespace Arslan.Vms.ProductService.v1.Taxes.Dtos
{
    public class UpdateTaxCodeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Tax1Rate { get; set; }
        public decimal Tax2Rate { get; set; }
    }
}