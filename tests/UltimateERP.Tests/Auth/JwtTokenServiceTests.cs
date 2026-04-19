using Microsoft.Extensions.Configuration;
using UltimateERP.Domain.Entities.Security;
using UltimateERP.Infrastructure.Auth;

namespace UltimateERP.Tests.Auth;

public class JwtTokenServiceTests
{
    private readonly JwtTokenService _tokenService;

    public JwtTokenServiceTests()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "ThisIsATestKeyThatIsLongEnoughForHmacSha256!!",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience"
            })
            .Build();

        _tokenService = new JwtTokenService(config);
    }

    [Fact]
    public void GenerateAccessToken_ReturnsValidJwt()
    {
        var user = new User { Id = 1, Username = "admin", BranchId = 1, IsSystemAdmin = true };
        var token = _tokenService.GenerateAccessToken(user);

        Assert.NotNull(token);
        Assert.Contains('.', token); // JWT has 3 parts separated by dots
        Assert.Equal(3, token.Split('.').Length);
    }

    [Fact]
    public void GenerateRefreshToken_ReturnsBase64String()
    {
        var token = _tokenService.GenerateRefreshToken();
        Assert.NotNull(token);

        var bytes = Convert.FromBase64String(token);
        Assert.Equal(64, bytes.Length);
    }

    [Fact]
    public void GenerateRefreshToken_ProducesUniqueTokens()
    {
        var token1 = _tokenService.GenerateRefreshToken();
        var token2 = _tokenService.GenerateRefreshToken();
        Assert.NotEqual(token1, token2);
    }

    [Fact]
    public void ValidateRefreshToken_StoredToken_ReturnsUserId()
    {
        var refreshToken = _tokenService.GenerateRefreshToken();
        _tokenService.StoreRefreshToken(refreshToken, 42);

        var userId = _tokenService.ValidateRefreshToken(refreshToken);
        Assert.Equal(42, userId);
    }

    [Fact]
    public void ValidateRefreshToken_UnknownToken_ReturnsNull()
    {
        var userId = _tokenService.ValidateRefreshToken("unknown-token");
        Assert.Null(userId);
    }

    [Fact]
    public void RevokeRefreshToken_RemovesToken()
    {
        var refreshToken = _tokenService.GenerateRefreshToken();
        _tokenService.StoreRefreshToken(refreshToken, 1);

        _tokenService.RevokeRefreshToken(refreshToken);

        var userId = _tokenService.ValidateRefreshToken(refreshToken);
        Assert.Null(userId);
    }
}
