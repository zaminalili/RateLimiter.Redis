namespace RateLimiter.Configure;

public class RateLimiterOptions
{
    public int MaxRequestCount { get; set; } = 100;
    public TimeSpan TimeInterval { get; set; } = TimeSpan.FromSeconds(10);
}
