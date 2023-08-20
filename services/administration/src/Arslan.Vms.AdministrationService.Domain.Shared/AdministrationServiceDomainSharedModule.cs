using Arslan.Vms.AdministrationService.Localization;
using Volo.Abp.AuditLogging;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Arslan.Vms.AdministrationService;
[DependsOn(
	typeof(AbpAuditLoggingDomainSharedModule),
	typeof(AbpFeatureManagementDomainSharedModule),
	typeof(AbpPermissionManagementDomainSharedModule),
	typeof(AbpSettingManagementDomainSharedModule),
	typeof(AbpTenantManagementDomainSharedModule)

	)]
public class AdministrationServiceDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AdministrationServiceDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AdministrationServiceResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/AdministrationService");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("AdministrationService", typeof(AdministrationServiceResource));
        });
    }
}
