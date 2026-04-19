using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Reporting.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Reporting.Queries;

// ── Cancel Day Book ───────────────────────────────────────────────────
public record GetCancelDayBookQuery(DateTime? FromDate, DateTime? ToDate)
    : IRequest<ApiResponse<List<CancelDayBookDto>>>;

public class GetCancelDayBookHandler
    : IRequestHandler<GetCancelDayBookQuery, ApiResponse<List<CancelDayBookDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetCancelDayBookHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<CancelDayBookDto>>> Handle(GetCancelDayBookQuery request, CancellationToken ct)
    {
        var from = request.FromDate ?? DateTime.MinValue;
        var to = request.ToDate ?? DateTime.UtcNow;

        var result = await _db.Vouchers
            .Where(v => v.IsCancelled
                     && v.CancelledDate >= from
                     && v.CancelledDate <= to)
            .OrderByDescending(v => v.CancelledDate)
            .Select(v => new CancelDayBookDto
            {
                VoucherId = v.Id,
                VoucherNumber = v.VoucherNumber,
                VoucherDate = v.VoucherDate,
                VoucherType = v.VoucherTypeId.ToString(),
                CancelledDate = v.CancelledDate,
                CancelReason = v.CancelReason,
                Amount = v.TotalDebit
            })
            .ToListAsync(ct);

        return ApiResponse<List<CancelDayBookDto>>.Success(result, "Cancel day book generated", result.Count);
    }
}

// ── Cost Center Analysis ──────────────────────────────────────────────
public record GetCostCenterAnalysisQuery(DateTime? FromDate, DateTime? ToDate)
    : IRequest<ApiResponse<List<CostCenterAnalysisDto>>>;

public class GetCostCenterAnalysisHandler
    : IRequestHandler<GetCostCenterAnalysisQuery, ApiResponse<List<CostCenterAnalysisDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetCostCenterAnalysisHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<CostCenterAnalysisDto>>> Handle(GetCostCenterAnalysisQuery request, CancellationToken ct)
    {
        var from = request.FromDate ?? DateTime.MinValue;
        var to = request.ToDate ?? DateTime.UtcNow;

        var result = await _db.VoucherDetails
            .Include(vd => vd.Voucher)
            .Where(vd => vd.CostCenterId.HasValue
                      && vd.Voucher.VoucherDate >= from
                      && vd.Voucher.VoucherDate <= to
                      && !vd.Voucher.IsCancelled)
            .GroupBy(vd => vd.CostCenterId!.Value)
            .Select(g => new CostCenterAnalysisDto
            {
                CostCenterId = g.Key,
                CostCenterName = $"Cost Center #{g.Key}",
                DebitTotal = g.Sum(x => x.DebitAmount),
                CreditTotal = g.Sum(x => x.CreditAmount),
                NetAmount = g.Sum(x => x.DebitAmount) - g.Sum(x => x.CreditAmount)
            })
            .OrderBy(c => c.CostCenterId)
            .ToListAsync(ct);

        return ApiResponse<List<CostCenterAnalysisDto>>.Success(result, "Cost center analysis generated", result.Count);
    }
}

// ── Bills Receivable ──────────────────────────────────────────────────
public record GetBillsReceivableQuery(DateTime? AsOfDate)
    : IRequest<ApiResponse<List<BillsReceivableDto>>>;

