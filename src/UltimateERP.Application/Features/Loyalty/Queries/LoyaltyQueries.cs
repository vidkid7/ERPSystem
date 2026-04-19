using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Loyalty.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Loyalty.Queries;

public record GetMemberPointsQuery(int CustomerId, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<MembershipPointDto>>>;

public class GetMemberPointsHandler : IRequestHandler<GetMemberPointsQuery, ApiResponse<List<MembershipPointDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetMemberPointsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<MembershipPointDto>>> Handle(GetMemberPointsQuery request, CancellationToken ct)
    {
        var query = _db.MembershipPoints
            .Include(m => m.Customer)
            .Where(m => m.CustomerId == request.CustomerId);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(m => m.TransactionDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<MembershipPointDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<MembershipPointDto>>.Success(items, "Member points retrieved", total);
    }
}

// Get Points Balance
public record GetPointsBalanceQuery(int CustomerId) : IRequest<ApiResponse<PointsBalanceDto>>;

public class GetPointsBalanceHandler : IRequestHandler<GetPointsBalanceQuery, ApiResponse<PointsBalanceDto>>
{
    private readonly IApplicationDbContext _db;
    public GetPointsBalanceHandler(IApplicationDbContext db) { _db = db; }

    public async Task<ApiResponse<PointsBalanceDto>> Handle(GetPointsBalanceQuery request, CancellationToken ct)
    {
        var lastEntry = await _db.MembershipPoints
            .Include(m => m.Customer)
            .Where(m => m.CustomerId == request.CustomerId)
            .OrderByDescending(m => m.TransactionDate)
            .FirstOrDefaultAsync(ct);

        if (lastEntry == null)
            return ApiResponse<PointsBalanceDto>.Failure("No membership points found for this customer");

        var dto = new PointsBalanceDto
        {
            CustomerId = lastEntry.CustomerId,
            CustomerName = lastEntry.Customer?.Name,
            CurrentBalance = lastEntry.Balance
        };

        return ApiResponse<PointsBalanceDto>.Success(dto, "Points balance retrieved");
    }
}

// Get Points History
public record GetPointsHistoryQuery(int CustomerId, DateTime? From, DateTime? To, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<MembershipPointDto>>>;

public class GetPointsHistoryHandler : IRequestHandler<GetPointsHistoryQuery, ApiResponse<List<MembershipPointDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetPointsHistoryHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<MembershipPointDto>>> Handle(GetPointsHistoryQuery request, CancellationToken ct)
    {
        var query = _db.MembershipPoints
            .Include(m => m.Customer)
            .Where(m => m.CustomerId == request.CustomerId);

        if (request.From.HasValue)
            query = query.Where(m => m.TransactionDate >= request.From.Value);
        if (request.To.HasValue)
            query = query.Where(m => m.TransactionDate <= request.To.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(m => m.TransactionDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<MembershipPointDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<MembershipPointDto>>.Success(items, "Points history retrieved", total);
    }
}
