using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using Arslan.Vms.Shared.Hosting.AspNetCore;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using StackExchange.Redis;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Security.Claims;
using Volo.Abp.Auditing;
using Volo.Abp.BackgroundJobs.RabbitMQ;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Emailing;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.IdentityModel;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Arslan.Vms.Shared.Hosting.Microservices;

[DependsOn(
    typeof(ArslanVmsSharedHostingAspNetCoreModule),
	typeof(AbpBackgroundJobsRabbitMqModule),
	typeof(AbpAspNetCoreMultiTenancyModule),
	typeof(AbpEventBusRabbitMqModule),
	typeof(AbpCachingStackExchangeRedisModule),
	typeof(AdministrationServiceEntityFrameworkCoreModule),
	typeof(AbpDistributedLockingModule),
	typeof(AbpHttpClientIdentityModelWebModule),
	typeof(AbpAspNetCoreAuthenticationJwtBearerModule)
)]
public class ArslanVmsSharedHostingMicroservicesModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        IdentityModelEventSource.ShowPII = true;
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
        });

        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "Vms:";
        });

        var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
        context.Services
            .AddDataProtection()
            .PersistKeysToStackExchangeRedis(redis, "Vms-Protection-Keys");

        context.Services.AddSingleton<IDistributedLockProvider>(sp =>
        {
            var connection = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
        });

        if (!hostingEnvironment.IsProduction())
        {
            IdentityModelEventSource.ShowPII = true;
        }

        Configure<AbpClaimsMapOptions>(options =>
        {
            options.Maps.Add("clientId", () => AbpClaimTypes.ClientId);
            options.Maps.Add("impersonator.id", () => AbpClaimTypes.ImpersonatorUserId);
            options.Maps.Add("impersonator.username", () => AbpClaimTypes.ImpersonatorUserName);
        });

        Configure<AbpAuditingOptions>(options =>
        {
            options.IsEnabledForGetRequests = true;
            options.IsEnabledForIntegrationServices = true;
        });

        //Replacing the IConnectionStringResolver service
        context.Services.Replace(
            ServiceDescriptor.Transient<IdentityModelAuthenticationService, CustomIdentityModelAuthenticationService>());

        if (hostingEnvironment.IsDevelopment())
        {
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
        }
        else
        {
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, CustomSmtpEmailSender>());
        }
    }
}