using Arslan.Vms.IdentityService.v1.Addresses.Dtos;
using System;
using System.Collections.Generic;

namespace Arslan.Vms.IdentityService.v1.Vendors.Dtos
{
    public class VendorDto
    {
        public Guid Id { get; set; }
        public Guid? PaymentTermId { get; set; }
        public Guid? CurrencyId { get; set; }
        public Guid? TaxingSchemeId { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public decimal Discount { get; set; }
        public bool IsTaxInclusivePricing { get; set; }
        public string Remarks { get; set; }
        public string Website { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string HomePhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }

        //public List<CreateUpdateVendorLineDto> Lines { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }
}