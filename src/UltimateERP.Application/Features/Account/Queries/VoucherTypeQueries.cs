using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Account.Queries;

// ── Receipt Voucher Queries ──────────────────────────────────────────

public record GetReceiptVouchersQuery(DateTime? FromDate, DateTime? ToDate, int? PartyLedgerId, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<ReceiptVoucherDto>>>;

public class GetReceiptVouchersHandler : IRequestHandler<GetReceiptVouchersQuery, ApiResponse<List<ReceiptVoucherDto>>>
{
    private readonly IApplicationDbContext _db;

    public GetReceiptVouchersHandler(IApplicationDbContext db) { _db = db; }

    public async Task<ApiResponse<List<ReceiptVoucherDto>>> Handle(GetReceiptVouchersQuery request, CancellationToken ct)
    {
        var query = _db.Vouchers
            .Include(v => v.Details).ThenInclude(d => d.Ledger)
            .Where(v => v.VoucherTypeId == 1) // Receipt
            .AsQueryable();

        if (request.FromDate.HasValue) query = query.Where(v => v.VoucherDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(v => v.VoucherDate <= request.ToDate);
        if (request.PartyLedgerId.HasValue)
            query = query.Where(v => v.Details.Any(d => d.LedgerId == request.PartyLedgerId && d.CreditAmount > 0));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(v => v.VoucherDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var result = items.Select(v =>
        {
            var bankDetail = v.Details.FirstOrDefault(d => d.DebitAmount > 0);
            var partyDetail = v.Details.FirstOrDefault(d => d.CreditAmount > 0);
            return new ReceiptVoucherDto
            {
                Id = v.Id,
                VoucherNo = v.VoucherNumber,
                Date = v.VoucherDate,
                Amount = v.TotalDebit,
                BankLedgerId = bankDetail?.LedgerId ?? 0,
                BankLedgerName = bankDetail?.Ledger?.Name,
                PartyLedgerId = partyDetail?.LedgerId,
                PartyLedgerName = partyDetail?.Ledger?.Name,
                Narration = v.CommonNarration
            };
        }).ToList();

        return ApiResponse<List<ReceiptVoucherDto>>.Success(result, "Receipt vouchers retrieved", total);
    }
}

// ── Payment Voucher Queries ──────────────────────────────────────────

public record GetPaymentVouchersQuery(DateTime? FromDate, DateTime? ToDate, int? PartyLedgerId, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<PaymentVoucherDto>>>;

public class GetPaymentVouchersHandler : IRequestHandler<GetPaymentVouchersQuery, ApiResponse<List<PaymentVoucherDto>>>
{
    private readonly IApplicationDbContext _db;

    public GetPaymentVouchersHandler(IApplicationDbContext db) { _db = db; }

    public async Task<ApiResponse<List<PaymentVoucherDto>>> Handle(GetPaymentVouchersQuery request, CancellationToken ct)
    {
        var query = _db.Vouchers
            .Include(v => v.Details).ThenInclude(d => d.Ledger)
            .Where(v => v.VoucherTypeId == 2) // Payment
            .AsQueryable();

        if (request.FromDate.HasValue) query = query.Where(v => v.VoucherDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(v => v.VoucherDate <= request.ToDate);
        if (request.PartyLedgerId.HasValue)
            query = query.Where(v => v.Details.Any(d => d.LedgerId == request.PartyLedgerId && d.DebitAmount > 0));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(v => v.VoucherDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var result = items.Select(v =>
        {
            var partyDetail = v.Details.FirstOrDefault(d => d.DebitAmount > 0);
            var bankDetail = v.Details.FirstOrDefault(d => d.CreditAmount > 0);
            return new PaymentVoucherDto
            {
                Id = v.Id,
                VoucherNo = v.VoucherNumber,
                Date = v.VoucherDate,
                Amount = v.TotalDebit,
                BankLedgerId = bankDetail?.LedgerId ?? 0,
                BankLedgerName = bankDetail?.Ledger?.Name,
                PartyLedgerId = partyDetail?.LedgerId,
                PartyLedgerName = partyDetail?.Ledger?.Name,
                Narration = v.CommonNarration
            };
        }).ToList();

        return ApiResponse<List<PaymentVoucherDto>>.Success(result, "Payment vouchers retrieved", total);
    }
}

// ── Journal Voucher Queries ──────────────────────────────────────────

public record GetJournalVouchersQuery(DateTime? FromDate, DateTime? ToDate, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<JournalVoucherDto>>>;

public class GetJournalVouchersHandler : IRequestHandler<GetJournalVouchersQuery, ApiResponse<List<JournalVoucherDto>>>
{
    private readonly IApplicationDbContext _db;

    public GetJournalVouchersHandler(IApplicationDbContext db) { _db = db; }

    public async Task<ApiResponse<List<JournalVoucherDto>>> Handle(GetJournalVouchersQuery request, CancellationToken ct)
    {
        var query = _db.Vouchers
            .Include(v => v.Details).ThenInclude(d => d.Ledger)
            .Where(v => v.VoucherTypeId == 3) // Journal
            .AsQueryable();

        if (request.FromDate.HasValue) query = query.Where(v => v.VoucherDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(v => v.VoucherDate <= request.ToDate);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(v => v.VoucherDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var result = items.Select(v => new JournalVoucherDto
        {
            Id = v.Id,
            VoucherNo = v.VoucherNumber,
            Date = v.VoucherDate,
            Narration = v.CommonNarration,
            TotalDebit = v.TotalDebit,
            TotalCredit = v.TotalCredit,
            Lines = v.Details.Select(d => new JournalLineDto
            {
                LedgerId = d.LedgerId,
                LedgerName = d.Ledger?.Name,
                DebitAmount = d.DebitAmount,
                CreditAmount = d.CreditAmount,
                CostCenterId = d.CostCenterId
            }).ToList()
        }).ToList();

        return ApiResponse<List<JournalVoucherDto>>.Success(result, "Journal vouchers retrieved", total);
    }
}

// ── Contra Voucher Queries ───────────────────────────────────────────

public record GetContraVouchersQuery(DateTime? FromDate, DateTime? ToDate, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<ContraVoucherDto>>>;

public class GetContraVouchersHandler : IRequestHandler<GetContraVouchersQuery, ApiResponse<List<ContraVoucherDto>>>
{
    private readonly IApplicationDbContext _db;

    public GetContraVouchersHandler(IApplicationDbContext db) { _db = db; }

    public async Task<ApiResponse<List<ContraVoucherDto>>> Handle(GetContraVouchersQuery request, CancellationToken ct)
    {
        var query = _db.Vouchers
            .Include(v => v.Details).ThenInclude(d => d.Ledger)
            .Where(v => v.VoucherTypeId == 4) // Contra
            .AsQueryable();

        if (request.FromDate.HasValue) query = query.Where(v => v.VoucherDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(v => v.VoucherDate <= request.ToDate);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(v => v.VoucherDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var result = items.Select(v =>
        {
            var toBank = v.Details.FirstOrDefault(d => d.DebitAmount > 0);
            var fromBank = v.Details.FirstOrDefault(d => d.CreditAmount > 0);
            return new ContraVoucherDto
            {
                Id = v.Id,
                VoucherNo = v.VoucherNumber,
                Date = v.VoucherDate,
                FromBankLedgerId = fromBank?.LedgerId ?? 0,
                FromBankLedgerName = fromBank?.Ledger?.Name,
                ToBankLedgerId = toBank?.LedgerId ?? 0,
                ToBankLedgerName = toBank?.Ledger?.Name,
                Amount = v.TotalDebit,
                Narration = v.CommonNarration
            };
        }).ToList();

        return ApiResponse<List<ContraVoucherDto>>.Success(result, "Contra vouchers retrieved", total);
    }
}
