using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Security;

namespace UltimateERP.Infrastructure.Auth;

public class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly ConcurrentDictionary<string, (int UserId, DateTime Expiry)> _refreshTokens = new();

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "ERP-Default-Secret-Key-Replace-In-Production-2024!"));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("BranchId", user.BranchId?.ToString() ?? ""),
            new Claim("IsSystemAdmin", user.IsSystemAdmin.ToString())
        };

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "UltimateERP",
            audience: _configuration["Jwt:Audience"] ?? "UltimateERP",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public int? ValidateRefreshToken(string token)
    {
        if (_refreshTokens.TryGetValue(token, out var entry))
        {
            if (entry.Expiry > DateTime.UtcNow)
                return entry.UserId;

            _refreshTokens.TryRemove(token, out _);
        }
        return null;
    }

    public void RevokeRefreshToken(string token)
    {
        _refreshTokens.TryRemove(token, out _);
    }

    internal void StoreRefreshToken(string token, int userId)
    {
        // Clean expired tokens periodically
        foreach (var kvp in _refreshTokens.Where(x => x.Value.Expiry < DateTime.UtcNow).ToList())
            _refreshTokens.TryRemove(kvp.Key, out _);

        _refreshTokens[token] = (userId, DateTime.UtcNow.AddDays(7));
    }
}
