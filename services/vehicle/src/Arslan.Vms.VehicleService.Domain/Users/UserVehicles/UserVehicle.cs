using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.VehicleService.Users.UserVehicles
{
    public class UserVehicle : FullAuditedEntity, IMultiTenant, IUserVehicle
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid UserId { get; protected set; }
        public virtual Guid VehicleId { get; protected set; }

        protected UserVehicle() { }

        public UserVehicle(Guid? tenantId, Guid userId, Guid vehicleId)
        {
            TenantId = tenantId;
            UserId = userId;
            VehicleId = vehicleId;
        }

        public override object[] GetKeys()
        {
            return new object[] { UserId, VehicleId };
        }
    }
}