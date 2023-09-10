using Localization.Resources.AbpUi;
using Arslan.Vms.OrderService.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Arslan.Vms.OrderService;

[DependsOn(
    typeof(OrderServiceApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class OrderServiceHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(OrderServiceHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<OrderServiceResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
