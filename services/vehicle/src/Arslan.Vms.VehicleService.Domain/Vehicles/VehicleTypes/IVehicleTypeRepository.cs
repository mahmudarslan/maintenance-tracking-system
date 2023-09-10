using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.VehicleService.Vehicles.VehicleTypes
{
    public interface IVehicleTypeRepository : IRepository<VehicleType, Guid>
    {
        Task AddRangeAsync(List<VehicleType> entityList, CancellationToken cancellationToken = default);
        void UpdateRange(List<VehicleType> entityList);
    }
}