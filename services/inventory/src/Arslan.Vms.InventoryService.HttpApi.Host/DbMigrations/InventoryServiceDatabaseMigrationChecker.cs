using Arslan.Vms.InventoryService.EntityFrameworkCore;
using Arslan.Vms.Shared.Hosting.Microservices.DbMigrations.EfCore;
using System;
using System.Threading.Tasks;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Arslan.Vms.InventoryService.DbMigrations;

public class InventoryServiceDatabaseMigrationChecker
    : PendingEfCoreMigrationsChecker<InventoryServiceDbContext>
{
    public InventoryServiceDatabaseMigrationChecker(
        IUnitOfWorkManager unitOfWorkManager,
        IServiceProvider serviceProvider,
        ICurrentTenant currentTenant,
        IDistributedEventBus distributedEventBus,
        IAbpDistributedLock abpDistributedLock)
        : base(
            unitOfWorkManager,
            serviceProvider,
            currentTenant,
            distributedEventBus,
            abpDistributedLock,
            InventoryServiceDbProperties.ConnectionStringName)
    {
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