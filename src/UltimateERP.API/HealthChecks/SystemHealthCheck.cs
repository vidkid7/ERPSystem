using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UltimateERP.API.HealthChecks;

/// <summary>
/// Health check that reports system information: memory usage, uptime, and version.
/// </summary>
public class SystemHealthCheck : IHealthCheck
{
    private static readonly DateTime StartTime = DateTime.UtcNow;

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var process = Process.GetCurrentProcess();
        var memoryMb = process.WorkingSet64 / (1024.0 * 1024.0);
        var uptime = DateTime.UtcNow - StartTime;
        var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0";

        var data = new Dictionary<string, object>
        {
            ["MemoryUsageMB"] = Math.Round(memoryMb, 2),
            ["UptimeSeconds"] = Math.Round(uptime.TotalSeconds, 0),
            ["Uptime"] = uptime.ToString(@"dd\.hh\:mm\:ss"),
            ["Version"] = version,
            ["MachineName"] = Environment.MachineName,
            ["ProcessorCount"] = Environment.ProcessorCount,
            ["OSDescription"] = System.Runtime.InteropServices.RuntimeInformation.OSDescription
        };

        // Flag unhealthy if memory exceeds 2GB
        var isHealthy = memoryMb < 2048;

        return Task.FromResult(isHealthy
            ? HealthCheckResult.Healthy("System is healthy.", data)
            : HealthCheckResult.Degraded("High memory usage detected.", null, data));
    }
}
