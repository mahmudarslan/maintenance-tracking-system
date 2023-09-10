using Localization.Resources.AbpUi;
using Arslan.Vms.PlannerService.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Arslan.Vms.PlannerService;

[DependsOn(
    typeof(PlannerServiceApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class PlannerServiceHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(PlannerServiceHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<PlannerServiceResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
