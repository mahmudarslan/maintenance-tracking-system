using System;
using System.Collections.Generic;

namespace Arslan.Vms.ProductService.v1.Taxes.Dtos
{
    public class UpdateTaxingSchemeDto
    {
        public Guid? DefaultTaxCodeId { get; set; }
        public string Name { get; set; }
        public string Tax1Name { get; set; }
        public string Tax2Name { get; set; }
        public bool CalculateTax2OnTax1 { get; set; }

        public List<UpdateTaxCodeDto> TaxCodes { get; set; }
    }
}