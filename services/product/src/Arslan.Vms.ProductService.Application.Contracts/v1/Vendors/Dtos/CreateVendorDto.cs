using Arslan.Vms.ProductService.v1.Address.Dtos;
using System;
using System.Collections.Generic;

namespace Arslan.Vms.ProductService.v1.Vendors.Dtos
{
    public class CreateVendorDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string HomePhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public virtual DateTime? BirthDate { get; set; }

        public List<CreateAddressDto> Addresses { get; set; }
    }
}