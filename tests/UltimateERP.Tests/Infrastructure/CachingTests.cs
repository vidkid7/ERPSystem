using Microsoft.Extensions.Caching.Memory;
using UltimateERP.Infrastructure.Caching;

namespace UltimateERP.Tests.Infrastructure;

public class CachingTests
{
    private readonly MemoryCacheService _cacheService;

    public CachingTests()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        _cacheService = new MemoryCacheService(memoryCache);
    }

    [Fact]
    public async Task SetAsync_ThenGetAsync_ReturnsValue()
    {
        await _cacheService.SetAsync("key1", "value1");
        var result = await _cacheService.GetAsync<string>("key1");
        Assert.Equal("value1", result);
    }

    [Fact]
    public async Task GetAsync_NonExistentKey_ReturnsNull()
    {
        var result = await _cacheService.GetAsync<string>("nonexistent");
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_RemovesCachedValue()
    {
        await _cacheService.SetAsync("key2", "value2");
        await _cacheService.RemoveAsync("key2");
        var result = await _cacheService.GetAsync<string>("key2");
        Assert.Null(result);
    }

    [Fact]
    public async Task SetAsync_WithExpiration_RespectsExpiration()
    {
        await _cacheService.SetAsync("key3", "value3", TimeSpan.FromMilliseconds(50));
        var beforeExpiry = await _cacheService.GetAsync<string>("key3");
        Assert.Equal("value3", beforeExpiry);

        await Task.Delay(100);
        var afterExpiry = await _cacheService.GetAsync<string>("key3");
        Assert.Null(afterExpiry);
    }

    [Fact]
    public async Task SetAsync_ComplexObject_ReturnsCorrectly()
    {
        var obj = new TestCacheItem { Id = 1, Name = "Test" };
        await _cacheService.SetAsync("complex", obj);
        var result = await _cacheService.GetAsync<TestCacheItem>("complex");
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task SetAsync_OverwritesExistingKey()
    {
        await _cacheService.SetAsync("key4", "original");
        await _cacheService.SetAsync("key4", "updated");
        var result = await _cacheService.GetAsync<string>("key4");
        Assert.Equal("updated", result);
    }

    private class TestCacheItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
