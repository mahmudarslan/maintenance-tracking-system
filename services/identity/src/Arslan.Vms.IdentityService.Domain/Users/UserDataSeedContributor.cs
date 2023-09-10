using Arslan.Vms.IdentityService.Roles;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Arslan.Vms.IdentityService.Users
{
    public class UserDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Role, Guid> _appRoleRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public UserDataSeedContributor(
            IRepository<Role, Guid> appRoleRepository,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
            _appRoleRepository = appRoleRepository;
            _currentTenant = currentTenant;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            var roles = await _appRoleRepository.GetListAsync();

            if (!roles.Any(a => a.NormalizedName == RoleConsts.Owner)) await _appRoleRepository.InsertAsync(new Role(_guidGenerator.Create(), RoleConsts.Owner.ToLowerInvariant(), _currentTenant.Id));
            if (!roles.Any(a => a.NormalizedName == RoleConsts.HeadTechnician)) await _appRoleRepository.InsertAsync(new Role(_guidGenerator.Create(), RoleConsts.HeadTechnician.ToLowerInvariant(), _currentTenant.Id));
            if (!roles.Any(a => a.NormalizedName == RoleConsts.Technician)) await _appRoleRepository.InsertAsync(new Role(_guidGenerator.Create(), RoleConsts.Technician.ToLowerInvariant(), _currentTenant.Id));
            if (!roles.Any(a => a.NormalizedName == RoleConsts.Customer)) await _appRoleRepository.InsertAsync(new Role(_guidGenerator.Create(), RoleConsts.Customer.ToLowerInvariant(), _currentTenant.Id));
            if (!roles.Any(a => a.NormalizedName == RoleConsts.Vendor)) await _appRoleRepository.InsertAsync(new Role(_guidGenerator.Create(), RoleConsts.Vendor.ToLowerInvariant(), _currentTenant.Id));
        }
    }
}