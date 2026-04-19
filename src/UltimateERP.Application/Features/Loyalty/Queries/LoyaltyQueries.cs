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
