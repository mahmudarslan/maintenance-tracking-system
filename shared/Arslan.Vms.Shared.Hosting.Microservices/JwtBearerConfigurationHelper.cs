using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;

namespace Arslan.Vms.Shared.Hosting.Microservices;

public class KeycloakConsts
{
    public static string KeycloakCacheKey = "KeycloakUserMapCacheItem";
}

[IgnoreMultiTenancy]
[CacheName("KeycloakUserMapCacheItem")]
public class KeycloakUserMapCacheItem
{
    public string Id { get; set; }
}

[IgnoreMultiTenancy]
[CacheName("TenantCacheItem")]
public class TenatCacheItem
{
    public List<string> Names { get; set; }
}

[UnitOfWork]
public static class JwtBearerConfigurationHelper
{
    public static void Configure(
        ServiceConfigurationContext context,
        string audience)
    {
        var configuration = context.Services.GetConfiguration();
        var url = configuration["AuthServer:Authority"];

        var tenants = new List<string>();

        using (var serviceProvider = context.Services.BuildServiceProvider())
        {
            var cachedTenants = serviceProvider.GetRequiredService<IDistributedCache<TenatCacheItem>>().Get("TenantCacheItem");
            if (cachedTenants != null && cachedTenants.Names.Count > 0)
            {
                tenants.AddRange(cachedTenants.Names);
            }
        }

        if (tenants.Count == 0)
        {
            var t = configuration["Tenants:Tenants"]
                               .Split(',')
                               .Select(s => s.Trim())
                               .Where(w => w.Length > 0)
                               .ToList();

            foreach (var item in t)
            {
                if (tenants.Any(a => a == item))
                {
                    continue;
                }

                tenants.Add(item);
            }
        }

        var auth = context.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

        auth.AddPolicyScheme(JwtBearerDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.ForwardDefaultSelector = c =>
            {
                string authorization = c.Request.Headers[HeaderNames.Authorization];
                if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                {
                    var token = authorization.Substring("Bearer ".Length).Trim();
                    var jwtHandler = new JwtSecurityTokenHandler();

                    foreach (var tenant in tenants)
                    {
                        if ((jwtHandler.CanReadToken(token) && jwtHandler.ReadJwtToken(token).Issuer.Split('/').Last() == tenant))
                        {
                            return tenant;
                        }
                    }
                }

                return configuration["Tenants:DefaultTenant"];
            };
        });

        foreach (var tenant in tenants)
        {
            auth.AddJwtBearer(tenant, x =>
            {
                x.Authority = url.TrimEnd('/') + "/" + tenant;
                x.Audience = configuration["AuthServer:Audience"];
                x.IncludeErrorDetails = true;
                x.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                x.RefreshOnIssuerKeyNotFound = false;
                x.TokenValidationParameters = GetTokenValidationParameters(url, new List<string> { "master-realm", "account" });
                x.BackchannelHttpHandler = GetHttpClientHandler();
                x.Events = GetJwtBearerEvent(configuration, context);
                x.Validate();
            });
        }

        context.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Api", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
        });
    }

    static HttpClientHandler GetHttpClientHandler()
    {
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
    }

    static TokenValidationParameters GetTokenValidationParameters(string validIssuer, List<string> validAudiences)
    {
        return new TokenValidationParameters
        {
            ValidIssuer = validIssuer,
            ValidAudiences = validAudiences,
            RequireSignedTokens = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
        };
    }

    static JwtBearerEvents GetJwtBearerEvent(IConfiguration configuration, ServiceConfigurationContext context)
    {
        return new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var queryPath = configuration["AuthServer:ReadTokenFromQueryPath"];

                if (!string.IsNullOrEmpty(queryPath))
                {
                    var queryPaths = queryPath.Split(',');
                    var pathList = new List<string>();

                    foreach (var item in queryPaths)
                    {
                        pathList.Add(item);
                    }

                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path.ToString();
                    if (!string.IsNullOrEmpty(accessToken) && pathList.Contains(path))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }

                    if (string.IsNullOrEmpty(context.Token) && pathList.Contains(path) && context.Request.HasFormContentType)
                    {
                        var formData = context.Request.ReadFormAsync().Result;
                        accessToken = formData["access_token"];

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                    }
                }
                return Task.CompletedTask;
            },
            OnTokenValidated = c =>
            {
                var resourceAccess = JObject.Parse(c.Principal.FindFirst("resource_access").Value);
                var clientResource = resourceAccess[c.Options.Audience];

                if (clientResource == null)
                {
                    return Task.CompletedTask;
                }

                var clientRoles = clientResource["roles"];
                var claimsIdentity = c.Principal.Identity as ClaimsIdentity;

                if (claimsIdentity == null)
                {
                    return Task.CompletedTask;
                }

                foreach (var clientRole in clientRoles)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, clientRole.ToString()));
                }

                claimsIdentity.AddClaim(new Claim(AbpClaimTypes.UserName, c.Principal.FindFirst("preferred_username")?.Value));

                var nameIdentifier = claimsIdentity.Claims.Where(w => w.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

                if (nameIdentifier != null)
                {
                    var cacheUsers = context.Services.GetRequiredService<IDistributedCache<KeycloakUserMapCacheItem>>().Get(nameIdentifier.Value);

                    if (cacheUsers != null)
                    {
                        claimsIdentity.RemoveClaim(nameIdentifier);
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, cacheUsers.Id));
                    }
                }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = c =>
            {
                c.NoResult();
                c.Response.StatusCode = 401;
                c.Response.ContentType = "text/plain";
                return c.Response.WriteAsync(c.Exception.ToString());
            }
        };
    }
}