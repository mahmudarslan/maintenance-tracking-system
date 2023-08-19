using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Arslan.Vms.InventoryService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(InventoryServiceDomainSharedModule)
)]
public class InventoryServiceDomainModule : AbpModule
{

}
