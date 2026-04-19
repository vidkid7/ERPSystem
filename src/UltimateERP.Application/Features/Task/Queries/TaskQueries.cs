using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Task.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Task.Queries;

public record GetTasksQuery(string? Status, int? AssignedToId, string? Search, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<TaskItemDto>>>;

public class GetTasksHandler : IRequestHandler<GetTasksQuery, ApiResponse<List<TaskItemDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetTasksHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async System.Threading.Tasks.Task<ApiResponse<List<TaskItemDto>>> Handle(GetTasksQuery request, CancellationToken ct)
    {
        var query = _db.TaskItems
            .Include(t => t.AssignedTo)
            .Include(t => t.AssignedBy)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<Domain.Enums.TaskStatus>(request.Status, out var status))
            query = query.Where(t => t.Status == status);
        if (request.AssignedToId.HasValue)
            query = query.Where(t => t.AssignedToId == request.AssignedToId);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.ToLower();
            query = query.Where(t => t.TaskTitle.ToLower().Contains(s));
        }

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(t => t.CreatedDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<TaskItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<TaskItemDto>>.Success(items, "Tasks retrieved", total);
    }
}
