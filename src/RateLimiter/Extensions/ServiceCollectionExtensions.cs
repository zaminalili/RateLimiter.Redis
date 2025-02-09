using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RateLimiter.Options;
using RateLimiter.Middlewares;
using RateLimiter.Services.Abstract;
using RateLimiter.Services.Concrete;

namespace RateLimiter.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLimiter(this IServiceCollection services, string redisConnection, Action<RateLimiterOptions> options)
    {
        services.Configure(options);
       
        services.AddStackExchangeRedisCache(opt => opt.Configuration = redisConnection);

        services.AddMemoryCache();

        services.AddSingleton<RedisCacheService>();
        services.AddSingleton<InMemoryCacheService>();

        services.AddTransient<ICacheService>(s =>
        {
            var options = s.GetRequiredService<IOptions<RateLimiterOptions>>().Value;

            return options.CacheType switch
            {
                CacheType.InMemory => s.GetRequiredService<InMemoryCacheService>(),
                CacheType.Redis => s.GetRequiredService<RedisCacheService>(),
                _ => throw new InvalidOperationException("Invalid cache type")
            };
         });

        services.AddScoped<IRateLimiterService, RateLimiterService>();
        services.AddScoped<RateLimiterMiddleware>();

        return services;
    }

    public static IApplicationBuilder UseLimiter(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RateLimiterMiddleware>();
    }
}
