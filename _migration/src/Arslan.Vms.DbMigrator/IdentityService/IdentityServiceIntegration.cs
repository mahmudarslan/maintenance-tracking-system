using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;
using Minio.DataModel.Tags;
using Arslan.Vms.IdentityService.Users;
using System.Collections.Generic;
using Arslan.Vms.IdentityService.Roles;

namespace Arslan.Vms.AdministrationService;

internal class IdentityServiceIntegration
{
    private readonly IRepository<Tenant> _tenantRepository;
    private readonly IRepository<PermissionGrant> _permissionGrantRepository;
    private readonly IRepository<PermissionDefinitionRecord> _permissionDefinitionRecordRepository;
    private readonly IRepository<PermissionGroupDefinitionRecord> _permissionGroupDefinitionRecordRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IDataFilter _dataFilter;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    IDbContextProvider<AdministrationServiceDbContext> _dbContextProvider;
    private readonly IConfiguration _configuration;

    public IdentityServiceIntegration(IAbpApplicationWithInternalServiceProvider application)
    {
        _tenantRepository = application.ServiceProvider.GetRequiredService<IRepository<Tenant>>();
        _permissionGrantRepository = application.ServiceProvider.GetRequiredService<IRepository<PermissionGrant>>();
        _permissionDefinitionRecordRepository = application.ServiceProvider.GetRequiredService<IRepository<PermissionDefinitionRecord>>();
        _permissionGroupDefinitionRecordRepository = application.ServiceProvider.GetRequiredService<IRepository<PermissionGroupDefinitionRecord>>();
        _userRepository = application.ServiceProvider.GetRequiredService<IRepository<User>>();
        _roleRepository = application.ServiceProvider.GetRequiredService<IRepository<Role>>();
        _guidGenerator = application.ServiceProvider.GetRequiredService<IGuidGenerator>();
        _dataFilter = application.ServiceProvider.GetRequiredService<IDataFilter>();
        _unitOfWorkManager = application.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        _dbContextProvider = application.ServiceProvider.GetRequiredService<IDbContextProvider<AdministrationServiceDbContext>>();
        _configuration = application.ServiceProvider.GetRequiredService<IConfiguration>();
    }

    public async Task SeedDataAsync()
    {
        using (_dataFilter.Disable<IMultiTenant>())
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                if (_configuration["Migration:DeleteAllData"] == "true")
                {
                    using (var uow = _unitOfWorkManager.Begin(true))
                    {

                        await uow.CompleteAsync();
                    }
                }

                await CreateRolesAsync(MigrationConst.DemoTenantId);

                await CreateDemoUser1();
            }
        }
    }

    public async Task CreateRolesAsync(Guid tenantId)
    {
        var roles = await _roleRepository.ToListAsync();

        var ls = new List<Role>()
    {
new Role(id: _guidGenerator.Create(), name: RoleConsts.Admin,tenantId: tenantId ),
new Role(id: _guidGenerator.Create(), name: RoleConsts.Owner,tenantId: tenantId ),
new Role(id: _guidGenerator.Create(), name: RoleConsts.Vendor,tenantId: tenantId ),
new Role(id: _guidGenerator.Create(), name: RoleConsts.Customer,tenantId: tenantId ),
new Role(id: _guidGenerator.Create(), name: RoleConsts.HeadTechnician,tenantId: tenantId ),
new Role(id: _guidGenerator.Create(), name: RoleConsts.Technician,tenantId: tenantId ),
    };

        var insertList = new List<Role>();

        foreach (var item in ls)
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin(true))
                {
                    if (!roles.Any(w => w.TenantId == tenantId && w.NormalizedName == item.NormalizedName.ToUpperInvariant()))
                        await _roleRepository.InsertAsync(item, true);

                    await uow.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error("CreateRolesAsync->" + item.Name + ex.Message);
            }
        }
    }

    public async Task CreateDemoUser1()
    {
        try
        {
            using (var uow = _unitOfWorkManager.Begin(true))
            {
                var userId = Guid.Parse("F79FD9CE-CB3D-5B7C-744C-3A12373104F6");

                var anyUser = await _userRepository.AnyAsync(a => a.TenantId == MigrationConst.DemoTenantId && a.Id == userId);

                if (!anyUser)
                {

                    var user = new User(
                                   id: userId,
                                   tenantId: MigrationConst.DemoTenantId,
                                   userName: "901111111111",
                                   name: "demo",
                                   surName: "use",
                                   email: "demo@user.io");



                    await uow.CompleteAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error("CreateDemoUser1->" + ex.Message);
        }
    }
}