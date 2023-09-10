using Arslan.Vms.IdentityService.Addresses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.IdentityService.Users
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<List<Address>> GetAddresses(Guid userId);
        //Task<List<Vehicle>> GetVehicles(Guid userId);
        //Task UndoVehicleAsync(Guid userId, Guid vehicleId);
    }
}