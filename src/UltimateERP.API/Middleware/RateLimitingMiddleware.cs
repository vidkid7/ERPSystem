using System.Collections.Concurrent;
using System.Net;
using UltimateERP.Application.Common.Models;

namespace UltimateERP.API.Middleware;

/// <summary>
/// Simple in-memory rate limiter: max 100 requests per minute per IP.
/// </summary>
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private static readonly ConcurrentDictionary<string, ClientRateInfo> _clients = new();
    private const int MaxRequests = 100;
    private static readonly TimeSpan Window = TimeSpan.FromMinutes(1);

    public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var now = DateTime.UtcNow;

        var clientInfo = _clients.GetOrAdd(clientIp, _ => new ClientRateInfo(now));

        lock (clientInfo)
        {
            if (now - clientInfo.WindowStart >= Window)
            {
                clientInfo.WindowStart = now;
                clientInfo.RequestCount = 0;
            }

            clientInfo.RequestCount++;

            if (clientInfo.RequestCount > MaxRequests)
            {
                _logger.LogWarning("Rate limit exceeded for IP {ClientIp}", clientIp);

                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.ContentType = "application/json";
                context.Response.Headers["Retry-After"] = "60";

                var response = ApiResponse.Failure("Rate limit exceeded. Please try again later.");
                context.Response.WriteAsJsonAsync(response).GetAwaiter().GetResult();
                return;
            }
        }

        await _next(context);
    }

    private class ClientRateInfo
    {
        public DateTime WindowStart;
        public int RequestCount;

        public ClientRateInfo(DateTime windowStart)
        {
            WindowStart = windowStart;
            RequestCount = 0;
        }
    }
}
