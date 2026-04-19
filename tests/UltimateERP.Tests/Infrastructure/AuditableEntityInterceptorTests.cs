using Microsoft.EntityFrameworkCore;
using UltimateERP.Domain.Common;
using UltimateERP.Domain.Interfaces;
using UltimateERP.Infrastructure.Persistence.Interceptors;
using Moq;

namespace UltimateERP.Tests.Infrastructure;

public class TestEntity : BaseEntity
{
    public string? Description { get; set; }
}

public class TestDbContext : DbContext
{
    public DbSet<TestEntity> TestEntities => Set<TestEntity>();

    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
}

public class AuditableEntityInterceptorTests
{
    private readonly Mock<ICurrentUserService> _currentUserMock;
    private readonly Mock<IDateTimeService> _dateTimeMock;
    private readonly AuditableEntityInterceptor _interceptor;

    public AuditableEntityInterceptorTests()
    {
        _currentUserMock = new Mock<ICurrentUserService>();
        _currentUserMock.Setup(x => x.UserId).Returns(42);

        _dateTimeMock = new Mock<IDateTimeService>();
        _dateTimeMock.Setup(x => x.UtcNow).Returns(new DateTime(2026, 1, 15, 10, 0, 0, DateTimeKind.Utc));

        _interceptor = new AuditableEntityInterceptor(_currentUserMock.Object, _dateTimeMock.Object);
    }

    [Fact]
    public async Task AddedEntity_ShouldSetCreatedFields()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_Add")
            .AddInterceptors(_interceptor)
            .Options;

        using var context = new TestDbContext(options);
        var entity = new TestEntity { Name = "Test", Code = "T1" };
        context.TestEntities.Add(entity);
        await context.SaveChangesAsync();

        Assert.Equal(42, entity.CUserId);
        Assert.Equal(new DateTime(2026, 1, 15, 10, 0, 0, DateTimeKind.Utc), entity.CreatedDate);
    }

    [Fact]
    public async Task ModifiedEntity_ShouldSetModifiedFields()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_Modify")
            .AddInterceptors(_interceptor)
            .Options;

        using var context = new TestDbContext(options);
        var entity = new TestEntity { Name = "Test", Code = "T1" };
        context.TestEntities.Add(entity);
        await context.SaveChangesAsync();

        entity.Name = "Updated";
        context.TestEntities.Update(entity);
        await context.SaveChangesAsync();

        Assert.Equal(42, entity.ModifiedBy);
        Assert.Equal(new DateTime(2026, 1, 15, 10, 0, 0, DateTimeKind.Utc), entity.ModifiedDate);
    }

    [Fact]
    public async Task DeletedEntity_ShouldSoftDelete()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_Delete")
            .AddInterceptors(_interceptor)
            .Options;

        using var context = new TestDbContext(options);
        var entity = new TestEntity { Name = "Test", Code = "T1" };
        context.TestEntities.Add(entity);
        await context.SaveChangesAsync();

        context.TestEntities.Remove(entity);
        await context.SaveChangesAsync();

        Assert.True(entity.IsDeleted);
        Assert.Equal(42, entity.ModifiedBy);
    }
}
