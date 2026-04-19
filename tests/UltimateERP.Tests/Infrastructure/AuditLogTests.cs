using Microsoft.EntityFrameworkCore;
using UltimateERP.Domain.Entities.Security;
using UltimateERP.Infrastructure.Persistence;
using UltimateERP.Infrastructure.Services;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Tests.Infrastructure;

public class AuditLogTests
{
    private ERPDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ERPDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new ERPDbContext(options);
    }

    [Fact]
    public async Task LogAsync_CreatesAuditLogEntry()
    {
        using var context = CreateContext("AuditLog_Create");
        var service = new AuditLogService(context);

        await service.LogAsync("Create", "Product", "42", "Created product X", "user1");

        var logs = await context.AuditLogs.ToListAsync();
        Assert.Single(logs);
        Assert.Equal("Create", logs[0].Action);
        Assert.Equal("Product", logs[0].EntityType);
        Assert.Equal("42", logs[0].EntityId);
        Assert.Equal("user1", logs[0].UserId);
    }

    [Fact]
    public async Task GetLogsAsync_ReturnsFilteredLogs()
    {
        using var context = CreateContext("AuditLog_Filter");
        var service = new AuditLogService(context);

        await service.LogAsync("Create", "Product", "1", "Created", "user1");
        await service.LogAsync("Update", "Product", "1", "Updated", "user1");
        await service.LogAsync("Delete", "Customer", "2", "Deleted", "user2");

        var filter = new AuditLogFilterDto { EntityType = "Product" };
        var results = await service.GetLogsAsync(filter);

        Assert.Equal(2, results.Count);
        Assert.All(results, r => Assert.Equal("Product", r.EntityType));
    }

    [Fact]
    public async Task GetLogsAsync_FilterByUserId()
    {
        using var context = CreateContext("AuditLog_UserFilter");
        var service = new AuditLogService(context);

        await service.LogAsync("Create", "Product", "1", "Created", "user1");
        await service.LogAsync("Update", "Product", "2", "Updated", "user2");

        var filter = new AuditLogFilterDto { UserId = "user2" };
        var results = await service.GetLogsAsync(filter);

        Assert.Single(results);
        Assert.Equal("user2", results[0].UserId);
    }

    [Fact]
    public async Task GetLogsAsync_ReturnsInDescendingOrder()
    {
        using var context = CreateContext("AuditLog_Order");
        var service = new AuditLogService(context);

        await service.LogAsync("First", "Product", "1", "First entry", "user1");
        await Task.Delay(10);
        await service.LogAsync("Second", "Product", "2", "Second entry", "user1");

        var filter = new AuditLogFilterDto();
        var results = await service.GetLogsAsync(filter);

        Assert.Equal(2, results.Count);
        Assert.True(results[0].Timestamp >= results[1].Timestamp);
    }
}
