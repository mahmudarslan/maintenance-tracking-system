using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using Arslan.Vms.IdentityService.EntityFrameworkCore;
using Arslan.Vms.InventoryService.EntityFrameworkCore;
using Arslan.Vms.OrderService.EntityFrameworkCore;
using Arslan.Vms.PaymentService.EntityFrameworkCore;
using Arslan.Vms.PlannerService.EntityFrameworkCore;
using Arslan.Vms.ProductService.EntityFrameworkCore;
using Arslan.Vms.VehicleService.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace Arslan.Vms.DbIntegration;

[DependsOn(
typeof(AbpAutofacModule),
typeof(AdministrationServiceEntityFrameworkCoreModule),
typeof(IdentityServiceEntityFrameworkCoreModule),
typeof(InventoryServiceEntityFrameworkCoreModule),
typeof(OrderServiceEntityFrameworkCoreModule),
typeof(PaymentServiceEntityFrameworkCoreModule),
typeof(PlannerServiceEntityFrameworkCoreModule),
typeof(ProductServiceEntityFrameworkCoreModule),
typeof(VehicleServiceEntityFrameworkCoreModule),
typeof(AbpAutoMapperModule),
typeof(AbpEventBusRabbitMqModule),
typeof(AbpBlobStoringMinioModule),
typeof(AbpBlobStoringFileSystemModule)
)]
public class VmsDbIntegrationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);

        context.Services.AddAutoMapperObjectMapper<VmsDbIntegrationModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<VmsDbIntegrationModule>(validate: false);
        });

        context.Services.Replace(ServiceDescriptor.Singleton<IAuditPropertySetter, CustomAuditPropertySetter>());

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Clear();
            options.AutoEventSelectors.Clear();
        });

        Configure<AbpDistributedEventBusOptions>(options =>
        {
            options.Handlers.Clear();
            options.Inboxes.Clear();
        });

        ConfigureMinio(configuration);

        context.Services.AddAbpDbContext<PermissionManagementDbContext>(options =>
        {
            options.AddDefaultRepositories(includeAllEntities: true);
        });
    }

    private void ConfigureMinio(IConfiguration configuration)
    {
        if (configuration["Minio:IsEnabled"] == "true")
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseMinio(minio =>
                    {
                        minio.EndPoint = configuration["Minio:EndPoint"];
                        minio.AccessKey = configuration["Minio:AccessKey"];
                        minio.SecretKey = configuration["Minio:SecretKey"];
                        minio.BucketName = configuration["Minio:BucketName"];
                    });
                });
            });
        }
        else
        {
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseFileSystem(fileSystem =>
                    {
                        fileSystem.BasePath = configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + "upload-files";
                    });
                });
            });
        }
    }
}