using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Reporting.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Reporting;

namespace UltimateERP.Application.Features.Reporting.Commands;

// ── Create Report Definition ──────────────────────────────────────────
public record CreateReportDefinitionCommand(CreateReportDefinitionDto Dto)
    : IRequest<ApiResponse<ReportDefinitionDto>>;

public class CreateReportDefinitionHandler
    : IRequestHandler<CreateReportDefinitionCommand, ApiResponse<ReportDefinitionDto>>
{
    private readonly IApplicationDbContext _db;
    public CreateReportDefinitionHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<ReportDefinitionDto>> Handle(CreateReportDefinitionCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var entity = new ReportWriterDefinition
        {
            ReportName = dto.Name,
            ReportTitle = dto.Name,
            EntityQuery = dto.EntityName,
            ReportLayout = JsonSerializer.Serialize(new
            {
                Columns = dto.Columns,
                Filters = dto.Filters,
                GroupBy = dto.GroupBy,
                SortBy = dto.SortBy
            }),
            IsSharedWithAll = dto.IsShared,
            IsActive = true
        };

        _db.ReportWriterDefinitions.Add(entity);
        await _db.SaveChangesAsync(ct);

        var result = MapToDto(entity, dto.Columns);
        return ApiResponse<ReportDefinitionDto>.Success(result, "Report definition created");
    }

    private static ReportDefinitionDto MapToDto(ReportWriterDefinition entity, List<ReportColumnDto>? columns = null)
    {
        var cols = columns ?? DeserializeColumns(entity.ReportLayout);
        var layout = DeserializeLayout(entity.ReportLayout);

        return new ReportDefinitionDto
        {
            Id = entity.Id,
            Name = entity.ReportName,
            EntityName = entity.EntityQuery ?? string.Empty,
            Columns = cols,
            Filters = layout.Filters,
            GroupBy = layout.GroupBy,
            SortBy = layout.SortBy,
            IsShared = entity.IsSharedWithAll,
            CreatedById = entity.CreatedById
        };
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

// ── Update Report Definition ──────────────────────────────────────────
public record UpdateReportDefinitionCommand(UpdateReportDefinitionDto Dto)
    : IRequest<ApiResponse<ReportDefinitionDto>>;

public class UpdateReportDefinitionHandler
    : IRequestHandler<UpdateReportDefinitionCommand, ApiResponse<ReportDefinitionDto>>
{
    private readonly IApplicationDbContext _db;
    public UpdateReportDefinitionHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<ReportDefinitionDto>> Handle(UpdateReportDefinitionCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var entity = await _db.ReportWriterDefinitions.FindAsync(new object[] { dto.Id }, ct);
        if (entity is null) return ApiResponse<ReportDefinitionDto>.Failure("Report definition not found");

        entity.ReportName = dto.Name;
        entity.ReportTitle = dto.Name;
        entity.EntityQuery = dto.EntityName;
        entity.ReportLayout = JsonSerializer.Serialize(new
        {
            Columns = dto.Columns,
            Filters = dto.Filters,
            GroupBy = dto.GroupBy,
            SortBy = dto.SortBy
        });
        entity.IsSharedWithAll = dto.IsShared;
        entity.ModifiedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        var result = new ReportDefinitionDto
        {
            Id = entity.Id,
            Name = entity.ReportName,
            EntityName = entity.EntityQuery ?? string.Empty,
            Columns = dto.Columns,
            Filters = dto.Filters,
            GroupBy = dto.GroupBy,
            SortBy = dto.SortBy,
            IsShared = entity.IsSharedWithAll,
            CreatedById = entity.CreatedById
        };
        return ApiResponse<ReportDefinitionDto>.Success(result, "Report definition updated");
    }
}

// ── Delete Report Definition ──────────────────────────────────────────
public record DeleteReportDefinitionCommand(int Id) : IRequest<ApiResponse<bool>>;

public class DeleteReportDefinitionHandler
    : IRequestHandler<DeleteReportDefinitionCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _db;
    public DeleteReportDefinitionHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<bool>> Handle(DeleteReportDefinitionCommand request, CancellationToken ct)
    {
        var entity = await _db.ReportWriterDefinitions.FindAsync(new object[] { request.Id }, ct);
        if (entity is null) return ApiResponse<bool>.Failure("Report definition not found");

        entity.IsDeleted = true;
        entity.IsActive = false;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<bool>.Success(true, "Report definition deleted");
    }
}
