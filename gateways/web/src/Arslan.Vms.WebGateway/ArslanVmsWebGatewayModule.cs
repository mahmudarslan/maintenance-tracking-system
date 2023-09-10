using Arslan.Vms.Shared.Hosting.AspNetCore;
using Arslan.Vms.Shared.Hosting.Gateways;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Rewrite;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Modularity;

namespace Arslan.Vms.WebGateway;

[DependsOn(
    typeof(ArlanVmsSharedHostingGatewaysModule)
)]
public class ArslanVmsWebGatewayModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        SwaggerConfigurationHelper.ConfigureWithAuth(
            context: context,
            configuration: configuration,
            scopes: new
                Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
                    {"AdministrationService", "Administration Service API"},
                    {"AccountService", "Account Service API"},
                    {"IdentityService", "Identity Service API"},
                    {"PlannerService", "Planner Service API"},
                    {"InventoryService", "Inventory Service API"},
                    {"ProductService", "Product Service API"},
                    {"OrderingService", "Ordering Service API"},
                    {"VehicleService", "Vehicle Service API"},
                }
        );

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


        context.Services.Configure<AbpAuditingOptions>(a =>
                     {
                         a.IsEnabledForGetRequests = true; 
                     });

        //context.Services.AddReverseProxy().LoadFromConfig(configuration.GetSection("ReverseProxy"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseCorrelationId();
        app.UseAbpSerilogEnrichers();
        app.UseCors();
        app.UseSwaggerUIWithYarp(context);

        app.UseRewriter(new RewriteOptions()
            // Regex for "", "/" and "" (whitespace)
            .AddRedirect("^(|\\|\\s+)$", "/swagger"));

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapReverseProxy();
        });
    }
}