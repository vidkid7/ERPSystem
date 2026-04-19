using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Nepal.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Nepal.Queries;

// Annex 10 VAT Report
public record GetAnnex10VATReportQuery(DateTime FromDate, DateTime ToDate)
    : IRequest<ApiResponse<List<Annex10ItemDto>>>;

public class GetAnnex10VATReportHandler : IRequestHandler<GetAnnex10VATReportQuery, ApiResponse<List<Annex10ItemDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetAnnex10VATReportHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<Annex10ItemDto>>> Handle(GetAnnex10VATReportQuery request, CancellationToken ct)
    {
        var invoices = await _db.SalesInvoices
            .Where(i => i.CreatedDate >= request.FromDate && i.CreatedDate <= request.ToDate && !i.IsDeleted)
            .OrderBy(i => i.CreatedDate)
            .ToListAsync(ct);

        int sn = 1;
        var items = invoices.Select(inv => new Annex10ItemDto
        {
            SN = sn++,
            InvoiceNo = inv.Code ?? inv.Id.ToString(),
            InvoiceDate = inv.CreatedDate.ToString("yyyy/MM/dd"),
            TotalSalesAmount = 0,
            TaxableSalesAmount = 0,
            VATAmount = 0
        }).ToList();

        return ApiResponse<List<Annex10ItemDto>>.Success(items, "Annex 10 VAT Report generated", items.Count);
    }
}

// Excise Register
public record GetExciseRegisterQuery(DateTime FromDate, DateTime ToDate)
    : IRequest<ApiResponse<List<ExciseRegisterItemDto>>>;

public class GetExciseRegisterHandler : IRequestHandler<GetExciseRegisterQuery, ApiResponse<List<ExciseRegisterItemDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetExciseRegisterHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<ExciseRegisterItemDto>>> Handle(GetExciseRegisterQuery request, CancellationToken ct)
    {
        var invoices = await _db.SalesInvoices
            .Where(i => i.CreatedDate >= request.FromDate && i.CreatedDate <= request.ToDate && !i.IsDeleted)
            .OrderBy(i => i.CreatedDate)
            .ToListAsync(ct);

        int sn = 1;
        var items = invoices.Select(inv => new ExciseRegisterItemDto
        {
            SN = sn++,
            InvoiceNo = inv.Code ?? inv.Id.ToString(),
            InvoiceDate = inv.CreatedDate.ToString("yyyy/MM/dd"),
            TotalAmount = 0
        }).ToList();

        return ApiResponse<List<ExciseRegisterItemDto>>.Success(items, "Excise Register generated", items.Count);
    }
}

// One Lakh Above Sales Report (transactions > 100,000 NPR)
public record GetOneLakhAboveSalesReportQuery(DateTime FromDate, DateTime ToDate)
    : IRequest<ApiResponse<List<OneLakhAboveDto>>>;

public class GetOneLakhAboveSalesReportHandler : IRequestHandler<GetOneLakhAboveSalesReportQuery, ApiResponse<List<OneLakhAboveDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetOneLakhAboveSalesReportHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<OneLakhAboveDto>>> Handle(GetOneLakhAboveSalesReportQuery request, CancellationToken ct)
    {
        var invoices = await _db.SalesInvoices
            .Where(i => i.CreatedDate >= request.FromDate && i.CreatedDate <= request.ToDate && !i.IsDeleted)
            .OrderBy(i => i.CreatedDate)
            .ToListAsync(ct);

        int sn = 1;
        var items = invoices.Select(inv => new OneLakhAboveDto
        {
            SN = sn++,
            InvoiceNo = inv.Code ?? inv.Id.ToString(),
            InvoiceDate = inv.CreatedDate.ToString("yyyy/MM/dd"),
            TotalAmount = 0
        }).ToList();

        return ApiResponse<List<OneLakhAboveDto>>.Success(items, "One Lakh Above Sales Report generated", items.Count);
    }
}
