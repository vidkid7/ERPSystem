using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.HMS.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.HMS.Queries;

// Patients
public record GetPatientsQuery(string? Search, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<PatientDto>>>;

public class GetPatientsHandler : IRequestHandler<GetPatientsQuery, ApiResponse<List<PatientDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetPatientsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<PatientDto>>> Handle(GetPatientsQuery request, CancellationToken ct)
    {
        var query = _db.Patients.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.ToLower();
            query = query.Where(p => (p.FirstName + " " + p.LastName).ToLower().Contains(s)
                                  || p.PatientNumber.ToLower().Contains(s));
        }

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(p => p.RegistrationDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<PatientDto>>.Success(items, "Patients retrieved", total);
    }
}

// Beds
public record GetBedsQuery(string? Status) : IRequest<ApiResponse<List<BedDto>>>;

public class GetBedsHandler : IRequestHandler<GetBedsQuery, ApiResponse<List<BedDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetBedsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<BedDto>>> Handle(GetBedsQuery request, CancellationToken ct)
    {
        var query = _db.Beds.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<Domain.Enums.BedStatus>(request.Status, out var status))
            query = query.Where(b => b.Status == status);

        var items = await query.OrderBy(b => b.BedNumber)
            .ProjectTo<BedDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<BedDto>>.Success(items, "Beds retrieved", items.Count);
    }
}

// OPD Tickets
public record GetOPDTicketsQuery(int? PatientId, DateTime? FromDate, DateTime? ToDate, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<OPDTicketDto>>>;

public class GetOPDTicketsHandler : IRequestHandler<GetOPDTicketsQuery, ApiResponse<List<OPDTicketDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetOPDTicketsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<OPDTicketDto>>> Handle(GetOPDTicketsQuery request, CancellationToken ct)
    {
        var query = _db.OPDTickets.Include(t => t.Patient).AsQueryable();
        if (request.PatientId.HasValue) query = query.Where(t => t.PatientId == request.PatientId);
        if (request.FromDate.HasValue) query = query.Where(t => t.TicketDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(t => t.TicketDate <= request.ToDate);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(t => t.TicketDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<OPDTicketDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<OPDTicketDto>>.Success(items, "OPD tickets retrieved", total);
    }
}

// IPD Admissions
public record GetIPDAdmissionsQuery(int? PatientId, string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<IPDAdmissionDto>>>;

public class GetIPDAdmissionsHandler : IRequestHandler<GetIPDAdmissionsQuery, ApiResponse<List<IPDAdmissionDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetIPDAdmissionsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<IPDAdmissionDto>>> Handle(GetIPDAdmissionsQuery request, CancellationToken ct)
    {
        var query = _db.IPDAdmissions.Include(a => a.Patient).Include(a => a.Bed).AsQueryable();
        if (request.PatientId.HasValue) query = query.Where(a => a.PatientId == request.PatientId);
        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<Domain.Enums.IPDStatus>(request.Status, out var status))
            query = query.Where(a => a.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(a => a.AdmissionDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<IPDAdmissionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<IPDAdmissionDto>>.Success(items, "IPD admissions retrieved", total);
    }
}
