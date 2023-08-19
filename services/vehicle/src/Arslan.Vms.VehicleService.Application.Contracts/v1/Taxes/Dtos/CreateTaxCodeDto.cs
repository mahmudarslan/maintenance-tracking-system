using System;

namespace Arslan.Vms.VehicleService.v1.Taxes.Dtos
{
    public class CreateTaxCodeDto
    {
        public string Name { get; set; }
        public decimal Tax1Rate { get; set; }
        public decimal Tax2Rate { get; set; }
    }
}