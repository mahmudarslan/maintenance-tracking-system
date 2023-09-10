using Volo.Abp.Modularity;

namespace Arslan.Vms.IdentityService;
[DependsOn(
    typeof(IdentityServiceDomainSharedModule)
    )]
public class IdentityServiceApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
    }
}