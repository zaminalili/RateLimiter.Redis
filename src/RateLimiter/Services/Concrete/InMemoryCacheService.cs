using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RateLimiter.Services.Abstract;

namespace RateLimiter.Services.Concrete;

internal class InMemoryCacheService(ILogger<InMemoryCacheService> logger, IMemoryCache cacheService) : ICacheService
{
    public async Task<string?> GetStringAsync(string key)
    {
        logger.LogInformation("Getting value from cache for key: {key}", key);

        var result = await Task.FromResult(cacheService.Get<string>(key));

        logger.LogInformation("Value retrieved from cache for key: {key}", key);

        return result;
    }

    public async Task SetStringAsync(string key, string value, TimeSpan absoluteTimeInterval, TimeSpan? slidingTimeInterval = null)
    {
        logger.LogInformation("Setting value in cache for key: {key}", key);

        cacheService.Set(key, value, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteTimeInterval,
            SlidingExpiration = slidingTimeInterval
        });

        logger.LogInformation("Value set in cache for key: {key}", key);
        logger.LogInformation("Task completed");

        await Task.CompletedTask;
    }
}
