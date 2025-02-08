using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RateLimiter.Services.Abstract;

namespace RateLimiter.Services.Concrete;

internal class RedisCacheService(ILogger<RedisCacheService> logger, IDistributedCache cache) : ICacheService
{
    public async Task<string?> GetStringAsync(string key)
    {
        logger.LogInformation("Getting value from cache for key: {key}", key);
        
        var value = await cache.GetStringAsync(key);

        logger.LogInformation("Value retrieved from cache for key: {key}", key);

        return value;
    }

    public async Task SetStringAsync(string key, string value, TimeSpan absoluteTimeInterval, TimeSpan? slidingTimeInterval = null)
    {
        logger.LogInformation("Setting value in cache for key: {key}", key);

        await cache.SetStringAsync(key, value, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteTimeInterval,
            SlidingExpiration = slidingTimeInterval
        });

        logger.LogInformation("Value set in cache for key: {key}", key);
    }
}
