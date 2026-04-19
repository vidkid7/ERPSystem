using UltimateERP.Domain.Common;

namespace UltimateERP.Tests.Domain;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity { }

    [Fact]
    public void NewEntity_ShouldHaveDefaults()
    {
        var entity = new TestEntity();

        Assert.True(entity.IsActive);
        Assert.False(entity.IsDeleted);
        Assert.True(entity.CreatedDate <= DateTime.UtcNow);
        Assert.Null(entity.ModifiedDate);
        Assert.Null(entity.ModifiedBy);
    }

    [Fact]
    public void SoftDelete_ShouldSetIsDeleted()
    {
        var entity = new TestEntity { IsDeleted = true };

        Assert.True(entity.IsDeleted);
    }

    [Fact]
    public void Entity_ShouldSupportCodeAndName()
    {
        var entity = new TestEntity
        {
            Code = "TST001",
            Name = "Test Entity",
            Alias = "TE"
        };

        Assert.Equal("TST001", entity.Code);
        Assert.Equal("Test Entity", entity.Name);
        Assert.Equal("TE", entity.Alias);
    }
}
