using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Arslan.Vms.InventoryService;

[DependsOn(
    typeof(InventoryServiceApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class InventoryServiceHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(InventoryServiceApplicationContractsModule).Assembly,
            InventoryServiceRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<InventoryServiceHttpApiClientModule>();
        });

    }
}
