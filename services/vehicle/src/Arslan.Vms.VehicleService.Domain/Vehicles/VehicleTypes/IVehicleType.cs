using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.VehicleService.Vehicles.VehicleTypes
{
    public interface IVehicleType : IAggregateRoot<Guid>, IMultiTenant, ISoftDelete
    {
        public string Name { get; }
        public int Level { get; }
        public Guid? ParentId { get; }
    }
}