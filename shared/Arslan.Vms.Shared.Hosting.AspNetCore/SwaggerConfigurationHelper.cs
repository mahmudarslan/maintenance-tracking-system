using Arslan.Vms.Shared.Hosting.AspNetCore.Swagger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Volo.Abp.Modularity; 

namespace Arslan.Vms.Shared.Hosting.AspNetCore;

public static class SwaggerConfigurationHelper
{
    public static void Configure(
        ServiceConfigurationContext context,
        string apiTitle)
    {
        context.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = apiTitle, Version = "v1" });
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
        });
    }

    public static void ConfigureWithAuth(ServiceConfigurationContext context, IConfiguration configuration, Dictionary<string, string> scopes, string apiTitle = "", bool hideAbp = true)
    {
        var env = context.Services.GetHostingEnvironment();
        var version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        context.Services.AddAbpApiVersioning(options =>
        {
            options.UseApiBehavior = false;
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        });

        context.Services.AddVersionedApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        context.Services.Configure<SwaggerGenOptions>(options =>
        {
            options.SwaggerDoc("a", new OpenApiInfo() { Title = apiTitle });
        });

        context.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        context.Services.AddAbpSwaggerGen(
        options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{configuration["AuthServer:Authority"]}/protocol/openid-connect/auth"),
                        Scopes = scopes,
                        TokenUrl = new Uri($"{configuration["AuthServer:Authority"]}/protocol/openid-connect/token")
                    }
                }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            if (hideAbp)
            {
                options.DocumentFilter<AbpSwashbuckleDocumentFilter>();
            }
            options.OperationFilter<SwaggerDefaultValues>();
            options.CustomSchemaIds(type => type.FullName);
        });
    }
}