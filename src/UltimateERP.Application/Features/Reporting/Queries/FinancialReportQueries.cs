using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Reporting.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Reporting.Queries;

// ── Balance Sheet ─────────────────────────────────────────────────────
public record GetBalanceSheetQuery(DateTime? FromDate, DateTime? ToDate) : IRequest<ApiResponse<BalanceSheetReportDto>>;

public class GetBalanceSheetHandler : IRequestHandler<GetBalanceSheetQuery, ApiResponse<BalanceSheetReportDto>>
{
    private readonly IApplicationDbContext _db;
    public GetBalanceSheetHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<BalanceSheetReportDto>> Handle(GetBalanceSheetQuery request, CancellationToken ct)
    {
        var asOfDate = request.ToDate ?? DateTime.UtcNow;

        var balances = await _db.VoucherDetails
            .Include(vd => vd.Voucher)
            .Include(vd => vd.Ledger).ThenInclude(l => l.LedgerGroup)
            .Where(vd => vd.Voucher.VoucherDate <= asOfDate && !vd.Voucher.IsCancelled)
            .GroupBy(vd => new
            {
                vd.Ledger.LedgerGroup.NatureOfGroup,
                GroupName = vd.Ledger.LedgerGroup.Name ?? string.Empty
            })
            .Select(g => new
            {
                g.Key.NatureOfGroup,
                g.Key.GroupName,
                Amount = g.Sum(x => x.DebitAmount) - g.Sum(x => x.CreditAmount)
            })
            .ToListAsync(ct);

        var result = new BalanceSheetReportDto { AsOfDate = asOfDate };

        result.Assets.Items = balances
            .Where(b => b.NatureOfGroup == NatureOfGroup.Asset)
            .Select(b => new ReportSectionItemDto { LedgerGroupName = b.GroupName, Amount = b.Amount })
            .ToList();
        result.Assets.Total = result.Assets.Items.Sum(i => i.Amount);

        result.Liabilities.Items = balances
            .Where(b => b.NatureOfGroup == NatureOfGroup.Liability)
            .Select(b => new ReportSectionItemDto { LedgerGroupName = b.GroupName, Amount = Math.Abs(b.Amount) })
            .ToList();
        result.Liabilities.Total = result.Liabilities.Items.Sum(i => i.Amount);

        result.NetWorth = result.Assets.Total - result.Liabilities.Total;

        return ApiResponse<BalanceSheetReportDto>.Success(result, "Balance sheet generated");
    }
}

// ── Profit & Loss ─────────────────────────────────────────────────────
public record GetProfitLossQuery(DateTime? FromDate, DateTime? ToDate) : IRequest<ApiResponse<ProfitLossReportDto>>;

public class GetProfitLossHandler : IRequestHandler<GetProfitLossQuery, ApiResponse<ProfitLossReportDto>>
{
    private readonly IApplicationDbContext _db;
    public GetProfitLossHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<ProfitLossReportDto>> Handle(GetProfitLossQuery request, CancellationToken ct)
    {
        var from = request.FromDate ?? DateTime.MinValue;
        var to = request.ToDate ?? DateTime.UtcNow;

        var balances = await _db.VoucherDetails
            .Include(vd => vd.Voucher)
            .Include(vd => vd.Ledger).ThenInclude(l => l.LedgerGroup)
            .Where(vd => vd.Voucher.VoucherDate >= from
                      && vd.Voucher.VoucherDate <= to
                      && !vd.Voucher.IsCancelled
                      && (vd.Ledger.LedgerGroup.NatureOfGroup == NatureOfGroup.Income
                       || vd.Ledger.LedgerGroup.NatureOfGroup == NatureOfGroup.Expense))
            .GroupBy(vd => new
            {
                vd.Ledger.LedgerGroup.NatureOfGroup,
                GroupName = vd.Ledger.LedgerGroup.Name ?? string.Empty
            })
            .Select(g => new
            {
                g.Key.NatureOfGroup,
                g.Key.GroupName,
                Amount = g.Sum(x => x.CreditAmount) - g.Sum(x => x.DebitAmount)
            })
            .ToListAsync(ct);

        var result = new ProfitLossReportDto { FromDate = from, ToDate = to };

        result.Income.Items = balances
            .Where(b => b.NatureOfGroup == NatureOfGroup.Income)
            .Select(b => new ReportSectionItemDto { LedgerGroupName = b.GroupName, Amount = b.Amount })
            .ToList();
        result.Income.Total = result.Income.Items.Sum(i => i.Amount);

        result.Expense.Items = balances
            .Where(b => b.NatureOfGroup == NatureOfGroup.Expense)
            .Select(b => new ReportSectionItemDto { LedgerGroupName = b.GroupName, Amount = Math.Abs(b.Amount) })
            .ToList();
        result.Expense.Total = result.Expense.Items.Sum(i => i.Amount);

        result.NetProfit = result.Income.Total - result.Expense.Total;

        return ApiResponse<ProfitLossReportDto>.Success(result, "Profit & loss generated");
    }
}

