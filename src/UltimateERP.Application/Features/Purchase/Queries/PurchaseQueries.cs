using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Purchase.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Purchase.Queries;

public record GetPurchaseInvoicesQuery(int? VendorId, DateTime? FromDate, DateTime? ToDate, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<PurchaseInvoiceDto>>>;

public class GetPurchaseInvoicesHandler : IRequestHandler<GetPurchaseInvoicesQuery, ApiResponse<List<PurchaseInvoiceDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPurchaseInvoicesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<PurchaseInvoiceDto>>> Handle(GetPurchaseInvoicesQuery request, CancellationToken ct)
    {
        var query = _db.PurchaseInvoices
            .Include(p => p.Vendor).Include(p => p.Godown)
            .Include(p => p.Details).ThenInclude(d => d.Product)
            .AsQueryable();

        if (request.VendorId.HasValue) query = query.Where(p => p.VendorId == request.VendorId);
        if (request.FromDate.HasValue) query = query.Where(p => p.InvoiceDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(p => p.InvoiceDate <= request.ToDate);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(p => p.InvoiceDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<PurchaseInvoiceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<PurchaseInvoiceDto>>.Success(items, "Purchase invoices retrieved", total);
    }
}
