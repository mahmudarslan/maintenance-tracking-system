using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Arslan.Vms.InventoryService;

[DependsOn(
    typeof(InventoryServiceDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class InventoryServiceApplicationContractsModule : AbpModule
{

}