using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Locations
{
    public class Location : BasicAggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid? ParentId { get; protected set; }
        public string Name { get; protected set; }
        public bool IsDeleted { get; set; }
        public virtual int Level { get; protected set; }

        protected Location() { }

        public Location(Guid id, Guid? tenant, Guid? parentId, string name, int level) : base(id)
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