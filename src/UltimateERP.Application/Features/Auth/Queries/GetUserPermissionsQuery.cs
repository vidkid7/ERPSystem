using MediatR;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Auth.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Interfaces;

namespace UltimateERP.Application.Features.Auth.Queries;

public record GetUserPermissionsQuery : IRequest<ApiResponse<IReadOnlyList<UserPermissionDto>>>;

public class GetUserPermissionsQueryHandler
    : IRequestHandler<GetUserPermissionsQuery, ApiResponse<IReadOnlyList<UserPermissionDto>>>
{
    private readonly IPermissionService _permissionService;
    private readonly ICurrentUserService _currentUser;

    public GetUserPermissionsQueryHandler(IPermissionService permissionService, ICurrentUserService currentUser)
    {
        _permissionService = permissionService;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<IReadOnlyList<UserPermissionDto>>> Handle(
        GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is null or 0)
            return ApiResponse<IReadOnlyList<UserPermissionDto>>.Failure("Unauthorized");

        var permissions = await _permissionService.GetUserPermissionsAsync(_currentUser.UserId.Value);
        return ApiResponse<IReadOnlyList<UserPermissionDto>>.Success(permissions, "Permissions retrieved", permissions.Count);
    }
}
