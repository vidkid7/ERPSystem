using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.AppCMS.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.AppCMS.Queries;

// Get Sliders
public record GetSlidersQuery(bool? ActiveOnly, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<SliderDto>>>;

public class GetSlidersHandler : IRequestHandler<GetSlidersQuery, ApiResponse<List<SliderDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetSlidersHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<SliderDto>>> Handle(GetSlidersQuery request, CancellationToken ct)
    {
        var query = _db.Sliders.AsQueryable();

        if (request.ActiveOnly == true)
            query = query.Where(s => s.IsActive && (s.ValidTo == null || s.ValidTo >= DateTime.UtcNow));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(s => s.DisplayOrder)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<SliderDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<SliderDto>>.Success(items, "Sliders retrieved", total);
    }
}

// Get Banners
public record GetBannersQuery(bool? ActiveOnly, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<BannerDto>>>;

public class GetBannersHandler : IRequestHandler<GetBannersQuery, ApiResponse<List<BannerDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetBannersHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<BannerDto>>> Handle(GetBannersQuery request, CancellationToken ct)
    {
        var query = _db.Banners.AsQueryable();

        if (request.ActiveOnly == true)
            query = query.Where(b => b.IsActive && (b.ValidTo == null || b.ValidTo >= DateTime.UtcNow));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(b => b.DisplayOrder)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<BannerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<BannerDto>>.Success(items, "Banners retrieved", total);
    }
}

// Get Notices
public record GetNoticesQuery(bool? ActiveOnly, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<NoticeDto>>>;

public class GetNoticesHandler : IRequestHandler<GetNoticesQuery, ApiResponse<List<NoticeDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetNoticesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<NoticeDto>>> Handle(GetNoticesQuery request, CancellationToken ct)
    {
        var query = _db.Notices.AsQueryable();

        if (request.ActiveOnly == true)
            query = query.Where(n => n.IsActive && (n.ExpiryDate == null || n.ExpiryDate >= DateTime.UtcNow));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(n => n.PublishDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<NoticeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<NoticeDto>>.Success(items, "Notices retrieved", total);
    }
}
