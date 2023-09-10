using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Arslan.Vms.Shared.Hosting.AspNetCore.Swagger;

public class AbpSwashbuckleDocumentFilter : IDocumentFilter
{
    public virtual void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc
            .Paths
            .RemoveAll(path => path.Key.StartsWith("/api/abp/"));
    }
}