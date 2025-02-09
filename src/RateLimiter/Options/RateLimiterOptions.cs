namespace RateLimiter.Options;

public class RateLimiterOptions
{
    public CacheType CacheType { get; set; }
    public int MaxRequestCount { get; set; }
    public TimeSpan TimeInterval { get; set; }
}