// ── Cash Flow ─────────────────────────────────────────────────────────
public record GetCashFlowQuery(DateTime? FromDate, DateTime? ToDate) : IRequest<ApiResponse<CashFlowDto>>;

public class GetCashFlowHandler : IRequestHandler<GetCashFlowQuery, ApiResponse<CashFlowDto>>
{
    private readonly IApplicationDbContext _db;
    public GetCashFlowHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<CashFlowDto>> Handle(GetCashFlowQuery request, CancellationToken ct)
    {
        var from = request.FromDate ?? DateTime.MinValue;
        var to = request.ToDate ?? DateTime.UtcNow;

        // Simplified cash flow: group by nature of group
        var flows = await _db.VoucherDetails
            .Include(vd => vd.Voucher)
            .Include(vd => vd.Ledger).ThenInclude(l => l.LedgerGroup)
            .Where(vd => vd.Voucher.VoucherDate >= from
                      && vd.Voucher.VoucherDate <= to
                      && !vd.Voucher.IsCancelled)
            .GroupBy(vd => vd.Ledger.LedgerGroup.NatureOfGroup)
            .Select(g => new
            {
                Nature = g.Key,
                Net = g.Sum(x => x.DebitAmount) - g.Sum(x => x.CreditAmount)
            })
            .ToListAsync(ct);

        var operating = flows.Where(f => f.Nature == NatureOfGroup.Income || f.Nature == NatureOfGroup.Expense)
                            .Sum(f => f.Net);
        var investing = flows.Where(f => f.Nature == NatureOfGroup.Asset).Sum(f => f.Net);
        var financing = flows.Where(f => f.Nature == NatureOfGroup.Liability).Sum(f => f.Net);

        var result = new CashFlowDto
        {
            FromDate = from,
            ToDate = to,
            OperatingActivities = operating,
            InvestingActivities = investing,
            FinancingActivities = financing,
            NetCashFlow = operating + investing + financing
        };

        return ApiResponse<CashFlowDto>.Success(result, "Cash flow statement generated");
    }
}

// ── VAT Report ────────────────────────────────────────────────────────
public record GetVATReportQuery(DateTime? FromDate, DateTime? ToDate) : IRequest<ApiResponse<VATReportDto>>;

public class GetVATReportHandler : IRequestHandler<GetVATReportQuery, ApiResponse<VATReportDto>>
{
    private readonly IApplicationDbContext _db;
    public GetVATReportHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<VATReportDto>> Handle(GetVATReportQuery request, CancellationToken ct)
    {
        var from = request.FromDate ?? DateTime.MinValue;
        var to = request.ToDate ?? DateTime.UtcNow;

        var salesVat = await _db.SalesInvoices
            .Where(s => s.InvoiceDate >= from && s.InvoiceDate <= to)
            .SumAsync(s => s.TaxAmount, ct);

        var purchaseVat = await _db.PurchaseInvoices
            .Where(p => p.InvoiceDate >= from && p.InvoiceDate <= to)
            .SumAsync(p => p.TaxAmount, ct);

        var result = new VATReportDto
        {
            FromDate = from,
            ToDate = to,
            SalesVAT = salesVat,
            PurchaseVAT = purchaseVat,
            VATPayable = salesVat - purchaseVat
        };

        return ApiResponse<VATReportDto>.Success(result, "VAT report generated");
    }
}

// ── TDS Report ────────────────────────────────────────────────────────
public record GetTDSReportQuery(DateTime? FromDate, DateTime? ToDate) : IRequest<ApiResponse<TDSReportDto>>;

public class GetTDSReportHandler : IRequestHandler<GetTDSReportQuery, ApiResponse<TDSReportDto>>
{
    private readonly IApplicationDbContext _db;
    public GetTDSReportHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<TDSReportDto>> Handle(GetTDSReportQuery request, CancellationToken ct)
    {
        var from = request.FromDate ?? DateTime.MinValue;
        var to = request.ToDate ?? DateTime.UtcNow;

        // TDS is typically deducted on vendor payments; use purchase invoices as proxy
        var items = await _db.PurchaseInvoices
            .Include(p => p.Vendor)
            .Where(p => p.InvoiceDate >= from && p.InvoiceDate <= to)
            .GroupBy(p => new { p.VendorId, p.Vendor.Name, p.Vendor.PANNumber })
            .Select(g => new TDSReportItemDto
            {
                PartyName = g.Key.Name ?? string.Empty,
                PanNo = g.Key.PANNumber,
                Amount = g.Sum(x => x.NetAmount),
                TDSRate = 0, // TDS rate is determined by tax rules; placeholder
                TDSAmount = 0
            })
            .ToListAsync(ct);

        var result = new TDSReportDto
        {
            FromDate = from,
            ToDate = to,
            Items = items,
            TotalTDSAmount = items.Sum(i => i.TDSAmount)
        };

        return ApiResponse<TDSReportDto>.Success(result, "TDS report generated");
    }
}
