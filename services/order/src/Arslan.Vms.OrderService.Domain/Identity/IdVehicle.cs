//using Arslan.Vms.Base.Vehicles;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Arslan.Vms.OrderService.Identity
{
    public class IdVehicle : FullAuditedAggregateRoot<Guid>//, IVehicle
    {
        public virtual Guid? TenantId { get; set; }
        public virtual string Plate { get; set; }
        public virtual string Color { get; set; }
        public virtual string Motor { get; set; }
        public virtual string Chassis { get; set; }
        public virtual Guid ModelId { get; set; }
    }
}