using Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services;

public class CacheService : ICacheService
{
    // Implementation of Object Cache => is use to cache result in memory for a period of time
    private readonly IMemoryCache _memoryCache; 
    // Is considerable good 5 min to cache some result 
    private readonly static TimeSpan _expirationTime = TimeSpan.FromMinutes(5);

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<T?> GetOrCreateAsync<T>(string key,
        Func<CancellationToken, Task<T>> request,
        TimeSpan? expirationTime = null,
        CancellationToken cancellationToken = default)
    {
        // This is going to represent the current request and response of the request and is going to be saved in the cache
        T? result = await _memoryCache.GetOrCreateAsync
            (key,
            entry =>
            {
                // represent when the cache is going to expire
                entry.SetAbsoluteExpiration(expirationTime ?? _expirationTime);

                return request(cancellationToken); 
            });

        return result;
    }

    // remove a object that is cached with the given key
    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}
