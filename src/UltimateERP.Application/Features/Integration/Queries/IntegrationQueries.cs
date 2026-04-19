using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Features.Integration.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Integration.Queries;

public record ExportLedgersQuery : IRequest<ApiResponse<byte[]>>;

public class ExportLedgersHandler : IRequestHandler<ExportLedgersQuery, ApiResponse<byte[]>>
{
    private readonly IApplicationDbContext _db;
    private readonly IExcelService _excelService;
    private readonly IMapper _mapper;

    public ExportLedgersHandler(IApplicationDbContext db, IExcelService excelService, IMapper mapper)
    {
        _db = db;
        _excelService = excelService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<byte[]>> Handle(ExportLedgersQuery request, CancellationToken ct)
    {
        var ledgers = await _db.Ledgers
            .Include(l => l.LedgerGroup)
            .Where(l => l.IsActive)
            .ProjectTo<LedgerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        var bytes = await _excelService.ExportToExcelAsync(ledgers, "Ledgers");
        return ApiResponse<byte[]>.Success(bytes, "Ledgers exported", ledgers.Count);
    }
}

public record ExportProductsQuery : IRequest<ApiResponse<byte[]>>;

public class ExportProductsHandler : IRequestHandler<ExportProductsQuery, ApiResponse<byte[]>>
{
    private readonly IApplicationDbContext _db;
    private readonly IExcelService _excelService;

    public ExportProductsHandler(IApplicationDbContext db, IExcelService excelService)
    {
        _db = db;
        _excelService = excelService;
    }

    public async Task<ApiResponse<byte[]>> Handle(ExportProductsQuery request, CancellationToken ct)
    {
        var products = await _db.Products
            .Where(p => p.IsActive)
            .Select(p => new { p.Id, p.Code, p.Name, p.MRP, p.RetailRate, p.WholesaleRate, p.HSNCode, p.TaxRate })
            .ToListAsync(ct);

        var bytes = await _excelService.ExportToExcelAsync(products.Cast<object>().ToList(), "Products");
        return ApiResponse<byte[]>.Success(bytes, "Products exported", products.Count);
    }
}
