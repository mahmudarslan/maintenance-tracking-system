using Arslan.Vms.IdentityService.DbMigrations;
using Arslan.Vms.IdentityService.EntityFrameworkCore;
using Arslan.Vms.Shared.Hosting.AspNetCore;
using Arslan.Vms.Shared.Hosting.Microservices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Arslan.Vms.IdentityService;

[DependsOn(
    typeof(IdentityServiceHttpApiModule),
    typeof(IdentityServiceApplicationModule),
    typeof(IdentityServiceEntityFrameworkCoreModule),
    typeof(ArslanVmsSharedHostingMicroservicesModule)
    )]
public class IdentityServiceHttpApiHostModule : AbpModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        JwtBearerConfigurationHelper.Configure(context, "IdentityService");

        SwaggerConfigurationHelper.ConfigureWithAuth(
            context: context,
            authority: configuration["AuthServer:Authority"],
            scopes: new
                Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
                    {"IdentityService", "Identity Service API"}
            },
            apiTitle: "Identity Service API"
		);

        //if (hostingEnvironment.IsDevelopment())
        //{
        //    Configure<AbpVirtualFileSystemOptions>(options =>
        //    {
        //        options.FileSets.ReplaceEmbeddedByPhysical<IdentityServiceDomainSharedModule>(
        //            Path.Combine(hostingEnvironment.ContentRootPath,
        //                string.Format("..{0}..{0}src{0}Arslan.Vms.IdentityService.Domain.Shared", Path.DirectorySeparatorChar)));
        //        options.FileSets.ReplaceEmbeddedByPhysical<IdentityServiceDomainModule>(
        //            Path.Combine(hostingEnvironment.ContentRootPath,
        //                string.Format("..{0}..{0}src{0}Arslan.Vms.IdentityService.Domain", Path.DirectorySeparatorChar)));
        //        options.FileSets.ReplaceEmbeddedByPhysical<IdentityServiceApplicationContractsModule>(
        //            Path.Combine(hostingEnvironment.ContentRootPath,
        //                string.Format("..{0}..{0}src{0}Arslan.Vms.IdentityService.Application.Contracts",
        //                    Path.DirectorySeparatorChar)));
        //        options.FileSets.ReplaceEmbeddedByPhysical<IdentityServiceApplicationModule>(
        //            Path.Combine(hostingEnvironment.ContentRootPath,
        //                string.Format("..{0}..{0}src{0}Arslan.Vms.IdentityService.Application", Path.DirectorySeparatorChar)));
        //    });
        //}

        Configure<AbpLocalizationOptions>(options =>
        { 
            options.Languages.Add(new LanguageInfo("en", "en", "English")); 
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe")); 
        });

        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.Trim().RemovePostFix("/"))
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

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            IdentityModelEventSource.ShowPII = true;
        }

        app.UseCorrelationId();
        app.UseCors();
        app.UseAbpRequestLocalization();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpClaimsMap();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Administration Service API");
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            // options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
        });
        app.UseAbpSerilogEnrichers();
        app.UseAuditing();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints();
    }

    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider
            .GetRequiredService<IdentityServiceDatabaseMigrationChecker>()
            .CheckAndApplyDatabaseMigrationsAsync();
    }
}
