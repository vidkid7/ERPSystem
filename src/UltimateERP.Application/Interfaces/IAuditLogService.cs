namespace UltimateERP.Application.Interfaces;

public interface IAuditLogService
{
    Task LogAsync(string action, string entityType, string entityId, string details, string userId);
    Task<List<AuditLogDto>> GetLogsAsync(AuditLogFilterDto filter);
}

public class AuditLogDto
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? IpAddress { get; set; }
    public string? Details { get; set; }
}

public class AuditLogFilterDto
{
    public string? EntityType { get; set; }
    public string? EntityId { get; set; }
    public string? UserId { get; set; }
    public string? Action { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
