using UltimateERP.Domain.Entities.Security;

namespace UltimateERP.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    int? ValidateRefreshToken(string token);
    void RevokeRefreshToken(string token);
}
