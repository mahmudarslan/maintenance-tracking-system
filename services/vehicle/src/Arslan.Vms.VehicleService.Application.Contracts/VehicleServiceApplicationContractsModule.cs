using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Arslan.Vms.VehicleService;

[DependsOn(
    typeof(VehicleServiceDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class VehicleServiceApplicationContractsModule : AbpModule
{

}
