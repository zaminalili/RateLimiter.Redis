using Microsoft.AspNetCore.Http;
using RateLimiter.Services.Abstract;

namespace RateLimiter.Middlewares;

public class RateLimiterMiddleware(IRateLimiterService rateLimiter) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";


        bool isRateLimited = await rateLimiter.IsRateLimitedAsync(ipAddress);

        if (isRateLimited)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            return;
        }

        await next.Invoke(context);
    }
}
