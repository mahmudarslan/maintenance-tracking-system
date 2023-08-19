using Arslan.Vms.VehicleService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Arslan.Vms.VehicleService;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(VehicleServiceEntityFrameworkCoreTestModule)
    )]
public class VehicleServiceDomainTestModule : AbpModule
{

}
