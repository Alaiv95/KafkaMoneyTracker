namespace Infrastructure.redis;

public interface ICacheClient
{
    Task<T?> GetOrSetAndGetFromCache<T>(string key, Func<Task<T>> cb);

    Task InvalidateCache(string key);
}