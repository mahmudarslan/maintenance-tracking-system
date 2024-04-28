using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Arslan.Vms.Shared.Hosting.Microservices.Auths;

public interface IDynamicJwtBearerAuthorityResolver
{
    public TimeSpan ExpirationOfConfiguration { get; }

    public Task<string> ResolveAuthority(HttpContext httpContext);
}