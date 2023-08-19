using System;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.IdentityService.Users
{
    public interface IRoleRepository : IRepository<Role, Guid>
    {
    }
}