using System;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.VehicleService.Users
{
    public interface IRoleRepository : IRepository<Role, Guid>
    {
    }
}