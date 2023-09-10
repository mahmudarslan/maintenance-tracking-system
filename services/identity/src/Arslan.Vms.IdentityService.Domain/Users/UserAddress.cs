using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.IdentityService.Users
{
    public class UserAddress : Entity, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public Guid UserId { get; protected set; }
        public Guid AddressId { get; protected set; }

        protected UserAddress() { }

        public UserAddress(Guid? tenantId, Guid userId, Guid addressId)
        {
            TenantId = tenantId;
            UserId = userId;
            AddressId = addressId;
        }

        public override object[] GetKeys()
        {
            return new object[] { UserId, AddressId };
        }
    }
}