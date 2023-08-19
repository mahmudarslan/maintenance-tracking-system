using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Arslan.Vms.PaymentService;

[DependsOn(
    typeof(PaymentServiceApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class PaymentServiceHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(PaymentServiceApplicationContractsModule).Assembly,
            PaymentServiceRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PaymentServiceHttpApiClientModule>();
        });

    }
}
