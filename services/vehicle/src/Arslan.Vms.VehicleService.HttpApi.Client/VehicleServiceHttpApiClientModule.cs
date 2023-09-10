using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Arslan.Vms.VehicleService;

[DependsOn(
    typeof(VehicleServiceApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class VehicleServiceHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(VehicleServiceApplicationContractsModule).Assembly,
            VehicleServiceRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<VehicleServiceHttpApiClientModule>();
        });

    }
}
