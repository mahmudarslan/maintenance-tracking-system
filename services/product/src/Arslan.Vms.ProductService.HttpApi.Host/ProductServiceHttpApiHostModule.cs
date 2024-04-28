using Arslan.Vms.ProductService.DbMigrations;
using Arslan.Vms.ProductService.EntityFrameworkCore;
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
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Arslan.Vms.ProductService;

[DependsOn(
    typeof(ProductServiceApplicationModule),
    typeof(ProductServiceEntityFrameworkCoreModule),
    typeof(ProductServiceHttpApiModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(ArslanVmsSharedHostingMicroservicesModule)
    )]
public class ProductServiceHttpApiHostModule : AbpModule
{

	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		var hostingEnvironment = context.Services.GetHostingEnvironment();
		var configuration = context.Services.GetConfiguration();

		JwtBearerConfigurationHelper.Configure(context, "ProductService");

		SwaggerConfigurationHelper.ConfigureWithAuth(
			context: context,
            configuration: configuration,
            scopes: new
				Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
					{"ProductService", "Product Service API"}
			},
			"Product Service"
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
        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }
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

    //Hangfire için önceden database oluşturuluyor
    public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider
        .GetRequiredService<ProductServiceDatabaseMigrationChecker>()
        .CheckAndApplyDatabaseMigrationsAsync(dataSeed: false);
    }

    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.ServiceProvider
            .GetRequiredService<ProductServiceDatabaseMigrationChecker>()
            .CheckAndApplyDatabaseMigrationsAsync();
    }
}
