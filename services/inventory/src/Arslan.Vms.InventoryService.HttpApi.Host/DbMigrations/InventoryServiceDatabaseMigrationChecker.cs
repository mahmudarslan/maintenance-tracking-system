using Arslan.Vms.InventoryService.EntityFrameworkCore;
using Arslan.Vms.Shared.Hosting.Microservices.DbMigrations.EfCore;
using System;
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
}