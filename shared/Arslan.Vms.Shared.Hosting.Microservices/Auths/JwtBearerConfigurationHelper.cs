using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Arslan.Vms.Shared.Hosting.Microservices.Auths;

[UnitOfWork]
public static class JwtBearerConfigurationHelper
{
    public static void Configure(
        ServiceConfigurationContext context,
        string audience,
        bool isGateway = false)
    {
        var configuration = context.Services.GetConfiguration();

        var auth = context.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddDynamicJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.Events = GetJwtBearerEvent(configuration, context, isGateway);
                    options.Audience = configuration["AuthServer:Audience"];
                    options.IncludeErrorDetails = true;
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.RefreshOnIssuerKeyNotFound = false;
                    options.BackchannelHttpHandler = GetHttpClientHandler();
                    options.TokenValidationParameters = GetTokenValidationParameters(configuration["AuthServer:Authority"], new List<string> { "master-realm", "account" });
                })
                .AddDynamicAuthorityJwtBearerResolver<ResolveAuthorityService>();

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

    static JwtBearerEvents GetJwtBearerEvent(IConfiguration configuration, ServiceConfigurationContext context, bool isGateway)
    {
        return new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                try
                {
                    var accessToken = context.Request.Query["access_token"];

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }
                    else if (string.IsNullOrEmpty(context.Token))
                    {
                        string authorization = context.Request.Headers[HeaderNames.Authorization];

                        if (authorization?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ?? false)
                        {
                            context.Token = authorization.Substring("Bearer ".Length).Trim();
                        }
                        else if (string.IsNullOrEmpty(context.Token) && context.Request.HasFormContentType)
                        {
                            context.Request.EnableBuffering();
                            var formData = context.Request.ReadFormAsync().Result;
                            accessToken = formData["access_token"];

                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                        }

                        if (string.IsNullOrEmpty(context.Token) && context.Request.Cookies.Any())
                        {
                            context.Token = context.Request.Cookies["HangFireCookie"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

                return Task.CompletedTask;
            },
            OnTokenValidated = c =>
            {
                //try
                //{
                //var resourceAccess = JObject.Parse(c.Principal.FindFirst("resource_access").Value);
                //var clientResource = resourceAccess[c.Options.Audience];

                //if (clientResource == null)
                //{
                //	return Task.CompletedTask;
                //}

                //var clientRoles = clientResource["roles"];
                //var claimsIdentity = c.Principal.Identity as ClaimsIdentity;

                //if (claimsIdentity == null)
                //{
                //	return Task.CompletedTask;
                //}

                //foreach (var clientRole in clientRoles)
                //{
                //	claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, clientRole.ToString()));
                //}

                //claimsIdentity.AddClaim(new Claim(AbpClaimTypes.UserName, c.Principal.FindFirst("preferred_username")?.Value));

                //if (!isGateway)
                //{
                //	var nameIdentifier = claimsIdentity.Claims.Where(w => w.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

                //	if (nameIdentifier != null)
                //	{
                //		//var tenantId = claimsIdentity.Claims.Where(w => w.Type == AbpClaimTypes.TenantId).FirstOrDefault()?.Value;

                //		var cacheUsers = context.Services.GetRequiredService<IDistributedCache<KeycloakUserMapCacheItem>>().Get(nameIdentifier.Value);

                //		if (cacheUsers != null)
                //		{
                //			claimsIdentity.RemoveClaim(nameIdentifier);
                //			claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, cacheUsers.Id));
                //		}
                //	}
                //}
                //}
                //catch (Exception ex)
                //{
                //	throw;
                //}

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = c =>
            {
                c.NoResult();
                c.Response.StatusCode = 401;
                c.Response.ContentType = "text/plain";
                return c.Response.WriteAsync(c.Exception.ToString());
            },
            OnForbidden = c =>
            {
                return Task.CompletedTask;
            },
            OnChallenge = c =>
            {
                return Task.CompletedTask;
            }
        };
    }
}