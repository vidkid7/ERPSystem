using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Finance.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Finance.Queries;

// Get Loans
public record GetLoansQuery(string? Search, string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<LoanDto>>>;

public class GetLoansHandler : IRequestHandler<GetLoansQuery, ApiResponse<List<LoanDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetLoansHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<LoanDto>>> Handle(GetLoansQuery request, CancellationToken ct)
    {
        var query = _db.Loans.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.ToLower();
            query = query.Where(l => l.LoanNumber.ToLower().Contains(s)
                                  || (l.BorrowerName != null && l.BorrowerName.ToLower().Contains(s)));
        }
        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<Domain.Enums.LoanStatus>(request.Status, out var status))
            query = query.Where(l => l.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(l => l.LoanDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<LoanDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<LoanDto>>.Success(items, "Loans retrieved", total);
    }
}

// Get EMI Schedule
public record GetEMIScheduleQuery(int LoanId) : IRequest<ApiResponse<List<LoanEMIDto>>>;

public class GetEMIScheduleHandler : IRequestHandler<GetEMIScheduleQuery, ApiResponse<List<LoanEMIDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetEMIScheduleHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<LoanEMIDto>>> Handle(GetEMIScheduleQuery request, CancellationToken ct)
    {
        var items = await _db.LoanEMIs
            .Include(e => e.Loan)
            .Where(e => e.LoanId == request.LoanId)
            .OrderBy(e => e.EMINumber)
            .ProjectTo<LoanEMIDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<LoanEMIDto>>.Success(items, "EMI schedule retrieved", items.Count);
    }
}

// Get Overdue EMIs
public record GetOverdueEMIsQuery(int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<LoanEMIDto>>>;

public class GetOverdueEMIsHandler : IRequestHandler<GetOverdueEMIsQuery, ApiResponse<List<LoanEMIDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetOverdueEMIsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<LoanEMIDto>>> Handle(GetOverdueEMIsQuery request, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var query = _db.LoanEMIs.Include(e => e.Loan)
            .Where(e => e.Status == Domain.Enums.EMIStatus.Overdue
                     || (e.Status == Domain.Enums.EMIStatus.Pending && e.EMIDueDate < now));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(e => e.EMIDueDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<LoanEMIDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<LoanEMIDto>>.Success(items, "Overdue EMIs retrieved", total);
    }
}

// Get Loan Summary
public record GetLoanSummaryQuery() : IRequest<ApiResponse<LoanSummaryDto>>;

public class GetLoanSummaryHandler : IRequestHandler<GetLoanSummaryQuery, ApiResponse<LoanSummaryDto>>
{
    private readonly IApplicationDbContext _db;
    public GetLoanSummaryHandler(IApplicationDbContext db) { _db = db; }

    public async Task<ApiResponse<LoanSummaryDto>> Handle(GetLoanSummaryQuery request, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var totalLoans = await _db.Loans.CountAsync(ct);
        var activeLoans = await _db.Loans.CountAsync(l => l.Status == Domain.Enums.LoanStatus.Active, ct);
        var totalDisbursed = await _db.Loans.SumAsync(l => l.PrincipalAmount, ct);
        var totalCollected = await _db.LoanEMIs
            .Where(e => e.Status == Domain.Enums.EMIStatus.Paid)
            .SumAsync(e => e.PaidAmount, ct);
        var overdueCount = await _db.LoanEMIs
            .CountAsync(e => e.Status == Domain.Enums.EMIStatus.Overdue
                          || (e.Status == Domain.Enums.EMIStatus.Pending && e.EMIDueDate < now), ct);

        var summary = new LoanSummaryDto
        {
            TotalDisbursed = totalDisbursed,
            TotalCollected = totalCollected,
            OutstandingBalance = totalDisbursed - totalCollected,
            OverdueCount = overdueCount,
            TotalLoans = totalLoans,
            ActiveLoans = activeLoans
        };

        return ApiResponse<LoanSummaryDto>.Success(summary, "Loan summary retrieved");
    }
}
