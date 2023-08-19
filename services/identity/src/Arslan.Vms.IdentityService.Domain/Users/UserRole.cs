using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.IdentityService.Users
{
    public class UserRole : Entity, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid UserId { get; protected set; }
        public virtual Guid RoleId { get; protected set; }

        protected UserRole() { }

        public UserRole(Guid? tenantId, Guid userId, Guid roleId)
        {
            RoleId = roleId;
            UserId = userId;
            TenantId = tenantId;
        }

        public override object[] GetKeys()
        {
            return new object[] { UserId, RoleId };
        }
    }
}