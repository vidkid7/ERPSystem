using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Reporting.DTOs;
using UltimateERP.Application.Features.Reporting.Services;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Reporting.Queries;

// Trial Balance
public record GetTrialBalanceQuery(DateTime? AsOfDate) : IRequest<ApiResponse<TrialBalanceDto>>;

public class GetTrialBalanceHandler : IRequestHandler<GetTrialBalanceQuery, ApiResponse<TrialBalanceDto>>
{
    private readonly IApplicationDbContext _db;
    public GetTrialBalanceHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<TrialBalanceDto>> Handle(GetTrialBalanceQuery request, CancellationToken ct)
    {
        var service = new LedgerBalanceService(_db);
        var result = await service.GetTrialBalance(request.AsOfDate, ct);
        return ApiResponse<TrialBalanceDto>.Success(result, "Trial balance generated");
    }
}

// Day Book
public record GetDayBookQuery(DateTime Date) : IRequest<ApiResponse<List<DayBookEntryDto>>>;

public class GetDayBookHandler : IRequestHandler<GetDayBookQuery, ApiResponse<List<DayBookEntryDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetDayBookHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<DayBookEntryDto>>> Handle(GetDayBookQuery request, CancellationToken ct)
    {
        var service = new LedgerBalanceService(_db);
        var result = await service.GetDayBook(request.Date, ct);
        return ApiResponse<List<DayBookEntryDto>>.Success(result, "Day book retrieved", result.Count);
    }
}

// Ledger Statement (per-ledger detail with running balance)
public record GetLedgerStatementQuery(int LedgerId, DateTime? FromDate, DateTime? ToDate)
    : IRequest<ApiResponse<List<LedgerStatementLineDto>>>;

public class LedgerStatementLineDto
{
    public DateTime Date { get; set; }
    public string VoucherNumber { get; set; } = string.Empty;
    public string? Narration { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public decimal RunningBalance { get; set; }
}

public class GetLedgerStatementHandler : IRequestHandler<GetLedgerStatementQuery, ApiResponse<List<LedgerStatementLineDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetLedgerStatementHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<LedgerStatementLineDto>>> Handle(GetLedgerStatementQuery request, CancellationToken ct)
    {
        var from = request.FromDate ?? DateTime.MinValue;
        var to = request.ToDate ?? DateTime.UtcNow;

        var details = await _db.VoucherDetails
            .Include(vd => vd.Voucher)
            .Where(vd => vd.LedgerId == request.LedgerId
                      && vd.Voucher.VoucherDate >= from
                      && vd.Voucher.VoucherDate <= to
                      && !vd.Voucher.IsCancelled)
            .OrderBy(vd => vd.Voucher.VoucherDate)
            .ThenBy(vd => vd.Voucher.VoucherNumber)
            .Select(vd => new LedgerStatementLineDto
            {
                Date = vd.Voucher.VoucherDate,
                VoucherNumber = vd.Voucher.VoucherNumber,
                Narration = vd.Narration ?? vd.Voucher.CommonNarration,
                DebitAmount = vd.DebitAmount,
                CreditAmount = vd.CreditAmount
            })
            .ToListAsync(ct);

        // Calculate running balance
        decimal balance = 0;
        foreach (var line in details)
        {
            balance += line.DebitAmount - line.CreditAmount;
            line.RunningBalance = balance;
        }

        return ApiResponse<List<LedgerStatementLineDto>>.Success(details, "Ledger statement generated", details.Count);
    }
}

// Post Purchase Invoice to Accounting
public record PostPurchaseInvoiceCommand(int PurchaseInvoiceId, int PurchaseLedgerId) : IRequest<ApiResponse<int>>;

public class PostPurchaseInvoiceHandler : IRequestHandler<PostPurchaseInvoiceCommand, ApiResponse<int>>
{
    private readonly IApplicationDbContext _db;
    public PostPurchaseInvoiceHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<int>> Handle(PostPurchaseInvoiceCommand request, CancellationToken ct)
    {
        var service = new VoucherPostingService(_db);
        var voucher = await service.PostPurchaseInvoice(request.PurchaseInvoiceId, request.PurchaseLedgerId, ct);
        if (voucher is null) return ApiResponse<int>.Failure("Invoice not found or already posted");
        return ApiResponse<int>.Success(voucher.Id, "Purchase invoice posted to accounting");
    }
}

// Post Sales Invoice to Accounting
public record PostSalesInvoiceCommand(int SalesInvoiceId, int SalesLedgerId) : IRequest<ApiResponse<int>>;

public class PostSalesInvoiceHandler : IRequestHandler<PostSalesInvoiceCommand, ApiResponse<int>>
{
    private readonly IApplicationDbContext _db;
    public PostSalesInvoiceHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<int>> Handle(PostSalesInvoiceCommand request, CancellationToken ct)
    {
        var service = new VoucherPostingService(_db);
        var voucher = await service.PostSalesInvoice(request.SalesInvoiceId, request.SalesLedgerId, ct);
        if (voucher is null) return ApiResponse<int>.Failure("Invoice not found or already posted");
        return ApiResponse<int>.Success(voucher.Id, "Sales invoice posted to accounting");
    }
}
