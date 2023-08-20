using Arslan.Vms.IdentityService.DbMigrations;
using Arslan.Vms.IdentityService.EntityFrameworkCore;
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
			scopes: new
				Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
					{"IdentityService", "Identity Service API"}
			},
			"Identity Service"
		);

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
			.GetRequiredService<IdentityServiceDatabaseMigrationChecker>()
			.CheckAndApplyDatabaseMigrationsAsync();
	}
}
