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
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

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
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        JwtBearerConfigurationHelper.Configure(context, "AdministrationService");

        SwaggerConfigurationHelper.ConfigureWithAuth(
            context: context,
            scopes: new
                Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
                    {"AdministrationService", "Administration Service API"}
            },
            apiTitle: "Administration Service API"
        );

        //if (hostingEnvironment.IsDevelopment())
        //{
        //    Configure<AbpVirtualFileSystemOptions>(options =>
        //    {
        //        options.FileSets.ReplaceEmbeddedByPhysical<AdministrationServiceDomainSharedModule>(
        //            Path.Combine(hostingEnvironment.ContentRootPath,
        //                string.Format("..{0}..{0}src{0}Arslan.Vms.AdministrationService.Domain.Shared", Path.DirectorySeparatorChar)));
        //        options.FileSets.ReplaceEmbeddedByPhysical<AdministrationServiceDomainModule>(
        //            Path.Combine(hostingEnvironment.ContentRootPath,
        //                string.Format("..{0}..{0}src{0}Arslan.Vms.AdministrationService.Domain", Path.DirectorySeparatorChar)));
        //        options.FileSets.ReplaceEmbeddedByPhysical<AdministrationServiceApplicationContractsModule>(
        //            Path.Combine(hostingEnvironment.ContentRootPath,
        //                string.Format("..{0}..{0}src{0}Arslan.Vms.AdministrationService.Application.Contracts",
        //                    Path.DirectorySeparatorChar)));
        //        options.FileSets.ReplaceEmbeddedByPhysical<AdministrationServiceApplicationModule>(
        //            Path.Combine(hostingEnvironment.ContentRootPath,
        //                string.Format("..{0}..{0}src{0}Arslan.Vms.AdministrationService.Application", Path.DirectorySeparatorChar)));
        //    });
        //}

        Configure<AbpLocalizationOptions>(options =>
        { 
            options.Languages.Add(new LanguageInfo("en", "en", "English")); 
            options.Languages.Add(new LanguageInfo("tr", "tr", "T�rk�e")); 
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
		var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();

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
		app.UseAbpSerilogEnrichers();
        app.UseAuditing();
        app.UseUnitOfWork();
        app.UseConfiguredEndpoints();
    }

    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider
            .GetRequiredService<AdministrationServiceDatabaseMigrationChecker>()
            .CheckAndApplyDatabaseMigrationsAsync();
    }
}
