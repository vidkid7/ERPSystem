using UltimateERP.Infrastructure.Auth;

namespace UltimateERP.Tests.Auth;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void HashPassword_ReturnsDifferentHashForSamePassword()
    {
        var hash1 = _hasher.HashPassword("Password123!");
        var hash2 = _hasher.HashPassword("Password123!");

        Assert.NotEqual(hash1, hash2); // different salts
    }

    [Fact]
    public void VerifyPassword_CorrectPassword_ReturnsTrue()
    {
        var hash = _hasher.HashPassword("MySecret!");
        Assert.True(_hasher.VerifyPassword("MySecret!", hash));
    }

    [Fact]
    public void VerifyPassword_WrongPassword_ReturnsFalse()
    {
        var hash = _hasher.HashPassword("MySecret!");
        Assert.False(_hasher.VerifyPassword("WrongPassword!", hash));
    }

    [Fact]
    public void VerifyPassword_MalformedHash_ReturnsFalse()
    {
        Assert.False(_hasher.VerifyPassword("test", "not-a-valid-hash"));
    }

    [Fact]
    public void HashPassword_ProducesFormatWithDotSeparator()
    {
        var hash = _hasher.HashPassword("test");
        Assert.Contains('.', hash);
        Assert.Equal(2, hash.Split('.').Length);
    }
}
