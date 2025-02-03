using Microsoft.AspNetCore.Http;
using Moq;
using RateLimiter.Services.Abstract;
using Xunit;

namespace RateLimiter.Middlewares.Tests;

public class RateLimiterMiddlewareTests
{
    private readonly Mock<IRateLimiterService> _mockRateLimiterService;
    private readonly Mock<RequestDelegate> _mockRequestDelegate;
    private readonly DefaultHttpContext _context;

    public RateLimiterMiddlewareTests()
    {
        _mockRateLimiterService = new Mock<IRateLimiterService>();
        _mockRequestDelegate = new Mock<RequestDelegate>();
        _context = new DefaultHttpContext();
    }

    [Fact]
    public async void InvokeAsync_RateLimited_Return429()
    {
        // Arrange
        _mockRateLimiterService.Setup(s => s.IsRateLimitedAsync(It.IsAny<string>()))
                        .Returns(Task.FromResult(true));

        var middleware = new RateLimiterMiddleware(_mockRateLimiterService.Object);

        // Act
        await middleware.InvokeAsync(_context, _mockRequestDelegate.Object);

        // Assert
        Xunit.Assert.Equal(StatusCodes.Status429TooManyRequests, _context.Response.StatusCode);

        _mockRequestDelegate.Verify(next => next.Invoke(It.IsAny<HttpContext>()), Times.Never);
    }

    [Fact]
    public async Task InvokeAsync_NotRateLimited_CallNextMiddleware()
    {
        // Arrange
        _mockRateLimiterService.Setup(r => r.IsRateLimitedAsync(It.IsAny<string>())).ReturnsAsync(false);

        var middleware = new RateLimiterMiddleware(_mockRateLimiterService.Object);


        // Act
        await middleware.InvokeAsync(_context, _mockRequestDelegate.Object);

        // Assert
        Xunit.Assert.NotEqual(StatusCodes.Status429TooManyRequests, _context.Response.StatusCode);

        _mockRequestDelegate.Verify(next => next.Invoke(_context), Times.Once);
    }
}