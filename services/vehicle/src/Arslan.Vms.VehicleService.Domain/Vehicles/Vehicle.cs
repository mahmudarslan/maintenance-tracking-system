using JetBrains.Annotations;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.VehicleService.Vehicles
{
    public class Vehicle : FullAuditedEntity<Guid>, IMultiTenant, IVehicle
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid ModelId { get; protected set; }
        public virtual string Plate { get; protected set; }
        public virtual string Chassis { get; protected set; }
        public virtual string Color { get; protected set; }
        public virtual string Motor { get; protected set; }

        protected Vehicle() { }

        public Vehicle(Guid id,
            Guid? tenantId,
            [NotNull] string plate,
            string color,
            string motor,
            string chassis,
            Guid modelId) : base(id)
        {
            TenantId = tenantId;
            Plate = plate;
            Color = color;
            Motor = motor;
            Chassis = chassis;
            ModelId = modelId;
        }

        public void SetModelId(Guid id)
        {
            ModelId = id;
        }

        public void SetPlate([NotNull] string plate)
        {
            Plate = plate;
        }

        public void SetChassis(string chassis)
        {
            Chassis = chassis;
        }

        public void SetColor(string color)
        {
            Color = color;
        }

        public void SetMotor(string motor)
        {
            Motor = motor;
        }
    }
}