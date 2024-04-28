using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using Arslan.Vms.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Medallion.Threading;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;

namespace Arslan.Vms.AdministrationService.DbMigrations;

public class AdministrationServiceDatabaseMigrationChecker
    : PendingEfCoreMigrationsChecker<AdministrationServiceDbContext>
{
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;
    private readonly IPermissionDataSeeder _permissionDataSeeder;
    private readonly ICurrentTenant _currentTenant;
    private readonly IConfiguration _configuration;
    private readonly IRepository<Tenant> _tenantRepository;
    private readonly ITenantManager _tenantManager;
    private readonly IPermissionGrantRepository _permissionManager;
    private readonly IGuidGenerator _guidGenerator;

    public AdministrationServiceDatabaseMigrationChecker(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDistributedEventBus distributedEventBus,
        IAbpDistributedLock abpDistributedLock,
        IPermissionDefinitionManager permissionDefinitionManager,
        IPermissionDataSeeder permissionDataSeeder,
        IConfiguration configuration,
        IRepository<Tenant> tenantRepository,
        ITenantManager tenantManager,
        IPermissionGrantRepository permissionManager,
        IGuidGenerator guidGenerator)
        : base(
            unitOfWorkManager,
            serviceProvider,
            currentTenant,
            distributedEventBus,
            abpDistributedLock,
            AdministrationServiceDbProperties.ConnectionStringName)
    {
        _permissionDefinitionManager = permissionDefinitionManager;
        _permissionDataSeeder = permissionDataSeeder;
        _currentTenant = currentTenant;
        _configuration = configuration;
        _tenantRepository = tenantRepository;
        _tenantManager = tenantManager;
        _permissionManager = permissionManager;
        _guidGenerator = guidGenerator;
    }

    public override async Task CheckAndApplyDatabaseMigrationsAsync(bool dataSeed = true)
    {
        await base.CheckAndApplyDatabaseMigrationsAsync(dataSeed);

        if (dataSeed)
        {
            //await using (var handle = await _distributedLock.TryAcquireAsync("AdministrationServiceDataSeedContributor"))
            //{
            //    if (handle is null)
            //    {
            //        return;
            //    }
                await SeedDataAsync();
            //}
        }
    }

    private async Task SeedDataAsync()
    {
        using var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true);

        //var tenants = (await _tenantRepository.ToListAsync()).Select(s => new TenantConfiguration { Id = s.Id, Name = s.Name }).ToList();

        //if (tenants.Count == 0)
        //{
        //    tenants.AddRange(_configuration.GetSection("Tenants").Get<List<TenantConfiguration>>());
        //}

        //await CreatePermissions(MultiTenancySides.Host);

        //foreach (var tenant in tenants)
        //{
        //    using (_currentTenant.Change(tenant.Id, tenant.Name))
        //    {
        //        await CreatePermissions(MultiTenancySides.Tenant);
        //    }
        //}

        await uow.CompleteAsync();
    }

    //public async Task CreateTenantAsync()
    //{
    //    var tenants = await _tenantRepository.ToListAsync();

    //    if (!tenants.Any(a => a.Name == "arslan"))
    //    {
    //        var tenant = await _tenantManager.CreateAsync("arslan");

    //        await _tenantRepository.InsertAsync(tenant, true);
    //    }
    //}

    //public async Task CreatePermissions(MultiTenancySides multiTenancySide)
    //{
    //    var permissionNames = (await _permissionDefinitionManager
    //        .GetPermissionsAsync())
    //        .Where(p => p.MultiTenancySide.HasFlag(multiTenancySide))
    //        .Where(p => !p.Providers.Any() ||
    //                    p.Providers.Contains(RolePermissionValueProvider.ProviderName))
    //        .Select(p => p.Name)
    //        .ToArray();

    //    var perList = await _permissionManager.GetListAsync();

    //    var insertList = new List<PermissionGrant>();

    //    foreach (var item in permissionNames)
    //    {
    //        if (!perList.Any(a => a.Name == item && a.ProviderName == RolePermissionValueProvider.ProviderName && a.ProviderKey == "admin"))
    //        {
    //            insertList.Add(new PermissionGrant(
    //                                id: _guidGenerator.Create(),
    //                                name: item,
    //                                providerName: RolePermissionValueProvider.ProviderName,
    //                                providerKey: "admin",
    //                                tenantId: _currentTenant.Id));
    //        }
    //    }
    //}
}