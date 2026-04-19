using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Reporting.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Reporting.Queries;

// ── Get Report Definitions (user's + shared) ──────────────────────────
public record GetReportDefinitionsQuery(int? UserId) : IRequest<ApiResponse<List<ReportDefinitionDto>>>;

public class GetReportDefinitionsHandler
    : IRequestHandler<GetReportDefinitionsQuery, ApiResponse<List<ReportDefinitionDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetReportDefinitionsHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<ReportDefinitionDto>>> Handle(GetReportDefinitionsQuery request, CancellationToken ct)
    {
        var query = _db.ReportWriterDefinitions
            .Where(r => !r.IsDeleted);

        if (request.UserId.HasValue)
            query = query.Where(r => r.IsSharedWithAll || r.CreatedById == request.UserId);

        var entities = await query.OrderBy(r => r.ReportName).ToListAsync(ct);

        var result = entities.Select(e =>
        {
            var columns = DeserializeColumns(e.ReportLayout);
            var layout = DeserializeLayout(e.ReportLayout);

            return new ReportDefinitionDto
            {
                Id = e.Id,
                Name = e.ReportName,
                EntityName = e.EntityQuery ?? string.Empty,
                Columns = columns,
                Filters = layout.Filters,
                GroupBy = layout.GroupBy,
                SortBy = layout.SortBy,
                IsShared = e.IsSharedWithAll,
                CreatedById = e.CreatedById
            };
        }).ToList();

        return ApiResponse<List<ReportDefinitionDto>>.Success(result, "Report definitions retrieved", result.Count);
    }

    private static List<ReportColumnDto> DeserializeColumns(string? layout)
    {
        if (string.IsNullOrEmpty(layout)) return new();
        try
        {
            var doc = JsonDocument.Parse(layout);
            if (doc.RootElement.TryGetProperty("Columns", out var colElement))
                return JsonSerializer.Deserialize<List<ReportColumnDto>>(colElement.GetRawText()) ?? new();
        }
        catch { }
        return new();
    }

    private static (string? Filters, string? GroupBy, string? SortBy) DeserializeLayout(string? layout)
    {
        if (string.IsNullOrEmpty(layout)) return (null, null, null);
        try
        {
            var doc = JsonDocument.Parse(layout);
            var filters = doc.RootElement.TryGetProperty("Filters", out var f) ? f.GetString() : null;
            var groupBy = doc.RootElement.TryGetProperty("GroupBy", out var g) ? g.GetString() : null;
            var sortBy = doc.RootElement.TryGetProperty("SortBy", out var s) ? s.GetString() : null;
            return (filters, groupBy, sortBy);
        }
        catch { return (null, null, null); }
    }
}

// ── Execute Custom Report ─────────────────────────────────────────────
public record ExecuteCustomReportQuery(int ReportDefinitionId) : IRequest<ApiResponse<CustomReportResultDto>>;

public class ExecuteCustomReportHandler
    : IRequestHandler<ExecuteCustomReportQuery, ApiResponse<CustomReportResultDto>>
{
    private readonly IApplicationDbContext _db;
    public ExecuteCustomReportHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<CustomReportResultDto>> Handle(ExecuteCustomReportQuery request, CancellationToken ct)
    {
        var definition = await _db.ReportWriterDefinitions
            .FirstOrDefaultAsync(r => r.Id == request.ReportDefinitionId && !r.IsDeleted, ct);

        if (definition is null)
            return ApiResponse<CustomReportResultDto>.Failure("Report definition not found");

        var columns = DeserializeColumns(definition.ReportLayout);
        var headers = columns.Where(c => c.IsVisible).OrderBy(c => c.SortOrder)
            .Select(c => c.DisplayName).ToList();

        // Return the report definition metadata; actual data execution is entity-specific
        var result = new CustomReportResultDto
        {
            ReportName = definition.ReportName,
            ColumnHeaders = headers,
            Rows = new(),
            TotalRows = 0
        };

        return ApiResponse<CustomReportResultDto>.Success(result, "Custom report executed");
    }

    private static List<ReportColumnDto> DeserializeColumns(string? layout)
    {
        if (string.IsNullOrEmpty(layout)) return new();
        try
        {
            var doc = JsonDocument.Parse(layout);
            if (doc.RootElement.TryGetProperty("Columns", out var colElement))
                return JsonSerializer.Deserialize<List<ReportColumnDto>>(colElement.GetRawText()) ?? new();
        }
        catch { }
        return new();
    }
}
