using Arslan.Vms.PaymentService.EntityFrameworkCore;
using Arslan.Vms.Shared.Hosting.AspNetCore;
using Arslan.Vms.Shared.Hosting.Microservices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.VirtualFileSystem;

namespace Arslan.Vms.PaymentService;

[DependsOn(
    typeof(PaymentServiceApplicationModule),
    typeof(PaymentServiceEntityFrameworkCoreModule),
    typeof(PaymentServiceHttpApiModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(ArslanVmsSharedHostingMicroservicesModule)
    )]
public class PaymentServiceHttpApiHostModule : AbpModule
{

	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		var hostingEnvironment = context.Services.GetHostingEnvironment();
		var configuration = context.Services.GetConfiguration();

		JwtBearerConfigurationHelper.Configure(context, "PaymentService");

		SwaggerConfigurationHelper.ConfigureWithAuth(
			context: context,
			scopes: new
				Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
					{"PaymentService", "Payment Service API"}
			},
			"Payment Service"
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
}
