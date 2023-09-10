using Arslan.Vms.OrderService.PurchaseOrders;
using Arslan.Vms.OrderService.SalesOrders;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Arslan.Vms.OrderService.EntityFrameworkCore;

[DependsOn(
    typeof(OrderServiceDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class OrderServiceEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<OrderServiceDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */

            options.AddDefaultRepositories<IOrderServiceDbContext>();

            options.AddRepository<SalesOrder, SalesOrderRepository>();
            options.AddRepository<PurchaseOrder, PurchaseOrderRepository>();
        });

		Configure<AbpDbContextOptions>(options =>
		{
			options.Configure<OrderServiceDbContext>(c =>
			{
				c.UseSqlServer();
			});
		});
	}
}
