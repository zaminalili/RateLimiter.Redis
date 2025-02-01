using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using RateLimiter.Configure;
using RateLimiter.Services.Abstract;

namespace RateLimiter.Services.Concrete;

internal class RateLimiterService(IOptions<RateLimiterOptions> options, IDistributedCache cache) : IRateLimiterService
{
    private readonly int _maxRequestCount = options.Value.MaxRequestCount;
    private readonly TimeSpan _timeInterval = options.Value.TimeInterval;

    private async Task<int> IncreaseAsync(string key, string? value)
    {
        int counter = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
        counter++;

        await cache.SetStringAsync(key, counter.ToString(), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _timeInterval
        });

        return counter;
    }

    public async Task<bool> IsRateLimitedAsync(string ipAddress)
    {
        var value = await cache.GetStringAsync(ipAddress);

        int counter = await IncreaseAsync(ipAddress, value);

        return counter > _maxRequestCount;
    }
}
