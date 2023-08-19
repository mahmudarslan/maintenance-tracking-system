using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Addresses
{
    public class Address : FullAuditedEntity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual int Version { get; protected set; }
        public virtual Guid? CountryId { get; set; }
        public virtual Guid? CityId { get; set; }
        public virtual Guid? DistrictId { get; set; }
        public virtual AddressKind AddressType { get; set; }
        public virtual string AddressName { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Remarks { get; set; }
        public virtual string CountryName { get; set; }
        public virtual string CityName { get; set; }
        public virtual string DistrictName { get; set; }
        public virtual bool IsDefaultAddress { get; set; }
        public virtual bool IsBillingAddress { get; set; }
        public virtual bool IsShippingAddress { get; set; }

        protected Address() { }

        public Address(Guid id, Guid? tenantId, AddressKind addressType, int version,
            Guid? countryId, Guid? cityId, Guid? districtId,
            string addressName, string address1, string address2,
            string postalCode, string remarks
            ) : base(id)
        {
            TenantId = tenantId;
            Version = 1;
            AddressName = addressName;
            Address1 = address1;
            Address2 = address2;
            PostalCode = postalCode;
            Remarks = remarks;
            Version = version;
            CountryId = countryId;
            CityId = cityId;
            DistrictId = districtId;
            AddressType = addressType;
        }
    }
}