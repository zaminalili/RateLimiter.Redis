namespace RateLimiter.Services.Abstract;

public interface ICacheService
{
    Task<string?> GetStringAsync(string key);
    Task SetStringAsync(string key, string value, TimeSpan absoluteTimeInterval, TimeSpan? slidingTimeInterval = null);
}
