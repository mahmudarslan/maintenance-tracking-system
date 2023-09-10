using Arslan.Vms.OrderService.EntityFrameworkCore;
using Arslan.Vms.Shared.Hosting.Microservices.DbMigrations.EfCore;
using System;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Arslan.Vms.OrderService.DbMigrations;

public class OrderServiceDatabaseMigrationChecker
    : PendingEfCoreMigrationsChecker<OrderServiceDbContext>
{
    public OrderServiceDatabaseMigrationChecker(
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
            OrderServiceDbProperties.ConnectionStringName)
    {
    }
}