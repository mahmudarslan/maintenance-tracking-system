using Arslan.Vms.AdministrationService.v1.Addresses.Dtos;
using System;
using System.Collections.Generic;

namespace Arslan.Vms.AdministrationService.v1.Company.Dtos
{
    public class CreateUpdateCompanyDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string TaxNumber { get; set; }
        public string WebsiteUrl { get; set; }
        public Guid? FakeId { get; set; }
        public List<UpdateAddressDto> Addresses { get; set; }
    }
}