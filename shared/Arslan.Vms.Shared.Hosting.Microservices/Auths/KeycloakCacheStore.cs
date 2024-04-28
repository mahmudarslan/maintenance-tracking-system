using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.Shared.Hosting.Microservices.Auths;

public class KeycloakCacheStore : IKeycloakCacheStore
{
    private readonly IDistributedCache<KeycloakUserMapCacheItem> _distributedCache;
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly IConfiguration _configuration;

    public KeycloakCacheStore(IDistributedCache<KeycloakUserMapCacheItem> distributedCache,
                            IConfiguration configuration,
                            IDistributedEventBus distributedEventBus)
    {
        _distributedCache = distributedCache;
        _configuration = configuration;
        _distributedEventBus = distributedEventBus;
    }

    public async Task SetCache(string tenatId)
    {
        try
        {
            if (!string.IsNullOrEmpty(tenatId))
            {
                Guid tenant;

                Guid.TryParse(tenatId, out tenant);

                await _distributedEventBus.PublishAsync(new UserMapRefreshEto() { TenantId = tenant });
            }
        }
        catch (Exception ex)
        { }
    }

    public async Task<KeycloakUserMapCacheItem> GetCache(string userId, string tenatId)
    {
        var user = _distributedCache.Get(userId);

        //if (user == null)
        //{
        //    await SetCache(tenatId);
        //}

        return user;
    }
}

[IgnoreMultiTenancy]
[CacheName("KeycloakUserMapCacheItem")]
public class KeycloakUserMapCacheItem
{
    public string Id { get; set; }
}

[Serializable]
[EventName("Vms.UserMapRefresh")]
public class UserMapRefreshEto : EtoBase, IMultiTenant
{
    public Guid? TenantId { get; set; }
}

public interface IKeycloakCacheStore : ITransientDependency
{
    Task SetCache(string tenatId);
    Task<KeycloakUserMapCacheItem> GetCache(string userId, string tenatId);
}