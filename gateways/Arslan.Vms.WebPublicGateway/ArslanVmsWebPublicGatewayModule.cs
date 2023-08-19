using Arslan.Vms.Shared.Hosting.AspNetCore;
using Arslan.Vms.Shared.Hosting.Gateways;
using Microsoft.AspNetCore.Rewrite;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Arslan.Vms.WebPublicGateway;

[DependsOn(
    typeof(ArlanVmsSharedHostingGatewaysModule)
)]
public class ArslanVmsWebPublicGatewayModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        SwaggerConfigurationHelper.ConfigureWithAuth(
            context: context,
            authority: configuration["AuthServer:Authority"],
            scopes: new Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
            {
                { "AccountService", "Account Service API" },
                { "IdentityService", "Identity Service API" },
                { "AdministrationService", "Administration Service API" },
                { "CatalogService", "Catalog Service API" },
                { "BasketService", "Basket Service API" },
                { "PaymentService", "Payment Service API" },
                { "OrderingService", "Ordering Service API" },
                { "CmskitService", "Cmskit Service API" },
            },
            apiTitle: "WebPublic Gateway"
        );

        // context.Services.AddReverseProxy()
        //     .LoadFromConfig(configuration.GetSection("ReverseProxy"));
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
        app.UseSwaggerUIWithYarp(context);

        app.UseRewriter(new RewriteOptions()
            // Regex for "", "/" and "" (whitespace)
            .AddRedirect("^(|\\|\\s+)$", "/swagger"));

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("", ctx => ctx.Response.WriteAsync("YAG"));
            endpoints.MapReverseProxy();
        });
    }
}