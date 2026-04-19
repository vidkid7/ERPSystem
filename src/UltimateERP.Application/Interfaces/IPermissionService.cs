using UltimateERP.Application.Features.Auth.DTOs;

namespace UltimateERP.Application.Interfaces;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(int userId, int entityId, string action);
    Task<bool> HasModuleAccessAsync(int userId, int moduleId);
    Task<bool> HasBranchAccessAsync(int userId, int branchId);
    Task<bool> HasGodownAccessAsync(int userId, int godownId);
    Task<IReadOnlyList<UserPermissionDto>> GetUserPermissionsAsync(int userId);
    Task<IReadOnlyList<int>> GetAccessibleBranchIdsAsync(int userId);
    Task<IReadOnlyList<int>> GetAccessibleGodownIdsAsync(int userId);
}
