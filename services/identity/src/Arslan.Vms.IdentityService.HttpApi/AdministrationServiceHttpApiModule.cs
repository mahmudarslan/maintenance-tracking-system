using Volo.Abp.Modularity;

namespace Arslan.Vms.IdentityService;
[DependsOn(
    typeof(IdentityServiceApplicationContractsModule))]
public class IdentityServiceHttpApiModule : AbpModule
{
}
