using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Services;

/// <summary>
/// Service for managing lab sample and report workflow transitions.
/// </summary>
public class LabWorkflowService
{
    private readonly IApplicationDbContext _db;

    public LabWorkflowService(IApplicationDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Transitions a sample from Collected → InProcess.
    /// </summary>
    public async Task<(bool Success, string Message)> ProcessSample(int sampleId, CancellationToken ct = default)
    {
        var sample = await _db.SampleCollections.FindAsync(new object[] { sampleId }, ct);
        if (sample is null)
            return (false, "Sample not found.");

        if (sample.Status != SampleCollectionStatus.Collected)
            return (false, $"Sample must be in Collected status to process. Current status: {sample.Status}.");

        sample.Status = SampleCollectionStatus.InProcess;
        await _db.SaveChangesAsync(ct);
        return (true, "Sample moved to InProcess.");
    }

    /// <summary>
    /// Marks a lab report as Completed with the provided results.
    /// </summary>
    public async Task<(bool Success, string Message)> CompleteReport(int reportId, string results, CancellationToken ct = default)
    {
        var report = await _db.LabReports
            .Include(r => r.SampleCollection)
            .FirstOrDefaultAsync(r => r.Id == reportId, ct);

        if (report is null)
            return (false, "Lab report not found.");

        if (report.Status != LabReportStatus.Pending)
            return (false, $"Report must be in Pending status to complete. Current status: {report.Status}.");

        report.Status = LabReportStatus.Completed;
        report.ReportData = results;
        await _db.SaveChangesAsync(ct);
        return (true, "Report marked as Completed.");
    }

    /// <summary>
    /// Validates a completed lab report.
    /// </summary>
    public async Task<(bool Success, string Message)> ValidateReport(int reportId, int validatedBy, CancellationToken ct = default)
    {
        var report = await _db.LabReports.FindAsync(new object[] { reportId }, ct);
        if (report is null)
            return (false, "Lab report not found.");

        if (report.Status != LabReportStatus.Completed)
            return (false, $"Report must be in Completed status to validate. Current status: {report.Status}.");

        report.Status = LabReportStatus.Validated;
        report.ValidatedBy = validatedBy;
        await _db.SaveChangesAsync(ct);
        return (true, "Report validated successfully.");
    }
}
