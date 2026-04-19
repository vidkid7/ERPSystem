using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Features.Reporting.Services;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Infrastructure.Persistence;

namespace UltimateERP.Tests.Reporting;

/// <summary>
/// Integration tests for LedgerBalanceService — cross-module reporting across Account vouchers.
/// Uses EF Core InMemory database.
/// </summary>
public class LedgerBalanceServiceTests : IDisposable
{
    private readonly ERPDbContext _db;
    private readonly LedgerBalanceService _service;

    public LedgerBalanceServiceTests()
    {
        var opts = new DbContextOptionsBuilder<ERPDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new ERPDbContext(opts);
        _service = new LedgerBalanceService(_db);
    }

    public void Dispose() => _db.Dispose();

    // ── Seed helpers ──────────────────────────────────────────────────────

    private async Task<(Ledger debitLedger, Ledger creditLedger)> SeedLedgersAsync()
    {
        var group = new LedgerGroup { Id = 1, Name = "General" };
        var debit  = new Ledger { Id = 1, Name = "Cash",        Code = "CASH", LedgerGroupId = 1, LedgerGroup = group };
        var credit = new Ledger { Id = 2, Name = "Sales",       Code = "SAL",  LedgerGroupId = 1, LedgerGroup = group };

        _db.LedgerGroups.Add(group);
        _db.Ledgers.AddRange(debit, credit);
        await _db.SaveChangesAsync();
        return (debit, credit);
    }

    private async Task SeedVoucherAsync(int id, string number, DateTime date, int debitLedgerId, int creditLedgerId, decimal amount, bool cancelled = false)
    {
        var debitLedger  = await _db.Ledgers.FindAsync(debitLedgerId);
        var creditLedger = await _db.Ledgers.FindAsync(creditLedgerId);

        var voucher = new Voucher
        {
            Id = id,
            VoucherTypeId = 1,
            VoucherNumber = number,
            VoucherDate = date,
            TotalDebit = amount,
            TotalCredit = amount,
            IsCancelled = cancelled,
        };
        voucher.Details.Add(new VoucherDetail
        {
            Id = id * 10 + 1,
            LineNumber = 1,
            LedgerId = debitLedgerId,
            Ledger = debitLedger!,
            DebitAmount = amount,
            CreditAmount = 0
        });
        voucher.Details.Add(new VoucherDetail
        {
            Id = id * 10 + 2,
            LineNumber = 2,
            LedgerId = creditLedgerId,
            Ledger = creditLedger!,
            DebitAmount = 0,
            CreditAmount = amount
        });

        _db.Vouchers.Add(voucher);
        await _db.SaveChangesAsync();
    }

    // ── GetLedgerBalances Tests ──────────────────────────────────────────

    [Fact]
    public async Task GetLedgerBalances_WithPostedVouchers_ReturnsCorrectBalances()
    {
        var (debit, credit) = await SeedLedgersAsync();
        await SeedVoucherAsync(1, "JV-001", DateTime.Today, debit.Id, credit.Id, 10000m);

        var balances = await _service.GetLedgerBalances(DateTime.Today, CancellationToken.None);

        Assert.Equal(2, balances.Count);

        var cashBalance  = balances.Single(b => b.LedgerId == debit.Id);
        var salesBalance = balances.Single(b => b.LedgerId == credit.Id);

        Assert.Equal(10000m,  cashBalance.DebitTotal);
        Assert.Equal(0m,      cashBalance.CreditTotal);
        Assert.Equal(10000m,  cashBalance.Balance); // Debit - Credit

        Assert.Equal(0m,      salesBalance.DebitTotal);
        Assert.Equal(10000m,  salesBalance.CreditTotal);
        Assert.Equal(-10000m, salesBalance.Balance);
    }

    [Fact]
    public async Task GetLedgerBalances_MultipleVouchers_AggregatesCorrectly()
    {
        var (debit, credit) = await SeedLedgersAsync();
        await SeedVoucherAsync(1, "JV-001", DateTime.Today, debit.Id, credit.Id, 5000m);
        await SeedVoucherAsync(2, "JV-002", DateTime.Today, debit.Id, credit.Id, 3000m);

        var balances = await _service.GetLedgerBalances(DateTime.Today, CancellationToken.None);

        var cashBalance = balances.Single(b => b.LedgerId == debit.Id);
        Assert.Equal(8000m, cashBalance.DebitTotal);
        Assert.Equal(8000m, cashBalance.Balance);
    }

    [Fact]
    public async Task GetLedgerBalances_CancelledVouchers_AreExcluded()
    {
        var (debit, credit) = await SeedLedgersAsync();
        await SeedVoucherAsync(1, "JV-001", DateTime.Today, debit.Id, credit.Id, 10000m);
        await SeedVoucherAsync(2, "JV-002", DateTime.Today, debit.Id, credit.Id, 5000m, cancelled: true);

        var balances = await _service.GetLedgerBalances(DateTime.Today, CancellationToken.None);

        var cashBalance = balances.Single(b => b.LedgerId == debit.Id);
        Assert.Equal(10000m, cashBalance.DebitTotal); // Only the non-cancelled voucher
    }