public class GetBillsReceivableHandler
    : IRequestHandler<GetBillsReceivableQuery, ApiResponse<List<BillsReceivableDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetBillsReceivableHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<BillsReceivableDto>>> Handle(GetBillsReceivableQuery request, CancellationToken ct)
    {
        var asOfDate = request.AsOfDate ?? DateTime.UtcNow;

        var customers = await _db.Customers
            .Include(c => c.Ledger)
            .Where(c => !c.IsDeleted)
            .ToListAsync(ct);

        var salesByCustomer = await _db.SalesInvoices
            .Where(s => s.InvoiceDate <= asOfDate)
            .GroupBy(s => s.CustomerId)
            .Select(g => new { CustomerId = g.Key, Total = g.Sum(x => x.NetAmount) })
            .ToListAsync(ct);

        // Payments received = credit entries on customer ledgers
        var customerLedgerIds = customers.Select(c => c.LedgerId).ToList();
        var payments = await _db.VoucherDetails
            .Include(vd => vd.Voucher)
            .Where(vd => customerLedgerIds.Contains(vd.LedgerId)
                      && vd.Voucher.VoucherDate <= asOfDate
                      && !vd.Voucher.IsCancelled)
            .GroupBy(vd => vd.LedgerId)
            .Select(g => new { LedgerId = g.Key, Received = g.Sum(x => x.CreditAmount) })
            .ToListAsync(ct);

        var result = customers.Select(c =>
        {
            var total = salesByCustomer.FirstOrDefault(s => s.CustomerId == c.Id)?.Total ?? 0;
            var received = payments.FirstOrDefault(p => p.LedgerId == c.LedgerId)?.Received ?? 0;
            var outstanding = total - received;
            if (outstanding <= 0) return null;

            return new BillsReceivableDto
            {
                CustomerId = c.Id,
                CustomerName = c.Name ?? string.Empty,
                TotalAmount = total,
                ReceivedAmount = received,
                OutstandingAmount = outstanding,
                AgingDays = (int)(asOfDate - (c.CreatedDate)).TotalDays
            };
        }).Where(b => b != null).Cast<BillsReceivableDto>()
        .OrderByDescending(b => b.OutstandingAmount).ToList();

        return ApiResponse<List<BillsReceivableDto>>.Success(result, "Bills receivable generated", result.Count);
    }
}

// ── Bills Payable ─────────────────────────────────────────────────────
public record GetBillsPayableQuery(DateTime? AsOfDate)
    : IRequest<ApiResponse<List<BillsPayableDto>>>;

public class GetBillsPayableHandler
    : IRequestHandler<GetBillsPayableQuery, ApiResponse<List<BillsPayableDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetBillsPayableHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<BillsPayableDto>>> Handle(GetBillsPayableQuery request, CancellationToken ct)
    {
        var asOfDate = request.AsOfDate ?? DateTime.UtcNow;

        var vendors = await _db.Vendors
            .Include(v => v.Ledger)
            .Where(v => !v.IsDeleted)
            .ToListAsync(ct);

        var purchasesByVendor = await _db.PurchaseInvoices
            .Where(p => p.InvoiceDate <= asOfDate)
            .GroupBy(p => p.VendorId)
            .Select(g => new { VendorId = g.Key, Total = g.Sum(x => x.NetAmount) })
            .ToListAsync(ct);

        // Payments made = debit entries on vendor ledgers
        var vendorLedgerIds = vendors.Select(v => v.LedgerId).ToList();
        var payments = await _db.VoucherDetails
            .Include(vd => vd.Voucher)
            .Where(vd => vendorLedgerIds.Contains(vd.LedgerId)
                      && vd.Voucher.VoucherDate <= asOfDate
                      && !vd.Voucher.IsCancelled)
            .GroupBy(vd => vd.LedgerId)
            .Select(g => new { LedgerId = g.Key, Paid = g.Sum(x => x.DebitAmount) })
            .ToListAsync(ct);

        var result = vendors.Select(v =>
        {
            var total = purchasesByVendor.FirstOrDefault(p => p.VendorId == v.Id)?.Total ?? 0;
            var paid = payments.FirstOrDefault(p => p.LedgerId == v.LedgerId)?.Paid ?? 0;
            var outstanding = total - paid;
            if (outstanding <= 0) return null;

            return new BillsPayableDto
            {
                VendorId = v.Id,
                VendorName = v.Name ?? string.Empty,
                TotalAmount = total,
                PaidAmount = paid,
                OutstandingAmount = outstanding,
                AgingDays = (int)(asOfDate - v.CreatedDate).TotalDays
            };
        }).Where(b => b != null).Cast<BillsPayableDto>()
        .OrderByDescending(b => b.OutstandingAmount).ToList();

        return ApiResponse<List<BillsPayableDto>>.Success(result, "Bills payable generated", result.Count);
    }
}
