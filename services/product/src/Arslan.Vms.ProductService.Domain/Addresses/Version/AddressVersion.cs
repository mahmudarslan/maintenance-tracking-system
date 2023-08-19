using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Addresses.Version
{
    public class AddressVersion : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid AddressId { get; protected set; }
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

        protected AddressVersion() { }

        public AddressVersion(Guid id, Address a) : base(id)
        {
            AddressId = a.Id;
            TenantId = a.TenantId;
            Version = a.Version;
            CountryId = a.CountryId;
            CityId = a.CityId;
            DistrictId = a.DistrictId;
            AddressType = a.AddressType;
            AddressName = a.AddressName;
            Address1 = a.Address1;
            Address2 = a.Address2;
            PostalCode = a.PostalCode;
            Remarks = a.Remarks;
            CountryName = a.CountryName;
            CityName = a.CityName;
            DistrictName = a.DistrictName;
            IsDefaultAddress = a.IsDefaultAddress;
            IsBillingAddress = a.IsBillingAddress;
            IsShippingAddress = a.IsShippingAddress;
        }
    }
}