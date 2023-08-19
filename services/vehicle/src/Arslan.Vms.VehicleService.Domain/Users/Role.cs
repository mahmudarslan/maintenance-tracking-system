using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.VehicleService.Users
{
    public class Role : AggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual string Name { get; protected internal set; }
        public virtual string NormalizedName { get; protected internal set; }
        public virtual bool IsDefault { get; set; }
        public virtual bool IsStatic { get; set; }
        public virtual bool IsPublic { get; set; }

        protected Role() { }

        public Role(Guid id, string name, Guid? tenantId) : base(id)
        {
            Name = name;
            NormalizedName = name.ToUpperInvariant();
            TenantId = tenantId;
        }
    }
}