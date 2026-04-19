using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Reporting.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Reporting.Queries;

// ── Stock Aging ───────────────────────────────────────────────────────
public record GetStockAgingQuery() : IRequest<ApiResponse<List<StockAgingDto>>>;

public class GetStockAgingHandler : IRequestHandler<GetStockAgingQuery, ApiResponse<List<StockAgingDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetStockAgingHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<StockAgingDto>>> Handle(GetStockAgingQuery request, CancellationToken ct)
    {
        var today = DateTime.UtcNow;

        var stocks = await _db.Stocks
            .Include(s => s.Product)
            .Where(s => s.Quantity > 0)
            .ToListAsync(ct);

        var result = stocks.Select(s =>
        {
            var days = (int)(today - s.CreatedDate).TotalDays;
            return new StockAgingDto
            {
                ProductId = s.ProductId,
                ProductName = s.Product?.Name ?? string.Empty,
                Quantity = s.Quantity,
                DaysInStock = days,
                Value = s.Value,
                AgingBucket = days switch
                {
                    <= 30 => "0-30",
                    <= 60 => "31-60",
                    <= 90 => "61-90",
                    _ => "90+"
                }
            };
        }).OrderBy(s => s.ProductName).ToList();

        return ApiResponse<List<StockAgingDto>>.Success(result, "Stock aging report generated", result.Count);
    }
}

// ── Stock Movement ────────────────────────────────────────────────────
public record GetStockMovementQuery(DateTime? FromDate, DateTime? ToDate) : IRequest<ApiResponse<List<StockMovementDto>>>;

public class GetStockMovementHandler : IRequestHandler<GetStockMovementQuery, ApiResponse<List<StockMovementDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetStockMovementHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<StockMovementDto>>> Handle(GetStockMovementQuery request, CancellationToken ct)
    {
        var from = request.FromDate ?? DateTime.MinValue;
        var to = request.ToDate ?? DateTime.UtcNow;

        var products = await _db.Products
            .Where(p => !p.IsDeleted)
            .ToListAsync(ct);

        // Inward = purchase invoice details in date range
        var purchaseDetails = await _db.PurchaseInvoices
            .Include(p => p.Details)
            .Where(p => p.InvoiceDate >= from && p.InvoiceDate <= to)
            .SelectMany(p => p.Details)
            .GroupBy(d => d.ProductId)
            .Select(g => new { ProductId = g.Key, Qty = g.Sum(x => x.Quantity) })
            .ToListAsync(ct);

        // Outward = sales invoice details in date range
        var salesDetails = await _db.SalesInvoices
            .Include(s => s.Details)
            .Where(s => s.InvoiceDate >= from && s.InvoiceDate <= to)
            .SelectMany(s => s.Details)
            .GroupBy(d => d.ProductId)
            .Select(g => new { ProductId = g.Key, Qty = g.Sum(x => x.Quantity) })
            .ToListAsync(ct);

        var currentStock = await _db.Stocks
            .GroupBy(s => s.ProductId)
            .Select(g => new { ProductId = g.Key, Qty = g.Sum(x => x.Quantity) })
            .ToListAsync(ct);

        var result = products.Select(p =>
        {
            var inward = purchaseDetails.FirstOrDefault(x => x.ProductId == p.Id)?.Qty ?? 0;
            var outward = salesDetails.FirstOrDefault(x => x.ProductId == p.Id)?.Qty ?? 0;
            var closing = currentStock.FirstOrDefault(x => x.ProductId == p.Id)?.Qty ?? 0;
            var opening = closing - inward + outward;

            return new StockMovementDto
            {
                ProductId = p.Id,
                ProductName = p.Name ?? string.Empty,
                OpeningQty = opening,
                InwardQty = inward,
                OutwardQty = outward,
                ClosingQty = closing
            };
        }).OrderBy(s => s.ProductName).ToList();

        return ApiResponse<List<StockMovementDto>>.Success(result, "Stock movement report generated", result.Count);
    }
}

// ── Sales Analysis ────────────────────────────────────────────────────
public record GetSalesAnalysisQuery(DateTime? FromDate, DateTime? ToDate, string? GroupBy)
    : IRequest<ApiResponse<List<SalesAnalysisDto>>>;

