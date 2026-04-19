using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Features.Auth.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Infrastructure.Persistence;

namespace UltimateERP.Infrastructure.Auth;

public class PermissionService : IPermissionService
{
    private readonly ERPDbContext _context;

    public PermissionService(ERPDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasPermissionAsync(int userId, int entityId, string action)
    {
        // System admins have all permissions
        var user = await _context.Users.FindAsync(userId);
        if (user?.IsSystemAdmin == true) return true;

        var userGroupIds = await GetUserGroupIds(userId);

        var permission = await _context.EntityPermissions
            .FirstOrDefaultAsync(p =>
                p.EntityId == entityId &&
                (p.UserId == userId || (p.UserGroupId != null && userGroupIds.Contains(p.UserGroupId.Value))));

        if (permission is null) return false;

        return action.ToLower() switch
        {
            "view" => permission.CanView,
            "create" => permission.CanCreate,
            "update" => permission.CanUpdate,
            "delete" => permission.CanDelete,
            "report" => permission.CanReport,
            _ => false
        };
    }

    public async Task<bool> HasModuleAccessAsync(int userId, int moduleId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user?.IsSystemAdmin == true) return true;

        var userGroupIds = await GetUserGroupIds(userId);

        return await _context.ModuleAccesses.AnyAsync(m =>
            m.ModuleId == moduleId &&
            (m.UserId == userId || (m.UserGroupId != null && userGroupIds.Contains(m.UserGroupId.Value))) &&
            m.IsActive);
    }

    public async Task<bool> HasBranchAccessAsync(int userId, int branchId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user?.IsSystemAdmin == true) return true;

        return await _context.BranchAccesses.AnyAsync(b =>
            b.UserId == userId && b.BranchId == branchId && b.IsActive);
    }

    public async Task<bool> HasGodownAccessAsync(int userId, int godownId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user?.IsSystemAdmin == true) return true;

        return await _context.GodownAccesses.AnyAsync(g =>
            g.UserId == userId && g.GodownId == godownId && g.IsActive);
    }

    public async Task<IReadOnlyList<UserPermissionDto>> GetUserPermissionsAsync(int userId)
    {
        var userGroupIds = await GetUserGroupIds(userId);

        var permissions = await _context.EntityPermissions
            .Where(p => p.UserId == userId || (p.UserGroupId != null && userGroupIds.Contains(p.UserGroupId.Value)))
            .Select(p => new UserPermissionDto
            {
                EntityId = p.EntityId,
                CanView = p.CanView,
                CanCreate = p.CanCreate,
                CanUpdate = p.CanUpdate,
                CanDelete = p.CanDelete,
                CanReport = p.CanReport
            })
            .ToListAsync();

        return permissions;
    }

    public async Task<IReadOnlyList<int>> GetAccessibleBranchIdsAsync(int userId)
    {
        return await _context.BranchAccesses
            .Where(b => b.UserId == userId && b.IsActive)
            .Select(b => b.BranchId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<int>> GetAccessibleGodownIdsAsync(int userId)
    {
        return await _context.GodownAccesses
            .Where(g => g.UserId == userId && g.IsActive)
            .Select(g => g.GodownId)
            .ToListAsync();
    }

    private async Task<List<int>> GetUserGroupIds(int userId)
    {
        return await _context.UserGroupMembers
            .Where(m => m.UserId == userId && m.IsActive)
            .Select(m => m.UserGroupId)
            .ToListAsync();
    }
}
