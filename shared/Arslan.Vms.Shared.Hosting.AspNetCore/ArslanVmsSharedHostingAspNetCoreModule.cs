using Arslan.Vms.Shared.Hosting.AspNetCore.Swagger;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace Arslan.Vms.Shared.Hosting.AspNetCore;

[DependsOn(
    typeof(ArslanVmsSharedHostingModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule)
)]
public class ArslanVmsSharedHostingAspNetCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        var preActions = context.Services.GetPreConfigureActions<AbpAspNetCoreMvcOptions>();
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            preActions.Configure(options);
        });

        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ChangeControllerModelApiExplorerGroupName = false;
        });

        context.Services.AddScoped<MultiTenantSwaggerUiMiddleware>();
    }
}