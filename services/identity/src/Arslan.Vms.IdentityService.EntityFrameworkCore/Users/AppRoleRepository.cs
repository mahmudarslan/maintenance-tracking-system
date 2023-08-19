using Arslan.Vms.IdentityService.EntityFrameworkCore;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.IdentityService.Users
{
    public class AppRoleRepository : EfCoreRepository<AdministrationServiceDbContext, Role, Guid>, IRoleRepository
    {
        public AppRoleRepository(IDbContextProvider<AdministrationServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}