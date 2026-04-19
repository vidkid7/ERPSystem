using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace UltimateERP.Infrastructure.Auth;

public class PermissionRequirement : IAuthorizationRequirement
{
    public int EntityId { get; }
    public string Action { get; }

    public PermissionRequirement(int entityId, string action)
    {
        EntityId = entityId;
        Action = action;
    }
}

public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceProvider _serviceProvider;

    public PermissionRequirementHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return;

        using var scope = _serviceProvider.CreateScope();
        var permissionService = scope.ServiceProvider
            .GetRequiredService<Application.Interfaces.IPermissionService>();

        if (await permissionService.HasPermissionAsync(userId, requirement.EntityId, requirement.Action))
            context.Succeed(requirement);
    }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(string policy) : base(policy) { }
}
