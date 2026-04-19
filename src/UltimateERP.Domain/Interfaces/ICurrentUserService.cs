namespace UltimateERP.Domain.Interfaces;

public interface ICurrentUserService
{
    int? UserId { get; }
    string? UserName { get; }
    int? BranchId { get; }
    bool IsAuthenticated { get; }
}
