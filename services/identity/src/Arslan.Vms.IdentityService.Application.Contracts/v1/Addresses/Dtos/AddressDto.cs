using Arslan.Vms.IdentityService.Addresses;
using System;
using System.ComponentModel;

namespace Arslan.Vms.IdentityService.v1.Addresses.Dtos
{
    public class AddressDto
    {
        public Guid Id { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? DistrictId { get; set; }
        public AddressKind AddressType { get; set; }
        public string AddressName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string Remarks { get; set; }
        [DisplayName("Country")]
        public string CountryName { get; set; }
        [DisplayName("City")]
        public string CityName { get; set; }
        [DisplayName("District")]
        public string DistrictName { get; set; }
        public bool IsDefaultAddress { get; set; }
        public bool IsBillingAddress { get; set; }
        public bool IsShippingAddress { get; set; }
    }
}