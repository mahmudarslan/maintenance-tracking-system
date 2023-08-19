using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.AdministrationService.Addresses.AddressTypes
{
    public class AddressType : BasicAggregateRoot<Guid>, IAddressType, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid? ParentId { get; protected set; }
        public virtual int Level { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual bool IsDeleted { get; set; }

        protected AddressType() { }

        public AddressType(Guid id, Guid? tenant, Guid? parentId, string name, int level) : base(id)
        {
            TenantId = tenant;
            ParentId = parentId;
            Name = name;
            Level = level;
        }

        public void Set(Guid? parentId, string name, int level)
        {
            ParentId = parentId;
            Name = name;
            Level = level;
        }
    }
}