using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.Shared.Hosting.AspNetCore.Swagger;

public class MultiTenantSwaggerUiMiddleware : IMiddleware
{
    private readonly IWebHostEnvironment _hostingEnv;
    private readonly ILoggerFactory _loggerFactory;
    private readonly SwaggerUIOptions _options;
    private readonly IConfiguration _configuration;
    private readonly IOptions<SwaggerGenOptions> _options1;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public MultiTenantSwaggerUiMiddleware(
        IWebHostEnvironment hostingEnv,
        ILoggerFactory loggerFactory,
        IOptions<SwaggerUIOptions> options,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        IOptions<SwaggerGenOptions> options1)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _options1 = options1;
        _hostingEnv = hostingEnv;
        _loggerFactory = loggerFactory;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requset = _httpContextAccessor.HttpContext.Request;

        if (!requset.RouteValues.Values.Contains("Home"))
        {
            await next(context);
            return;
        }

        var host = requset.Headers["X-Forwarded-Host"].ToString();

        if (string.IsNullOrEmpty(host))
        {
            host = requset.Host.ToString();
        }

        var tenant = host.Split('.').First();

        if (tenant == null || tenant.Contains("localhost"))
        {
            tenant = _configuration.GetSection("Tenants").Get<List<TenantConfiguration>>().FirstOrDefault().Name;
        }

        _options1.Value.SwaggerGeneratorOptions.SecuritySchemes["oauth2"].Flows.AuthorizationCode.AuthorizationUrl = new Uri($"{_configuration["AuthServer:Authority"]}/{tenant}/protocol/openid-connect/auth");
        _options1.Value.SwaggerGeneratorOptions.SecuritySchemes["oauth2"].Flows.AuthorizationCode.TokenUrl = new Uri($"{_configuration["AuthServer:Authority"]}/{tenant}/protocol/openid-connect/token");

        var m = new SwaggerUIMiddleware(next, _hostingEnv, _loggerFactory, _options);
        await m.Invoke(context);
    }
}