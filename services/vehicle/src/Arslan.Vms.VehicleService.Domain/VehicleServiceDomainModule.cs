using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Arslan.Vms.VehicleService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(VehicleServiceDomainSharedModule)
)]
public class VehicleServiceDomainModule : AbpModule
{

}
