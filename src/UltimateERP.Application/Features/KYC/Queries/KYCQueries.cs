using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.KYC.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.KYC.Queries;

public record GetKYCRecordsQuery(string? Search, int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<KYCRecordDto>>>;

public class GetKYCRecordsHandler : IRequestHandler<GetKYCRecordsQuery, ApiResponse<List<KYCRecordDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetKYCRecordsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<KYCRecordDto>>> Handle(GetKYCRecordsQuery request, CancellationToken ct)
    {
        var query = _db.KYCRecords.Include(k => k.Customer).AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(k => k.FullName!.Contains(request.Search) || k.MobileNo!.Contains(request.Search) || k.CitizenshipNo!.Contains(request.Search));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(k => k.CreatedDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<KYCRecordDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<KYCRecordDto>>.Success(items, "KYC records retrieved", total);
    }
}

public record GetPendingKYCQuery(int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<KYCRecordDto>>>;

public class GetPendingKYCHandler : IRequestHandler<GetPendingKYCQuery, ApiResponse<List<KYCRecordDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetPendingKYCHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<KYCRecordDto>>> Handle(GetPendingKYCQuery request, CancellationToken ct)
    {
        var query = _db.KYCRecords
            .Include(k => k.Customer)
            .Where(k => !k.IsVerified);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(k => k.CreatedDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<KYCRecordDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<KYCRecordDto>>.Success(items, "Pending KYC records retrieved", total);
    }
}
