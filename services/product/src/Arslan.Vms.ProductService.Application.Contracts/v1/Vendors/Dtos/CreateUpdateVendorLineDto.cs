using System;
using System.ComponentModel.DataAnnotations;

namespace Arslan.Vms.ProductService.v1.Vendors.Dtos
{
    public class CreateUpdateVendorLineDto
    {
        public Guid? VendorId { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
    }
}