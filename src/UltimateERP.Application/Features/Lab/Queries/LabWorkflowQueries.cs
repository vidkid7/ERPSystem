using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Lab.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Lab.Queries;

// Get Pending Lab Reports (reports not yet completed/validated)
public record GetPendingLabReportsQuery(int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<LabReportDto>>>;

public class GetPendingLabReportsHandler : IRequestHandler<GetPendingLabReportsQuery, ApiResponse<List<LabReportDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetPendingLabReportsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<LabReportDto>>> Handle(GetPendingLabReportsQuery request, CancellationToken ct)
    {
        var query = _db.LabReports
            .Include(r => r.SampleCollection)
            .Where(r => r.Status == LabReportStatus.Pending && !r.IsDeleted);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(r => r.ReportDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<LabReportDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<LabReportDto>>.Success(items, "Pending lab reports retrieved", total);
    }
}
