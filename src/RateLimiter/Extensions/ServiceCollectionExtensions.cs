using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RateLimiter.Configure;
using RateLimiter.Middlewares;
using RateLimiter.Services.Abstract;
using RateLimiter.Services.Concrete;

namespace RateLimiter.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRateLimiter(this IServiceCollection services, string connectionString, Action<RateLimiterOptions> options)
    {
        services.Configure(options);

        services.AddStackExchangeRedisCache(opt => opt.Configuration = connectionString);

        services.AddScoped<IRateLimiterService, RateLimiterService>();
        services.AddScoped<RateLimiterMiddleware>();

        return services;
    }

    public static IApplicationBuilder UseRateLimiter(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RateLimiterMiddleware>();
    }
}