    [Fact]
    public async Task GetLedgerBalances_FutureAsOfDate_ExcludesFutureVouchers()
    {
        var (debit, credit) = await SeedLedgersAsync();
        await SeedVoucherAsync(1, "JV-001", DateTime.Today.AddDays(-5), debit.Id, credit.Id, 10000m);
        await SeedVoucherAsync(2, "JV-002", DateTime.Today.AddDays(+5), debit.Id, credit.Id, 5000m);

        var balances = await _service.GetLedgerBalances(DateTime.Today, CancellationToken.None);

        var cashBalance = balances.Single(b => b.LedgerId == debit.Id);
        Assert.Equal(10000m, cashBalance.DebitTotal); // Only past voucher
    }

    [Fact]
    public async Task GetLedgerBalances_NoVouchers_ReturnsEmpty()
    {
        await SeedLedgersAsync();
        var balances = await _service.GetLedgerBalances(DateTime.Today, CancellationToken.None);
        Assert.Empty(balances);
    }

    // ── GetTrialBalance Tests ─────────────────────────────────────────────

    [Fact]
    public async Task GetTrialBalance_WithVouchers_TotalDebitEqualsTotalCredit()
    {
        var (debit, credit) = await SeedLedgersAsync();
        await SeedVoucherAsync(1, "JV-001", DateTime.Today, debit.Id, credit.Id, 20000m);

        var trialBalance = await _service.GetTrialBalance(DateTime.Today, CancellationToken.None);

        Assert.Equal(trialBalance.TotalDebit, trialBalance.TotalCredit);
    }

    [Fact]
    public async Task GetTrialBalance_ReturnsCorrectAsOfDate()
    {
        var (debit, credit) = await SeedLedgersAsync();
        var asOf = new DateTime(2025, 1, 31);

        var trialBalance = await _service.GetTrialBalance(asOf, CancellationToken.None);

        Assert.Equal(asOf, trialBalance.AsOfDate);
    }

    [Fact]
    public async Task GetTrialBalance_ContainsAllLedgers()
    {
        var (debit, credit) = await SeedLedgersAsync();
        await SeedVoucherAsync(1, "JV-001", DateTime.Today, debit.Id, credit.Id, 15000m);

        var trialBalance = await _service.GetTrialBalance(DateTime.Today, CancellationToken.None);

        Assert.Equal(2, trialBalance.Ledgers.Count);
    }

    // ── GetDayBook Tests ──────────────────────────────────────────────────

    [Fact]
    public async Task GetDayBook_ForSpecificDate_ReturnsVouchersOnThatDate()
    {
        var (debit, credit) = await SeedLedgersAsync();
        var targetDate = new DateTime(2025, 6, 15);
        await SeedVoucherAsync(1, "JV-001", targetDate,              debit.Id, credit.Id, 5000m);
        await SeedVoucherAsync(2, "JV-002", targetDate.AddDays(1),   debit.Id, credit.Id, 3000m);

        var entries = await _service.GetDayBook(targetDate, CancellationToken.None);

        Assert.Single(entries);
        Assert.Equal("JV-001", entries[0].VoucherNumber);
    }

    [Fact]
    public async Task GetDayBook_ForDateWithNoVouchers_ReturnsEmpty()
    {
        await SeedLedgersAsync();
        var entries = await _service.GetDayBook(DateTime.Today.AddYears(-5), CancellationToken.None);
        Assert.Empty(entries);
    }

    [Fact]
    public async Task GetDayBook_EachEntryHasTwoDetails()
    {
        var (debit, credit) = await SeedLedgersAsync();
        await SeedVoucherAsync(1, "JV-001", DateTime.Today, debit.Id, credit.Id, 7500m);

        var entries = await _service.GetDayBook(DateTime.Today, CancellationToken.None);

        Assert.Single(entries);
        Assert.Equal(2, entries[0].Details.Count);
    }

    [Fact]
    public async Task GetDayBook_CancelledVouchersExcluded()
    {
        var (debit, credit) = await SeedLedgersAsync();
        await SeedVoucherAsync(1, "JV-001", DateTime.Today, debit.Id, credit.Id, 5000m);
        await SeedVoucherAsync(2, "JV-002", DateTime.Today, debit.Id, credit.Id, 3000m, cancelled: true);

        var entries = await _service.GetDayBook(DateTime.Today, CancellationToken.None);

        Assert.Single(entries);
        Assert.Equal("JV-001", entries[0].VoucherNumber);
    }

    [Fact]
    public async Task GetDayBook_MultipleVouchers_OrderedByVoucherNumber()
    {
        var (debit, credit) = await SeedLedgersAsync();
        await SeedVoucherAsync(2, "JV-002", DateTime.Today, debit.Id, credit.Id, 3000m);
        await SeedVoucherAsync(1, "JV-001", DateTime.Today, debit.Id, credit.Id, 5000m);

        var entries = await _service.GetDayBook(DateTime.Today, CancellationToken.None);

        Assert.Equal(2, entries.Count);
        Assert.Equal("JV-001", entries[0].VoucherNumber);
        Assert.Equal("JV-002", entries[1].VoucherNumber);
    }
}
