using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Features.Reporting.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Reporting.Services;

public class LedgerBalanceService
{
    private readonly IApplicationDbContext _db;

    public LedgerBalanceService(IApplicationDbContext db) => _db = db;

    public async Task<List<LedgerBalanceDto>> GetLedgerBalances(DateTime? asOfDate, CancellationToken ct)
    {
        var date = asOfDate ?? DateTime.UtcNow;

        var balances = await _db.VoucherDetails
            .Include(vd => vd.Voucher)
            .Include(vd => vd.Ledger)
            .Where(vd => vd.Voucher.VoucherDate <= date && !vd.Voucher.IsCancelled)
            .GroupBy(vd => new { vd.LedgerId, vd.Ledger.Name, vd.Ledger.Code })
            .Select(g => new LedgerBalanceDto
            {
                LedgerId = g.Key.LedgerId,
                LedgerName = g.Key.Name,
                LedgerCode = g.Key.Code,
                DebitTotal = g.Sum(x => x.DebitAmount),
                CreditTotal = g.Sum(x => x.CreditAmount),
                Balance = g.Sum(x => x.DebitAmount) - g.Sum(x => x.CreditAmount)
            })
            .OrderBy(b => b.LedgerCode)
            .ToListAsync(ct);

        return balances;
    }

    public async Task<TrialBalanceDto> GetTrialBalance(DateTime? asOfDate, CancellationToken ct)
    {
        var balances = await GetLedgerBalances(asOfDate, ct);
        return new TrialBalanceDto
        {
            AsOfDate = asOfDate ?? DateTime.UtcNow,
            Ledgers = balances,
            TotalDebit = balances.Where(b => b.Balance > 0).Sum(b => b.Balance),
            TotalCredit = balances.Where(b => b.Balance < 0).Sum(b => Math.Abs(b.Balance))
        };
    }

    public async Task<List<DayBookEntryDto>> GetDayBook(DateTime date, CancellationToken ct)
    {
        var vouchers = await _db.Vouchers
            .Include(v => v.Details).ThenInclude(d => d.Ledger)
            .Where(v => v.VoucherDate.Date == date.Date && !v.IsCancelled)
            .OrderBy(v => v.VoucherNumber)
            .ToListAsync(ct);

        return vouchers.Select(v => new DayBookEntryDto
        {
            VoucherId = v.Id,
            VoucherNumber = v.VoucherNumber,
            VoucherDate = v.VoucherDate,
            Narration = v.CommonNarration,
            Details = v.Details.Select(d => new DayBookDetailDto
            {
                LedgerName = d.Ledger?.Name,
                DebitAmount = d.DebitAmount,
                CreditAmount = d.CreditAmount
            }).ToList()
        }).ToList();
    }
}
