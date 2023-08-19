using Arslan.Vms.InventoryService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Arslan.Vms.InventoryService;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(InventoryServiceEntityFrameworkCoreTestModule)
    )]
public class InventoryServiceDomainTestModule : AbpModule
{

}
