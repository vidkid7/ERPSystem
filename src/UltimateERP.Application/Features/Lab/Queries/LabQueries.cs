using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Lab.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Lab.Queries;

// Get Samples
public record GetSamplesQuery(string? Search, string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<SampleCollectionDto>>>;

public class GetSamplesHandler : IRequestHandler<GetSamplesQuery, ApiResponse<List<SampleCollectionDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetSamplesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<SampleCollectionDto>>> Handle(GetSamplesQuery request, CancellationToken ct)
    {
        var query = _db.SampleCollections.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.ToLower();
            query = query.Where(sc => sc.SampleNumber.ToLower().Contains(s)
                                   || (sc.PatientName != null && sc.PatientName.ToLower().Contains(s)));
        }
        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<Domain.Enums.SampleCollectionStatus>(request.Status, out var status))
            query = query.Where(sc => sc.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(sc => sc.CollectionDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<SampleCollectionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<SampleCollectionDto>>.Success(items, "Samples retrieved", total);
    }
}

// Get Lab Reports
public record GetLabReportsQuery(int? SampleCollectionId, string? Search, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<LabReportDto>>>;

public class GetLabReportsHandler : IRequestHandler<GetLabReportsQuery, ApiResponse<List<LabReportDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetLabReportsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<LabReportDto>>> Handle(GetLabReportsQuery request, CancellationToken ct)
    {
        var query = _db.LabReports
            .Include(r => r.SampleCollection)
            .AsQueryable();

        if (request.SampleCollectionId.HasValue)
            query = query.Where(r => r.SampleCollectionId == request.SampleCollectionId);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.ToLower();
            query = query.Where(r => r.SampleCollection.SampleNumber.ToLower().Contains(s)
                                  || (r.SampleCollection.PatientName != null && r.SampleCollection.PatientName.ToLower().Contains(s)));
        }

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(r => r.ReportDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<LabReportDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<LabReportDto>>.Success(items, "Lab reports retrieved", total);
    }
}
