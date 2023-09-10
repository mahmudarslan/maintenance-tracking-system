using Volo.Abp.Modularity;

namespace Arslan.Vms.VehicleService;

[DependsOn(
    typeof(VehicleServiceApplicationModule),
    typeof(VehicleServiceDomainTestModule)
    )]
public class VehicleServiceApplicationTestModule : AbpModule
{

}
