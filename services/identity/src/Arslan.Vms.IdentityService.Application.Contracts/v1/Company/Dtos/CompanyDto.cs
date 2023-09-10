using Arslan.Vms.IdentityService.v1.Files.Dtos;
using Arslan.Vms.IdentityService.v1.Addresses.Dtos;
using System;
using System.Collections.Generic;

namespace Arslan.Vms.IdentityService.v1.Company.Dtos
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string TaxNumber { get; set; }
        public string WebsiteUrl { get; set; }
        public List<AddressDto> Addresses { get; set; }
        public List<FileAttachmentDto> Attachments { get; set; }
    }
}