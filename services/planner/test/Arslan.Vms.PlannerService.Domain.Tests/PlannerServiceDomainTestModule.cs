using Arslan.Vms.PlannerService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Arslan.Vms.PlannerService;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(PlannerServiceEntityFrameworkCoreTestModule)
    )]
public class PlannerServiceDomainTestModule : AbpModule
{

}
