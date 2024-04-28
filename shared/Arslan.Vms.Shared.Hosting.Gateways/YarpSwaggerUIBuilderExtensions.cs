using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Yarp.ReverseProxy.Configuration;

namespace Arslan.Vms.Shared.Hosting.Gateways;

public static class YarpSwaggerUIBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerUIWithYarp(this IApplicationBuilder app,
        ApplicationInitializationContext context)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = context.ServiceProvider.GetRequiredService<ILogger<ApplicationInitializationContext>>();
            var proxyConfigProvider = context.ServiceProvider.GetRequiredService<IProxyConfigProvider>();
            var yarpConfig = proxyConfigProvider.GetConfig();

            var routedClusters = yarpConfig.Clusters
                .SelectMany(t => t.Destinations,
                    (clusterId, destination) => new { clusterId.ClusterId, destination.Value });

            var groupedClusters = routedClusters
                .GroupBy(q => q.Value.Address)
                .Select(t => t.First())
                .Distinct()
                .ToList();

            foreach (var clusterGroup in groupedClusters)
            {
                var routeConfig = yarpConfig.Routes.Where(w => w.RouteId.StartsWith("Swagger")).FirstOrDefault(q =>
                    q.ClusterId == clusterGroup.ClusterId);

                if (routeConfig == null)
                {
                    logger.LogWarning($"Swagger UI: Couldn't find route configuration for {clusterGroup.ClusterId}...");
                    continue;
                }

                var routes = yarpConfig.Routes.Where(f => f.RouteId.Contains("Swagger") && f.ClusterId == clusterGroup.ClusterId).ToList();
                var version = "v1";

                foreach (var route in routes)
                {
                    if (route.RouteId.Contains("-"))
                    {
                        var routeVersion = route.RouteId.Split("-").Last();

                        if (!string.IsNullOrEmpty(routeVersion))
                        {
                            version = routeVersion;
                            options.SwaggerEndpoint($"{clusterGroup.ClusterId}/{version}{configuration["App:PathBase"]}/swagger/{version}/swagger.json", $"{route.RouteId.Replace("Swagger", "")}");
                        }
                    }
                    else
                    {
                        options.SwaggerEndpoint($"{clusterGroup.ClusterId}{configuration["App:PathBase"]}/swagger/{version}/swagger.json", $"{route.RouteId.Replace("Swagger", "")}-{version}");
                    }

                    options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                    options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
                }
            }
        });

        return app;
    }
}