using Localization.Resources.AbpUi;
using Arslan.Vms.InventoryService.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Arslan.Vms.InventoryService;

[DependsOn(
    typeof(InventoryServiceApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class InventoryServiceHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(InventoryServiceHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<InventoryServiceResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
