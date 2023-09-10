using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.VehicleService.Addresses.AddressTypes
{
    public interface IAddressTypeRepository : IRepository<AddressType, Guid>
    {
        Task AddRangeAsync(List<AddressType> entityList, CancellationToken cancellationToken = default);
        void UpdateRange(List<AddressType> entityList);
    }
}