using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Sales.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Sales.Queries;

public record GetSalesInvoicesQuery(int? CustomerId, DateTime? FromDate, DateTime? ToDate, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<SalesInvoiceDto>>>;

public class GetSalesInvoicesHandler : IRequestHandler<GetSalesInvoicesQuery, ApiResponse<List<SalesInvoiceDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetSalesInvoicesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<SalesInvoiceDto>>> Handle(GetSalesInvoicesQuery request, CancellationToken ct)
    {
        var query = _db.SalesInvoices
            .Include(s => s.Customer).Include(s => s.Godown)
            .Include(s => s.Details).ThenInclude(d => d.Product)
            .AsQueryable();

        if (request.CustomerId.HasValue) query = query.Where(s => s.CustomerId == request.CustomerId);
        if (request.FromDate.HasValue) query = query.Where(s => s.InvoiceDate >= request.FromDate);
        if (request.ToDate.HasValue) query = query.Where(s => s.InvoiceDate <= request.ToDate);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(s => s.InvoiceDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<SalesInvoiceDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<SalesInvoiceDto>>.Success(items, "Sales invoices retrieved", total);
    }
}
