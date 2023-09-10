using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Arslan.Vms.OrderService;

[DependsOn(
    typeof(OrderServiceApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class OrderServiceHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(OrderServiceApplicationContractsModule).Assembly,
            OrderServiceRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<OrderServiceHttpApiClientModule>();
        });

    }
}
