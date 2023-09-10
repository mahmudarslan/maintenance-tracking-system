using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Arslan.Vms.PlannerService;

[DependsOn(
    typeof(PlannerServiceApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class PlannerServiceHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(PlannerServiceApplicationContractsModule).Assembly,
            PlannerServiceRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PlannerServiceHttpApiClientModule>();
        });

    }
}
