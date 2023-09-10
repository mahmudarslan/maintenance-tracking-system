using Arslan.Vms.InventoryService;
using Arslan.Vms.InventoryService.Localization;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Arslan.Vms.Inventory
{
    [DependsOn(
        typeof(InventoryServiceApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class InventoryHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(InventoryHttpApiModule).Assembly);
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
}