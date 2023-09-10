using Volo.Abp.Modularity;

namespace Arslan.Vms.InventoryService;

[DependsOn(
    typeof(InventoryServiceApplicationModule),
    typeof(InventoryServiceDomainTestModule)
    )]
public class InventoryServiceApplicationTestModule : AbpModule
{

}
