using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Arslan.Vms.Shared.Hosting.AspNetCore.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    readonly IApiVersionDescriptionProvider provider;
    static string envApiVersion;
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
    /// </summary>
    /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider,
        IWebHostEnvironment env)
    {
        var version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        var environment = $"{env.EnvironmentName} - ";

        if (env.IsProduction())
        {
            environment = "";
            version = version.Split('-').FirstOrDefault();
        }

        envApiVersion = $"{environment}{version}";

        this.provider = provider;
    }

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        var title = options.SwaggerGeneratorOptions.SwaggerDocs["a"].Title;
        options.SwaggerGeneratorOptions.SwaggerDocs.Clear();
        // add a swagger document for each discovered API version
        // note: you might choose to skip or document deprecated API versions differently
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, title));
        }
    }

    static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, string title)
    {
        var info = new OpenApiInfo()
        {
            Title = $"Vms {title} [{envApiVersion}]",
            Version = description.ApiVersion.ToString(),
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}