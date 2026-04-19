using MediatR;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Auth.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Auth.Commands;

public record RefreshTokenCommand(string RefreshToken) : IRequest<ApiResponse<LoginResponse>>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ApiResponse<LoginResponse>>
{
    private readonly ITokenService _tokenService;
    private readonly IApplicationDbContext _context;

    public RefreshTokenCommandHandler(ITokenService tokenService, IApplicationDbContext context)
    {
        _tokenService = tokenService;
        _context = context;
    }

    public async Task<ApiResponse<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = _tokenService.ValidateRefreshToken(request.RefreshToken);
        if (userId is null)
            return ApiResponse<LoginResponse>.Failure("Invalid or expired refresh token");

        var user = await _context.Users.FindAsync(new object[] { userId.Value }, cancellationToken);
        if (user is null || !user.IsActive)
            return ApiResponse<LoginResponse>.Failure("User not found or inactive");

        _tokenService.RevokeRefreshToken(request.RefreshToken);

        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        return ApiResponse<LoginResponse>.Success(new LoginResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            UserId = user.Id,
            Username = user.Username,
            BranchId = user.BranchId
        }, "Token refreshed");
    }
}
