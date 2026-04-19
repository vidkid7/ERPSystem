using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Account.Queries;

public record GetFiscalYearsQuery : IRequest<ApiResponse<List<FiscalYearDto>>>;

public class GetFiscalYearsHandler : IRequestHandler<GetFiscalYearsQuery, ApiResponse<List<FiscalYearDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetFiscalYearsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<FiscalYearDto>>> Handle(GetFiscalYearsQuery request, CancellationToken ct)
    {
        var items = await _db.FiscalYears
            .OrderByDescending(f => f.StartDate)
            .ProjectTo<FiscalYearDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<FiscalYearDto>>.Success(items, "Fiscal years retrieved", items.Count);
    }
}

public record GetClosingTrialBalanceQuery(int FiscalYearId) : IRequest<ApiResponse<List<ClosingTrialBalanceDto>>>;

public class GetClosingTrialBalanceHandler : IRequestHandler<GetClosingTrialBalanceQuery, ApiResponse<List<ClosingTrialBalanceDto>>>
{
    private readonly IApplicationDbContext _db;

    public GetClosingTrialBalanceHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<ClosingTrialBalanceDto>>> Handle(GetClosingTrialBalanceQuery request, CancellationToken ct)
    {
        var fiscalYear = await _db.FiscalYears.FindAsync(new object[] { request.FiscalYearId }, ct);
        if (fiscalYear is null) return ApiResponse<List<ClosingTrialBalanceDto>>.Failure("Fiscal year not found");

        var ledgers = await _db.Ledgers
            .Include(l => l.LedgerGroup)
            .Where(l => l.IsActive)
            .ToListAsync(ct);

        var voucherDetails = await _db.VoucherDetails
            .Include(vd => vd.Voucher)
            .Where(vd => vd.Voucher.VoucherDate >= fiscalYear.StartDate && vd.Voucher.VoucherDate <= fiscalYear.EndDate && !vd.Voucher.IsCancelled)
            .ToListAsync(ct);

        var result = ledgers.Select(l =>
        {
            var details = voucherDetails.Where(vd => vd.LedgerId == l.Id);
            var txnDebit = details.Sum(d => d.DebitAmount);
            var txnCredit = details.Sum(d => d.CreditAmount);
            var closingBalance = l.OpeningBalance + txnDebit - txnCredit;

            return new ClosingTrialBalanceDto
            {
                LedgerId = l.Id,
                LedgerCode = l.Code,
                LedgerName = l.Name,
                GroupName = l.LedgerGroup?.Name,
                OpeningDebit = l.OpeningBalance > 0 ? l.OpeningBalance : 0,
                OpeningCredit = l.OpeningBalance < 0 ? Math.Abs(l.OpeningBalance) : 0,
                TransactionDebit = txnDebit,
                TransactionCredit = txnCredit,
                ClosingDebit = closingBalance > 0 ? closingBalance : 0,
                ClosingCredit = closingBalance < 0 ? Math.Abs(closingBalance) : 0
            };
        }).ToList();

        return ApiResponse<List<ClosingTrialBalanceDto>>.Success(result, "Closing trial balance retrieved", result.Count);
    }
}
