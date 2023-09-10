using Arslan.Vms.AdministrationService.DbMigrations;
using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using Arslan.Vms.Shared.Hosting.AspNetCore;
using Arslan.Vms.Shared.Hosting.Microservices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Emailing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Arslan.Vms.AdministrationService;

[DependsOn(
    typeof(AdministrationServiceHttpApiModule),
    typeof(AdministrationServiceApplicationModule),
    typeof(AdministrationServiceEntityFrameworkCoreModule),
    typeof(ArslanVmsSharedHostingMicroservicesModule)
    )]
public class AdministrationServiceHttpApiHostModule : AbpModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        JwtBearerConfigurationHelper.Configure(context, "");

        ConfigureLocalization();
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        SwaggerConfigurationHelper.ConfigureWithAuth(
            context: context,
            configuration: configuration,
            scopes: new Dictionary<string, string> { { "Vms_AdministrationService", "Vms Administration Service API" } },
            apiTitle: "Administration Service");
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English", "./assets/media/flags/united-states.svg"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe", "./assets/media/flags/turkey.svg"));
        });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();
        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();

        app.UsePathBase(configuration["App:PathBase"]);

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            var settingManager = context.ServiceProvider.GetService<SettingManager>();
            //encrypts the password on set and decrypts on get
            settingManager.SetGlobalAsync(EmailSettingNames.Smtp.Password, configuration["Settings:Abp.Mailing.Smtp.Password"]);
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpClaimsMap();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();

        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            //options.SwaggerEndpoint(configuration["Swagger:Endpoint"], $"{env.EnvironmentName} - {version}");
            options.OAuthClientId(configuration["Swagger:SwaggerClientId"]);
            options.OAuthClientSecret(configuration["Swagger:SwaggerClientSecret"]);
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            // build a swagger endpoint for each discovered API version
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"{configuration["App:PathBase"]}/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }

    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider
            .GetRequiredService<AdministrationServiceDatabaseMigrationChecker>()
            .CheckAndApplyDatabaseMigrationsAsync();
    }
}
