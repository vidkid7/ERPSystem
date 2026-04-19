using Microsoft.Extensions.Diagnostics.HealthChecks;
using UltimateERP.Infrastructure.Persistence;

namespace UltimateERP.API.HealthChecks;

/// <summary>
/// Health check that verifies database connectivity.
/// </summary>
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly ERPDbContext _context;

    public DatabaseHealthCheck(ERPDbContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
            return canConnect
                ? HealthCheckResult.Healthy("Database connection is healthy.")
                : HealthCheckResult.Unhealthy("Cannot connect to database.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database check failed.", ex);
        }
    }
}
