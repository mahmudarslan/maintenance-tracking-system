using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityModel;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Arslan.Vms.Shared.Hosting.Microservices.Auths
{
    [Dependency(ReplaceServices = true)]
    public class CustomIdentityModelAuthenticationService : IdentityModelAuthenticationService
    {
        private readonly IDistributedCache<TenantCache> _cachedTenants;
        private readonly IConfiguration _configuration;

        public CustomIdentityModelAuthenticationService(IOptions<AbpIdentityClientOptions> options,
            ICancellationTokenProvider cancellationTokenProvider,
            IHttpClientFactory httpClientFactory,
            ICurrentTenant currentTenant,
            IOptions<IdentityModelHttpRequestMessageOptions> identityModelHttpRequestMessageOptions,
            IDistributedCache<IdentityModelTokenCacheItem> tokenCache,
            IDistributedCache<IdentityModelDiscoveryDocumentCacheItem> discoveryDocumentCache,
            IAbpHostEnvironment abpHostEnvironment,
            IDistributedCache<TenantCache> cachedTenants,
            IConfiguration configuration) :
            base(options,
                cancellationTokenProvider,
                httpClientFactory,
                currentTenant,
                identityModelHttpRequestMessageOptions,
                tokenCache,
                discoveryDocumentCache,
                abpHostEnvironment)
        {
            _cachedTenants = cachedTenants;
            _configuration = configuration;
        }

        protected override async Task<string> GetAccessTokenOrNullAsync(string identityClientName)
        {
            //return base.GetAccessTokenOrNullAsync("Tenant");	
            IdentityClientConfiguration clientConfiguration = ClientOptions.GetClientConfiguration(CurrentTenant, "Tenant");

            if (clientConfiguration == null)
            {
                //Logger.LogWarning("Could not find IdentityClientConfiguration for " + identityClientName + ". Either define a configuration for " + identityClientName + " or set a default configuration.");
                //return null;

                var tenants = _cachedTenants.Get("TenantCacheItem");

                if (tenants != null)
                {
                    clientConfiguration = new IdentityClientConfiguration(authority: _configuration["IdentityClients:Tenant.19cc716b-73ca-4bce-a7ee-37e95366e8c7:Authority"],
                                                                          scope: _configuration["IdentityClients:Tenant.19cc716b-73ca-4bce-a7ee-37e95366e8c7:Scope"],
                                                                          clientId: _configuration["IdentityClients:Tenant.19cc716b-73ca-4bce-a7ee-37e95366e8c7:ClientId"],
                                                                          clientSecret: _configuration["IdentityClients:Tenant.19cc716b-73ca-4bce-a7ee-37e95366e8c7:ClientSecret"],
                                                                          grantType: _configuration["IdentityClients:Tenant.19cc716b-73ca-4bce-a7ee-37e95366e8c7:GrantType"],
                                                                          userName: _configuration["IdentityClients:Tenant.19cc716b-73ca-4bce-a7ee-37e95366e8c7:UserName"],
                                                                          userPassword: _configuration["IdentityClients:Tenant.19cc716b-73ca-4bce-a7ee-37e95366e8c7:UserPassword"],
                                                                          requireHttps: false,
                                                                          cacheAbsoluteExpiration: int.Parse(_configuration["IdentityClients:Tenant.19cc716b-73ca-4bce-a7ee-37e95366e8c7:CacheAbsoluteExpiration"]),
                                                                          validateIssuerName: true,
                                                                          validateEndpoints: true);

                    var tenant = tenants.Items.FirstOrDefault(f => f.Id == CurrentTenant.Id)?.Name;
                    var t = clientConfiguration.Authority.Split("/").Last();
                    var auth = clientConfiguration.Authority.Replace(t, tenant);

                    clientConfiguration.Authority = auth;
                }
            }

            return await GetAccessTokenAsync(clientConfiguration).ConfigureAwait(continueOnCapturedContext: false);
        }

        protected override void AddHeaders(HttpClient client)
        {
            base.AddHeaders(client);
        }

        protected override Task AddParametersToRequestAsync(IdentityClientConfiguration configuration, ProtocolRequest request)
        {
            return base.AddParametersToRequestAsync(configuration, request);
        }

        protected override string CalculateDiscoveryDocumentCacheKey(IdentityClientConfiguration configuration)
        {
            return base.CalculateDiscoveryDocumentCacheKey(configuration);
        }

        protected override string CalculateTokenCacheKey(IdentityClientConfiguration configuration)
        {
            return base.CalculateTokenCacheKey(configuration);
        }

        protected override Task<ClientCredentialsTokenRequest> CreateClientCredentialsTokenRequestAsync(IdentityClientConfiguration configuration)
        {
            return base.CreateClientCredentialsTokenRequestAsync(configuration);
        }

        protected override Task<PasswordTokenRequest> CreatePasswordTokenRequestAsync(IdentityClientConfiguration configuration)
        {
            return base.CreatePasswordTokenRequestAsync(configuration);
        }

        public override Task<string> GetAccessTokenAsync(IdentityClientConfiguration configuration)
        {
            return base.GetAccessTokenAsync(configuration);
        }

        protected override Task<IdentityModelDiscoveryDocumentCacheItem> GetDiscoveryResponse(IdentityClientConfiguration configuration)
        {
            return base.GetDiscoveryResponse(configuration);
        }

        protected override Task<TokenResponse> GetTokenResponse(IdentityClientConfiguration configuration)
        {
            return base.GetTokenResponse(configuration);
        }

        protected override Task<TokenResponse> RequestDeviceAuthorizationAsync(HttpClient httpClient, IdentityClientConfiguration configuration)
        {
            return base.RequestDeviceAuthorizationAsync(httpClient, configuration);
        }

        protected override void SetAccessToken(HttpClient client, string accessToken)
        {
            base.SetAccessToken(client, accessToken);
        }
    }
}