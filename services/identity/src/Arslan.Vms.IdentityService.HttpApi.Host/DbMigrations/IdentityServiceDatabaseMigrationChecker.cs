﻿using Arslan.Vms.IdentityService.EntityFrameworkCore;
using Arslan.Vms.Shared.Hosting.Microservices.DbMigrations.EfCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace Arslan.Vms.IdentityService.DbMigrations;

public class IdentityServiceDatabaseMigrationChecker
    : PendingEfCoreMigrationsChecker<IdentityServiceDbContext>
{
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;
    private readonly IPermissionDataSeeder _permissionDataSeeder;

    public IdentityServiceDatabaseMigrationChecker(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDistributedEventBus distributedEventBus,
        IAbpDistributedLock abpDistributedLock,
        IPermissionDefinitionManager permissionDefinitionManager,
        IPermissionDataSeeder permissionDataSeeder)
        : base(
            unitOfWorkManager,
            serviceProvider,
            currentTenant,
            distributedEventBus,
            abpDistributedLock,
            IdentityServiceDbProperties.ConnectionStringName)
    {
        _permissionDefinitionManager = permissionDefinitionManager;
        _permissionDataSeeder = permissionDataSeeder;
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
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
        {
            //var multiTenancySide = MultiTenancySides.Host;

            //var permissionNames = _permissionDefinitionManager
            //    .GetPermissions()
            //    .Where(p => p.MultiTenancySide.HasFlag(multiTenancySide))
            //    .Where(p => !p.Providers.Any() ||
            //                p.Providers.Contains(RolePermissionValueProvider.ProviderName))
            //    .Select(p => p.Name)
            //    .ToArray();

            //await _permissionDataSeeder.SeedAsync(
            //    RolePermissionValueProvider.ProviderName,
            //    "admin",
            //    permissionNames
            //);

            await uow.CompleteAsync();
        }
    }
}