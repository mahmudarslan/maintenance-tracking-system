using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Threading.Tasks;

namespace Arslan.Vms.Shared.Hosting.Microservices.Auths;

public interface IDynamicJwtBearerHanderConfigurationResolver
{
    Task<OpenIdConnectConfiguration> ResolveCurrentOpenIdConfiguration(HttpContext context);
}