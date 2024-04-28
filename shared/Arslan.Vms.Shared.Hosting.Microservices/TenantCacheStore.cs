using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.Shared.Hosting.Microservices;

public class TenantCacheStore : ITenantCacheStore
{
    private readonly IDistributedCache<TenantCache> _distributedCache;
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly IConfiguration _configuration;
    private readonly string CacheKey = "TenantCacheItem";

    public TenantCacheStore(IDistributedCache<TenantCache> distributedCache,
                            IConfiguration configuration,
                            IDistributedEventBus distributedEventBus)
    {
        _distributedCache = distributedCache;
        _configuration = configuration;
        _distributedEventBus = distributedEventBus;
    }

    public async Task<List<TenantCacheItemModel>> SetCache()
    {
        var tenantItems = new List<TenantCacheItemModel>();

        var t = _configuration.GetSection("Tenants").Get<List<TenantConfiguration>>() ?? new List<TenantConfiguration>();

        foreach (var item in t)
        {
            if (tenantItems.Any(a => a.Name == item.Name))
            {
                continue;
            }

            tenantItems.Add(new TenantCacheItemModel() { Id = item.Id, Name = item.Name });
        }

        var tenantCache = new TenantCache()
        {
            Items = tenantItems
        };

        await _distributedCache.SetAsync(CacheKey, tenantCache, new DistributedCacheEntryOptions
        {
            SlidingExpiration = null
        });

        await _distributedEventBus.PublishAsync(new TenantRefreshEto());

        return tenantItems;
    }

    public async Task<List<TenantCacheItemModel>> GetCache()
    {
        var tenants = _distributedCache.Get(CacheKey);

        if (tenants == null || tenants.Items.Count == 0)
        {
            return await SetCache();
        }

        return tenants.Items;
    }
}

public interface ITenantCacheStore : ITransientDependency
{
    Task<List<TenantCacheItemModel>> SetCache();
    Task<List<TenantCacheItemModel>> GetCache();
}

[IgnoreMultiTenancy]
[CacheName("TenantCacheItem")]
public class TenantCache
{
    public List<TenantCacheItemModel> Items { get; set; } = new List<TenantCacheItemModel>();
}

public class TenantCacheItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}


[EventName("TenantRefreshEto")]
public class TenantRefreshEto
{
}