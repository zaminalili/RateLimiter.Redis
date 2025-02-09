using Microsoft.Extensions.Options;
using RateLimiter.Options;
using RateLimiter.Services.Abstract;

namespace RateLimiter.Services.Concrete;

internal class RateLimiterService(IOptions<RateLimiterOptions> options, ICacheService cacheService) : IRateLimiterService
{
    private readonly int maxRequestCount = options.Value.MaxRequestCount;
    private readonly TimeSpan timeInterval = options.Value.TimeInterval;

    private async Task<int> IncreaseAsync(string key, string? value)
    {
        int counter = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
        counter++;

        await cacheService.SetStringAsync(key, counter.ToString(), timeInterval);

        return counter;
    }

    public async Task<bool> IsRateLimitedAsync(string ipAddress)
    {
        var value = await cacheService.GetStringAsync(ipAddress);

        int counter = await IncreaseAsync(ipAddress, value);

        return counter > maxRequestCount;
    }
}
