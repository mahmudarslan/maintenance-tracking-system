using JetBrains.Annotations;
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.VehicleService.Vehicles.VehicleTypes
{
    public class VehicleType : BasicAggregateRoot<Guid>, IVehicleType, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid? ParentId { get; protected set; }
        public virtual int Level { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual bool IsDeleted { get; set; }

        protected VehicleType() { }

        public VehicleType(Guid id, Guid? tenant, Guid? parentId, [NotNull] string name, int level) : base(id)
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