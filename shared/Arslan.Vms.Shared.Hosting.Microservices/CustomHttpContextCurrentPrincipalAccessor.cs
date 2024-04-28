using Arslan.Vms.Shared.Hosting.Microservices.Auths;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Volo.Abp.AspNetCore.Security.Claims;
using Volo.Abp.Caching;
using Volo.Abp.Security.Claims;

namespace Arslan.Vms.Shared.Hosting.Microservices;

public class CustomHttpContextCurrentPrincipalAccessor : HttpContextCurrentPrincipalAccessor
{
    private readonly IDistributedCache<KeycloakUserMapCacheItem> _distributedCache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;

    public CustomHttpContextCurrentPrincipalAccessor(
        IHttpContextAccessor httpContextAccessor,
        IDistributedCache<KeycloakUserMapCacheItem> distributedCache,
        IConfiguration configuration,
        IMemoryCache memoryCache
        ) : base(httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _distributedCache = distributedCache;
        _configuration = configuration;
        _memoryCache = memoryCache;
    }

    protected override ClaimsPrincipal GetClaimsPrincipal()
    {
        ClaimsPrincipal principal;

        try
        {
            principal = _httpContextAccessor.HttpContext?.User ?? (Thread.CurrentPrincipal as ClaimsPrincipal)!;
        }
        catch (System.Exception)
        {
            principal = null;
        }

        if (principal == null)
        {
            return principal;
        }

        var claimsIdentity = principal.Identity as ClaimsIdentity;

        var nameIdentifier = claimsIdentity.Claims.Where(w => w.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

        if (nameIdentifier == null)
        {
            return principal;
        }

        var cachedUser = claimsIdentity.Claims.Where(w => w.Type == "cachedUser").FirstOrDefault();

        if (cachedUser != null)
        {
            return principal;
        }

        var isAdminUser = claimsIdentity.Claims.Where(w => w.Type == "preferred_username").FirstOrDefault().Value.ToString() == "admin@arslan.io";


        var cacheUserId = GetUserId(nameIdentifier.Value, isAdminUser); //_distributedCache.Get(nameIdentifier.Value);

        if (cacheUserId == null)
        {
            return principal;
        }

        var resourceAccess = JObject.Parse(principal.FindFirst("resource_access").Value);
        var clientResource = resourceAccess[_configuration["AuthServer:Audience"]];

        if (clientResource != null)
        {
            var clientRoles = clientResource["roles"];

            if (claimsIdentity != null)
            {
                foreach (var clientRole in clientRoles)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, clientRole.ToString()));
                }

                claimsIdentity.AddClaim(new Claim(AbpClaimTypes.UserName, principal.FindFirst("preferred_username")?.Value));
            }

            claimsIdentity.RemoveClaim(nameIdentifier);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, cacheUserId));
            claimsIdentity.AddClaim(new Claim("cachedUser", cacheUserId));
        }

        return principal;
    }


    string GetUserId(string userId, bool isAdminUser)
    {
        if (isAdminUser)
        {
            return userId;
        }

        var keyprefix = "keycloakMapId:";

        var cachedValue = _memoryCache.Get<string>(keyprefix + userId);

        if (cachedValue == null)
        {
            var distributedCacheValue = _distributedCache.Get(key: userId, hideErrors: true)?.Id;

            if (distributedCacheValue != null && !string.IsNullOrEmpty(distributedCacheValue))
            {
                _memoryCache.Set(
                    key: keyprefix + userId,
                    value: distributedCacheValue,
                    options: new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = null
                    });
            }

            return distributedCacheValue;
        }
        else
        {
            return cachedValue;
        }
    }
}