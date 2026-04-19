using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Assets.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Assets.Queries;

public record GetAssetsQuery(string? Search, string? Status, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<AssetDto>>>;

public class GetAssetsHandler : IRequestHandler<GetAssetsQuery, ApiResponse<List<AssetDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetAssetsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<AssetDto>>> Handle(GetAssetsQuery request, CancellationToken ct)
    {
        var query = _db.AssetMasters
            .Include(a => a.Vendor)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.ToLower();
            query = query.Where(a => a.AssetCode.ToLower().Contains(s)
                                  || a.AssetName.ToLower().Contains(s));
        }
        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<Domain.Enums.AssetStatus>(request.Status, out var status))
            query = query.Where(a => a.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(a => a.AssetCode)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<AssetDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<AssetDto>>.Success(items, "Assets retrieved", total);
    }
}
