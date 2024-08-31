using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.redis;

public class RedisCacheClient : ICacheClient
{
    private readonly IDistributedCache _cache;

    public RedisCacheClient(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public async Task<T?> GetOrSetAndGetFromCache<T>(string key, Func<Task<T>> cb)
    {
        var stringValue = await _cache.GetStringAsync(key);

        if (stringValue != null)
        {
            return JsonSerializer.Deserialize<T>(stringValue)!;
        }

        var entities = await cb();

        if (entities != null)
        {
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(entities), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            });
        }

        return entities;
    }
}