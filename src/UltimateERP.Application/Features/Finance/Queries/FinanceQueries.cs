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
