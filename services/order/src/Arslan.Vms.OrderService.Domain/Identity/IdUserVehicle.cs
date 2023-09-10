//using Arslan.Vms.Base.Users;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Arslan.Vms.OrderService.Identity
{
    public class UserVehicle : FullAuditedEntity//, //IUserVehicle
    {
        public Guid? TenantId { get; set; }
        public virtual Guid AppUserId { get; set; }
        public virtual Guid VehicleId { get; set; }

        public virtual IdVehicle Vehicle { get; protected set; }

        public override object[] GetKeys()
        {
            return new object[] { AppUserId, VehicleId };
        }
    }
}