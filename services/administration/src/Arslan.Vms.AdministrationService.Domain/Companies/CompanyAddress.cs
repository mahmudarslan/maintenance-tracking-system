using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.AdministrationService.Companies
{
    public class CompanyAddress : Entity, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid CompanyId { get; protected set; }
        public virtual Guid AddressId { get; protected set; }

        protected CompanyAddress() { }

        public CompanyAddress(Guid? tenantId, Guid companyId, Guid addressId)
        {
            TenantId = tenantId;
            AddressId = addressId;
            CompanyId = companyId;
        }

        public override object[] GetKeys()
        {
            return new object[] { CompanyId, AddressId };
        }
    }
}