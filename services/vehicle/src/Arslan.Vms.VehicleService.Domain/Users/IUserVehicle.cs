using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.VehicleService.Users
{
    public interface IUserVehicle : IMultiTenant
    {
        public Guid UserId { get; }
        public Guid VehicleId { get; }
    }
}