using Arslan.Vms.IdentityService;
using Arslan.Vms.InventoryService;
using Arslan.Vms.OrderService;
using Arslan.Vms.PaymentService;
using Arslan.Vms.PlannerService;
using Arslan.Vms.ProductService;
using Arslan.Vms.VehicleService;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Arslan.Vms.AdministrationService;
[DependsOn(
    typeof(AdministrationServiceDomainSharedModule),
	typeof(AbpFeatureManagementApplicationContractsModule),
	typeof(AbpPermissionManagementApplicationContractsModule),
	typeof(AbpSettingManagementApplicationContractsModule),
	typeof(AbpTenantManagementApplicationContractsModule),
	typeof(IdentityServiceApplicationContractsModule),
	typeof(InventoryServiceApplicationContractsModule),
	typeof(OrderServiceApplicationContractsModule),
	typeof(PaymentServiceApplicationContractsModule),
	typeof(PlannerServiceApplicationContractsModule),
	typeof(ProductServiceApplicationContractsModule),
	typeof(VehicleServiceApplicationContractsModule)
	)]
public class AdministrationServiceApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
    }
}