public class GetSalesAnalysisHandler : IRequestHandler<GetSalesAnalysisQuery, ApiResponse<List<SalesAnalysisDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetSalesAnalysisHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<SalesAnalysisDto>>> Handle(GetSalesAnalysisQuery request, CancellationToken ct)
    {
        var from = request.FromDate ?? DateTime.MinValue;
        var to = request.ToDate ?? DateTime.UtcNow;
        var groupBy = request.GroupBy?.ToLowerInvariant() ?? "product";

        var invoices = await _db.SalesInvoices
            .Include(s => s.Details).ThenInclude(d => d.Product)
            .Include(s => s.Customer)
            .Where(s => s.InvoiceDate >= from && s.InvoiceDate <= to)
            .ToListAsync(ct);

        List<SalesAnalysisDto> result;
        var totalAmount = invoices.Sum(i => i.NetAmount);

        if (groupBy == "customer")
        {
            result = invoices
                .GroupBy(i => i.Customer?.Name ?? "Unknown")
                .Select(g => new SalesAnalysisDto
                {
                    GroupName = g.Key,
                    Quantity = g.SelectMany(i => i.Details).Sum(d => d.Quantity),
                    Amount = g.Sum(i => i.NetAmount),
                    Percentage = totalAmount > 0 ? Math.Round(g.Sum(i => i.NetAmount) / totalAmount * 100, 2) : 0
                })
                .OrderByDescending(s => s.Amount).ToList();
        }
        else if (groupBy == "agent")
        {
            result = invoices
                .GroupBy(i => i.SalesAgentId ?? 0)
                .Select(g => new SalesAnalysisDto
                {
                    GroupName = g.Key == 0 ? "Unassigned" : $"Agent #{g.Key}",
                    Quantity = g.SelectMany(i => i.Details).Sum(d => d.Quantity),
                    Amount = g.Sum(i => i.NetAmount),
                    Percentage = totalAmount > 0 ? Math.Round(g.Sum(i => i.NetAmount) / totalAmount * 100, 2) : 0
                })
                .OrderByDescending(s => s.Amount).ToList();
        }
        else // product
        {
            result = invoices
                .SelectMany(i => i.Details)
                .GroupBy(d => d.Product?.Name ?? "Unknown")
                .Select(g => new SalesAnalysisDto
                {
                    GroupName = g.Key,
                    Quantity = g.Sum(d => d.Quantity),
                    Amount = g.Sum(d => d.NetAmount),
                    Percentage = totalAmount > 0 ? Math.Round(g.Sum(d => d.NetAmount) / totalAmount * 100, 2) : 0
                })
                .OrderByDescending(s => s.Amount).ToList();
        }

        return ApiResponse<List<SalesAnalysisDto>>.Success(result, "Sales analysis generated", result.Count);
    }
}

// ── Purchase Analysis ─────────────────────────────────────────────────
public record GetPurchaseAnalysisQuery(DateTime? FromDate, DateTime? ToDate)
    : IRequest<ApiResponse<List<PurchaseAnalysisDto>>>;

public class GetPurchaseAnalysisHandler : IRequestHandler<GetPurchaseAnalysisQuery, ApiResponse<List<PurchaseAnalysisDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetPurchaseAnalysisHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<PurchaseAnalysisDto>>> Handle(GetPurchaseAnalysisQuery request, CancellationToken ct)
    {
        var from = request.FromDate ?? DateTime.MinValue;
        var to = request.ToDate ?? DateTime.UtcNow;

        var invoices = await _db.PurchaseInvoices
            .Include(p => p.Vendor)
            .Where(p => p.InvoiceDate >= from && p.InvoiceDate <= to)
            .ToListAsync(ct);

        var totalAmount = invoices.Sum(i => i.NetAmount);

        var result = invoices
            .GroupBy(i => i.Vendor?.Name ?? "Unknown")
            .Select(g => new PurchaseAnalysisDto
            {
                VendorName = g.Key,
                Amount = g.Sum(i => i.NetAmount),
                Percentage = totalAmount > 0 ? Math.Round(g.Sum(i => i.NetAmount) / totalAmount * 100, 2) : 0
            })
            .OrderByDescending(s => s.Amount)
            .ToList();

        return ApiResponse<List<PurchaseAnalysisDto>>.Success(result, "Purchase analysis generated", result.Count);
    }
}
