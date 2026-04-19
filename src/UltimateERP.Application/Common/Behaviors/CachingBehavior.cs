using MediatR;
using Microsoft.Extensions.Logging;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that caches results for queries implementing ICacheableQuery.
/// </summary>
public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ICacheService _cache;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

    public CachingBehavior(ICacheService cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICacheableQuery cacheableQuery)
        {
            return await next(cancellationToken);
        }

        var cacheKey = cacheableQuery.CacheKey;
        var cached = await _cache.GetAsync<TResponse>(cacheKey);

        if (cached is not null)
        {
            _logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
            return cached;
        }

        _logger.LogDebug("Cache miss for {CacheKey}, executing handler", cacheKey);
        var response = await next(cancellationToken);

        await _cache.SetAsync(cacheKey, response, cacheableQuery.CacheExpiration);

        return response;
    }
}
