using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityModel;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Arslan.Vms.Shared.Hosting.Microservices
{
    [Dependency(ReplaceServices = true)]
    public class CustomIdentityModelAuthenticationService : IdentityModelAuthenticationService
    {
        public CustomIdentityModelAuthenticationService(IOptions<AbpIdentityClientOptions> options,
            ICancellationTokenProvider cancellationTokenProvider,
            IHttpClientFactory httpClientFactory,
            ICurrentTenant currentTenant,
            IOptions<IdentityModelHttpRequestMessageOptions> identityModelHttpRequestMessageOptions,
            IDistributedCache<IdentityModelTokenCacheItem> tokenCache,
            IDistributedCache<IdentityModelDiscoveryDocumentCacheItem> discoveryDocumentCache,
            IAbpHostEnvironment abpHostEnvironment) :
            base(options,
                cancellationTokenProvider,
                httpClientFactory,
                currentTenant,
                identityModelHttpRequestMessageOptions,
                tokenCache,
                discoveryDocumentCache,
                abpHostEnvironment)
        {

        }

        protected override Task<string> GetAccessTokenOrNullAsync(string identityClientName)
        {
            return base.GetAccessTokenOrNullAsync("Tenant");
        }
    }
}
