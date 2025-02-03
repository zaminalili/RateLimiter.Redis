namespace RateLimiter.Services.Abstract;

public interface IRateLimiterService
{
    Task<bool> IsRateLimitedAsync(string ipAddress);
}
