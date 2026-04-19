using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Account.Commands;

public record PerformYearEndClosingCommand(int FiscalYearId) : IRequest<ApiResponse<YearEndClosingResultDto>>;

public class PerformYearEndClosingHandler : IRequestHandler<PerformYearEndClosingCommand, ApiResponse<YearEndClosingResultDto>>
{
    private readonly IApplicationDbContext _db;

    public PerformYearEndClosingHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<YearEndClosingResultDto>> Handle(PerformYearEndClosingCommand request, CancellationToken ct)
    {
        var fiscalYear = await _db.FiscalYears.FindAsync(new object[] { request.FiscalYearId }, ct);
        if (fiscalYear is null) return ApiResponse<YearEndClosingResultDto>.Failure("Fiscal year not found");
        if (fiscalYear.Status == FiscalYearStatus.Locked) return ApiResponse<YearEndClosingResultDto>.Failure("Fiscal year is already locked");

        // Calculate closing balances for all ledgers
        var ledgers = await _db.Ledgers.Where(l => l.IsActive).ToListAsync(ct);
        var voucherDetails = await _db.VoucherDetails
            .Include(vd => vd.Voucher)
            .Where(vd => vd.Voucher.VoucherDate >= fiscalYear.StartDate && vd.Voucher.VoucherDate <= fiscalYear.EndDate && !vd.Voucher.IsCancelled)
            .ToListAsync(ct);

        foreach (var ledger in ledgers)
        {
            var details = voucherDetails.Where(vd => vd.LedgerId == ledger.Id);
            ledger.DebitAmount = details.Sum(d => d.DebitAmount);
            ledger.CreditAmount = details.Sum(d => d.CreditAmount);
            ledger.ClosingBalance = ledger.OpeningBalance + ledger.DebitAmount - ledger.CreditAmount;
        }

        // Create opening voucher for next year
        var openingVoucher = new Voucher
        {
            VoucherNumber = "OB-" + (fiscalYear.EndDate.Year + 1),
            VoucherDate = fiscalYear.EndDate.AddDays(1),
            CommonNarration = $"Opening balances from FY {fiscalYear.Name}",
            TotalDebit = 0,
            TotalCredit = 0
        };

        int lineNum = 1;
        int openingVouchersCreated = 0;
        foreach (var ledger in ledgers.Where(l => l.ClosingBalance != 0))
        {
            openingVoucher.Details.Add(new VoucherDetail
            {
                LineNumber = lineNum++,
                LedgerId = ledger.Id,
                DebitAmount = ledger.ClosingBalance > 0 ? ledger.ClosingBalance : 0,
                CreditAmount = ledger.ClosingBalance < 0 ? Math.Abs(ledger.ClosingBalance) : 0,
                Narration = $"Opening balance from FY {fiscalYear.Name}"
            });
            openingVouchersCreated++;
        }

        openingVoucher.TotalDebit = openingVoucher.Details.Sum(d => d.DebitAmount);
        openingVoucher.TotalCredit = openingVoucher.Details.Sum(d => d.CreditAmount);

        if (openingVoucher.Details.Any())
            _db.Vouchers.Add(openingVoucher);

        fiscalYear.Status = FiscalYearStatus.Closed;
        fiscalYear.ClosedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        return ApiResponse<YearEndClosingResultDto>.Success(new YearEndClosingResultDto
        {
            IsSuccess = true,
            FiscalYearId = request.FiscalYearId,
            OpeningVouchersCreated = openingVouchersCreated,
            Message = $"Year-end closing completed. {openingVouchersCreated} opening balance entries created."
        }, "Year-end closing completed");
    }
}

public record LockFiscalYearCommand(int FiscalYearId) : IRequest<ApiResponse<bool>>;

public class LockFiscalYearHandler : IRequestHandler<LockFiscalYearCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _db;

    public LockFiscalYearHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<bool>> Handle(LockFiscalYearCommand request, CancellationToken ct)
    {
        var fiscalYear = await _db.FiscalYears.FindAsync(new object[] { request.FiscalYearId }, ct);
        if (fiscalYear is null) return ApiResponse<bool>.Failure("Fiscal year not found");
        if (fiscalYear.Status == FiscalYearStatus.Open) return ApiResponse<bool>.Failure("Fiscal year must be closed before locking");

        fiscalYear.Status = FiscalYearStatus.Locked;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<bool>.Success(true, "Fiscal year locked");
    }
}
