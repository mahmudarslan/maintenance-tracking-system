using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.VehicleService.Vehicles
{
    public interface IVehicle : IAggregateRoot<Guid>, IMultiTenant
    {
        public string Plate { get; }
        public string Color { get; }
        public string Motor { get; }
        public string Chassis { get; }
        public Guid ModelId { get; }
    }
}