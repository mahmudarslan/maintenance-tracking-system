using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Arslan.Vms.Shared.Hosting.Microservices.Auths;

internal class ResolveAuthorityService : IDynamicJwtBearerAuthorityResolver
{
    private readonly IConfiguration configuration;

    public ResolveAuthorityService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public TimeSpan ExpirationOfConfiguration => TimeSpan.FromHours(1);

    public Task<string> ResolveAuthority(HttpContext httpContext)
    {
        string authorization = httpContext.Request.Headers[HeaderNames.Authorization];
        string authority = "";

        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authorization.Substring("Bearer ".Length).Trim();
            var jwtHandler = new JwtSecurityTokenHandler();
            var realm = jwtHandler.ReadJwtToken(token).Issuer.Split('/').Last();
            authority = $"{configuration["AuthServer:Authority"]}/{realm}";
        }
        else
        {
            var accessToken = httpContext.Request.Query["access_token"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var realm = jwtHandler.ReadJwtToken(accessToken).Issuer.Split('/').Last();
                authority = $"{configuration["AuthServer:Authority"]}/{realm}";
            }
            else if (string.IsNullOrEmpty(authority) && httpContext.Request.HasFormContentType)
            {
                var formData = httpContext.Request.ReadFormAsync().Result;
                accessToken = formData["access_token"];

                if (!string.IsNullOrEmpty(accessToken))
                {
                    var jwtHandler = new JwtSecurityTokenHandler();
                    var realm = jwtHandler.ReadJwtToken(accessToken).Issuer.Split('/').Last();
                    authority = $"{configuration["AuthServer:Authority"]}/{realm}";
                }
            }

            if (string.IsNullOrEmpty(authority))
            {
                var host = httpContext.Request.Headers["X-Forwarded-Host"].ToString();
                var realm = host.Split('.').First();

                if (!string.IsNullOrEmpty(realm))
                {
                    authority = $"{configuration["AuthServer:Authority"]}/{realm}";
                }
                else
                {
                    realm = httpContext.Request.Host.Host.Split('.').First();

                    authority = $"{configuration["AuthServer:Authority"]}/{realm}";
                }
            }
        }

        return Task.FromResult(authority);
    }
}