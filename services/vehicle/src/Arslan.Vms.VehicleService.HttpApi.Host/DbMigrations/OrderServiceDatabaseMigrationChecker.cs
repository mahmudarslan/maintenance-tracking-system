using Arslan.Vms.Shared.Hosting.Microservices.DbMigrations.EfCore;
using Arslan.Vms.VehicleService.EntityFrameworkCore;
using System;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Arslan.Vms.VehicleService.DbMigrations;

public class VehicleServiceDatabaseMigrationChecker
    : PendingEfCoreMigrationsChecker<VehicleServiceDbContext>
{
    public VehicleServiceDatabaseMigrationChecker(
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
            VehicleServiceDbProperties.ConnectionStringName)
    {
    }
}