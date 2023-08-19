using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.AdministrationService.Users
{
    public class AppRoleRepository : EfCoreRepository<AdministrationServiceDbContext, Role, Guid>, IRoleRepository
    {
        public AppRoleRepository(IDbContextProvider<AdministrationServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}