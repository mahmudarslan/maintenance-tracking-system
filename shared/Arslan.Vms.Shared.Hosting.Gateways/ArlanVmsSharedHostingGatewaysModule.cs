using Arslan.Vms.Shared.Hosting.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Arslan.Vms.Shared.Hosting.Gateways
{
    [DependsOn(
        typeof(ArslanVmsSharedHostingAspNetCoreModule)
    )]
    public class ArlanVmsSharedHostingGatewaysModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            
            context.Services.AddReverseProxy()
                .LoadFromConfig(configuration.GetSection("ReverseProxy"));
        }
    }
}