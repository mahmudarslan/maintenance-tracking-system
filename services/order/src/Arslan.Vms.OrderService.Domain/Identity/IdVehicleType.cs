//using Arslan.Vms.Base.VehicleTypes;
using System;
using Volo.Abp.Domain.Entities;

namespace Arslan.Vms.OrderService.Identity
{
    public class IdVehicleType : BasicAggregateRoot<Guid>//, IVehicleType
    {
        public virtual Guid? TenantId { get; set; }
        public virtual Guid? ParentId { get; set; }
        public virtual int Level { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}