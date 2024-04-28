using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using Arslan.Vms.Shared.Hosting.AspNetCore;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Security.Claims;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs.RabbitMQ;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.IdentityModel;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

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
    public override Task PreConfigureServicesAsync(ServiceConfigurationContext context)
    {
        PreConfigure<AbpHttpClientBuilderOptions>(options =>
        {
            options.ProxyClientActions.Add((remoteServiceName, clientBuilder, client) =>
            {
                client.Timeout = TimeSpan.FromMinutes(10);
            });

            //PreConfigure<WebRemoteDynamicClaimsPrincipalContributorOptions>(options =>
            //{
            //	options.IsEnabled = true;
            //});
        });

        return base.PreConfigureServicesAsync(context);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        IdentityModelEventSource.ShowPII = true;
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        Configure<AbpTenantResolveOptions>(options =>
        {
            options.TenantResolvers.Add(new CustomDomainTenantResolveContributor("{0}.arslan.io"));
        });

        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
        });

        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "Vms:";
        });

        //ConnectionMultiplexer.SetFeatureFlag("preventthreadtheft", true);
        var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);

        context.Services
            .AddDataProtection()
            .PersistKeysToStackExchangeRedis(redis, "Vms-Protection-Keys");

        context.Services.AddSingleton<IDistributedLockProvider>(sp =>
        {
            var connection = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
            return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
        });

        IdentityModelEventSource.ShowPII = true;

        if (!hostingEnvironment.IsProduction() && !hostingEnvironment.IsStaging())
        {
            Configure<AbpExceptionHandlingOptions>(options =>
            {
                options.SendExceptionsDetailsToClients = configuration["AbpExceptionHandlingOptions:SendExceptionsDetailsToClients"] == "true" ? true : false;// Default: false.;
                options.SendStackTraceToClients = configuration["AbpExceptionHandlingOptions:SendStackTraceToClients"] == "false" ? false : true;// Default: true.;
            });
        }

        Configure<AbpClaimsMapOptions>(options =>
        {
            options.Maps.Add("clientId", () => AbpClaimTypes.ClientId);
            options.Maps.Add("impersonator.id", () => AbpClaimTypes.ImpersonatorUserId);
            options.Maps.Add("impersonator.username", () => AbpClaimTypes.ImpersonatorUserName);
        });

        Configure<AbpAuditingOptions>(options =>
        {
            options.IsEnabled = configuration["AbpAuditingOptions:IsEnabled"] == "false" ? false : true;// Default: true.
            options.IsEnabledForGetRequests = configuration["AbpAuditingOptions:IsEnabledForGetRequests"] == "true" ? true : false;// Default: false.
            options.IsEnabledForIntegrationServices = configuration["AbpAuditingOptions:IsEnabledForIntegrationServices"] == "true" ? true : false;// Default: false.
            options.HideErrors = configuration["AbpAuditingOptions:HideErrors"] == "false" ? false : true;// Default: true.
            options.IsEnabledForAnonymousUsers = configuration["AbpAuditingOptions:IsEnabledForAnonymousUsers"] == "false" ? false : true;// Default: true.
            options.AlwaysLogOnException = configuration["AbpAuditingOptions:AlwaysLogOnException"] == "false" ? false : true;// Default: true.
            options.DisableLogActionInfo = configuration["AbpAuditingOptions:DisableLogActionInfo"] == "true" ? true : false;// Default: false.
            options.ApplicationName = !string.IsNullOrEmpty(configuration["AbpAuditingOptions:ApplicationName"]) ? configuration["AbpAuditingOptions:ApplicationName"] : options.ApplicationName;// Default: null.
            options.Contributors.Add(new CustomAuditLogContributor());
        });

        Configure<AbpEventBusBoxesOptions>(options =>
        {
            options.CleanOldEventTimeIntervalSpan = TimeSpan.FromHours(6);
            options.WaitTimeToDeleteProcessedInboxEvents = TimeSpan.FromHours(2);
            options.DistributedLockWaitDuration = TimeSpan.FromSeconds(15);
        });

        Configure<HubOptions>(options =>
        {
            options.EnableDetailedErrors = true;
        });

        PostConfigure<MvcOptions>(mvcOptions =>
        {
            mvcOptions.Filters.ReplaceOne(x => (x as ServiceFilterAttribute)?.ServiceType == typeof(AbpExceptionFilter), new ServiceFilterAttribute(typeof(CustomAbpExceptionFilter)));
        });

        //Configure<AbpAntiForgeryOptions>(options =>
        //{
        //	options.AutoValidate = false;
        //});

        context.Services.Replace(ServiceDescriptor.Transient<IdentityModelAuthenticationService, CustomIdentityModelAuthenticationService>());

        context.Services.Replace(ServiceDescriptor.Transient<ICurrentUser, CustomCurrentUser>());

        context.Services.Replace(ServiceDescriptor.Transient<IAuditLogInfoToAuditLogConverter, CustomAuditLogInfoToAuditLogConverter>());

        context.Services.Replace(ServiceDescriptor.Singleton<HttpContextCurrentPrincipalAccessor, CustomHttpContextCurrentPrincipalAccessor>());

        context.Services.AddHttpClient();
    }
}