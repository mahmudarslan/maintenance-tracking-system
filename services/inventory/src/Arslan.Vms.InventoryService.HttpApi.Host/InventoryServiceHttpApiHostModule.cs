using Arslan.Vms.InventoryService.DbMigrations;
using Arslan.Vms.InventoryService.EntityFrameworkCore;
using Arslan.Vms.Shared.Hosting.AspNetCore;
using Arslan.Vms.Shared.Hosting.Microservices;
using Arslan.Vms.Shared.Hosting.Microservices.Auths;
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
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Arslan.Vms.InventoryService;

[DependsOn(
    typeof(InventoryServiceHttpApiModule),
    typeof(InventoryServiceApplicationModule),
    typeof(InventoryServiceEntityFrameworkCoreModule),
    typeof(ArslanVmsSharedHostingMicroservicesModule)
    )]
public class InventoryServiceHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

		JwtBearerConfigurationHelper.Configure(context, "InventoryService");

		SwaggerConfigurationHelper.ConfigureWithAuth(
			context: context,
            configuration: configuration,
            scopes: new
				Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
					{"InventoryService", "Inventory Service API"}
			},
			"Inventory Service"
		);

		//Configure<AbpDistributedCacheOptions>(options =>
		//{
		//    options.KeyPrefix = "InventoryService:";
		//});

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
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        //if (MultiTenancyConsts.IsEnabled)
        //{
        //    app.UseMultiTenancy();
        //}
        app.UseAbpRequestLocalization();
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

    //Hangfire i�in �nceden database olu?turuluyor
    public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider
        .GetRequiredService<InventoryServiceDatabaseMigrationChecker>()
        .CheckAndApplyDatabaseMigrationsAsync(dataSeed: false);
    }

    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider
            .GetRequiredService<InventoryServiceDatabaseMigrationChecker>()
            .CheckAndApplyDatabaseMigrationsAsync();
    }
}
