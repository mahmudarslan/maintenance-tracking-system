using Localization.Resources.AbpUi;
using Arslan.Vms.VehicleService.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Arslan.Vms.VehicleService;

[DependsOn(
    typeof(VehicleServiceApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class VehicleServiceHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(VehicleServiceHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<VehicleServiceResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
