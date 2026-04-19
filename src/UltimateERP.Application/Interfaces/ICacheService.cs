namespace UltimateERP.Application.Interfaces;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
}

/// <summary>
/// Marker interface for queries whose results should be cached.
/// </summary>
public interface ICacheableQuery
{
    string CacheKey { get; }
    TimeSpan? CacheExpiration => null;
}
