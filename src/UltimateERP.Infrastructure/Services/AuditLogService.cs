using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Security;
using UltimateERP.Infrastructure.Persistence;

namespace UltimateERP.Infrastructure.Services;

public class AuditLogService : IAuditLogService
{
    private readonly ERPDbContext _context;

    public AuditLogService(ERPDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(string action, string entityType, string entityId, string details, string userId)
    {
        var auditLog = new AuditLog
        {
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details,
            UserId = userId,
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    public async Task<List<AuditLogDto>> GetLogsAsync(AuditLogFilterDto filter)
    {
        var query = _context.AuditLogs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.EntityType))
            query = query.Where(a => a.EntityType == filter.EntityType);

        if (!string.IsNullOrWhiteSpace(filter.EntityId))
            query = query.Where(a => a.EntityId == filter.EntityId);

        if (!string.IsNullOrWhiteSpace(filter.UserId))
            query = query.Where(a => a.UserId == filter.UserId);

        if (!string.IsNullOrWhiteSpace(filter.Action))
            query = query.Where(a => a.Action == filter.Action);

        if (filter.From.HasValue)
            query = query.Where(a => a.Timestamp >= filter.From.Value);

        if (filter.To.HasValue)
            query = query.Where(a => a.Timestamp <= filter.To.Value);

        return await query
            .OrderByDescending(a => a.Timestamp)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(a => new AuditLogDto
            {
                Id = a.Id,
                Action = a.Action,
                EntityType = a.EntityType,
                EntityId = a.EntityId,
                OldValues = a.OldValues,
                NewValues = a.NewValues,
                UserId = a.UserId,
                Timestamp = a.Timestamp,
                IpAddress = a.IpAddress,
                Details = a.Details
            })
            .ToListAsync();
    }
}
