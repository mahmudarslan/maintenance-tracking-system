using System;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.AdministrationService.Users
{
    public interface IRoleRepository : IRepository<Role, Guid>
    {
    }
}