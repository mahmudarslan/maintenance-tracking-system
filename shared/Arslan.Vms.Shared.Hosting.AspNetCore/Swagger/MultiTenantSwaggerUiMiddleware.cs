using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Arslan.Vms.Shared.Hosting.AspNetCore.Swagger;

public class MultiTenantSwaggerUiMiddleware : IMiddleware
{
    private readonly IWebHostEnvironment hostingEnv;
    private readonly ILoggerFactory loggerFactory;
    private readonly SwaggerUIOptions options;
    private readonly IConfiguration configuration;

    public MultiTenantSwaggerUiMiddleware(
        IWebHostEnvironment hostingEnv,
        ILoggerFactory loggerFactory,
        IOptions<SwaggerUIOptions> options,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        IOptions<SwaggerGenOptions> options1)
    {
        var requset = httpContextAccessor.HttpContext.Request;
        var host = requset.Headers["X-Forwarded-Host"].ToString();

        if (string.IsNullOrEmpty(host))
        {
            host = requset.Host.ToString();
        }

        var tenant = host.Split('.').First();

        if (tenant == null || tenant.Contains("localhost"))
        {
            tenant = configuration["Tenants:DefaultTenant"];
        }

        options1.Value.SwaggerGeneratorOptions.SecuritySchemes["oauth2"].Flows.AuthorizationCode.AuthorizationUrl = new Uri($"{configuration["AuthServer:Authority"]}/{tenant}/protocol/openid-connect/auth");
        options1.Value.SwaggerGeneratorOptions.SecuritySchemes["oauth2"].Flows.AuthorizationCode.TokenUrl = new Uri($"{configuration["AuthServer:Authority"]}/{tenant}/protocol/openid-connect/token");

        this.hostingEnv = hostingEnv;
        this.loggerFactory = loggerFactory;
        this.options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var m = new SwaggerUIMiddleware(next, hostingEnv, loggerFactory, options);
        await m.Invoke(context);
    }
}