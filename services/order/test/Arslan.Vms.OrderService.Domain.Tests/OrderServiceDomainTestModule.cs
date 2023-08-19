using Arslan.Vms.OrderService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Arslan.Vms.OrderService;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(OrderServiceEntityFrameworkCoreTestModule)
    )]
public class OrderServiceDomainTestModule : AbpModule
{

}
