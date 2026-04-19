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

// Get Tasks by Assignee
public record GetTasksByAssigneeQuery(int AssignedToId, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<TaskItemDto>>>;

public class GetTasksByAssigneeHandler : IRequestHandler<GetTasksByAssigneeQuery, ApiResponse<List<TaskItemDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetTasksByAssigneeHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async System.Threading.Tasks.Task<ApiResponse<List<TaskItemDto>>> Handle(GetTasksByAssigneeQuery request, CancellationToken ct)
    {
        var query = _db.TaskItems
            .Include(t => t.AssignedTo)
            .Include(t => t.AssignedBy)
            .Where(t => t.AssignedToId == request.AssignedToId);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(t => t.CreatedDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<TaskItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<TaskItemDto>>.Success(items, "Tasks by assignee retrieved", total);
    }
}

// Get Tasks by Date Range
public record GetTasksByDateRangeQuery(DateTime From, DateTime To, int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<TaskItemDto>>>;

public class GetTasksByDateRangeHandler : IRequestHandler<GetTasksByDateRangeQuery, ApiResponse<List<TaskItemDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetTasksByDateRangeHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async System.Threading.Tasks.Task<ApiResponse<List<TaskItemDto>>> Handle(GetTasksByDateRangeQuery request, CancellationToken ct)
    {
        var query = _db.TaskItems
            .Include(t => t.AssignedTo)
            .Include(t => t.AssignedBy)
            .Where(t => t.DueDate.HasValue && t.DueDate.Value >= request.From && t.DueDate.Value <= request.To);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(t => t.DueDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<TaskItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<TaskItemDto>>.Success(items, "Tasks by date range retrieved", total);
    }
}

// Get Overdue Tasks
public record GetOverdueTasksQuery(int Page = 1, int PageSize = 20)
    : IRequest<ApiResponse<List<TaskItemDto>>>;

public class GetOverdueTasksHandler : IRequestHandler<GetOverdueTasksQuery, ApiResponse<List<TaskItemDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetOverdueTasksHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async System.Threading.Tasks.Task<ApiResponse<List<TaskItemDto>>> Handle(GetOverdueTasksQuery request, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var query = _db.TaskItems
            .Include(t => t.AssignedTo)
            .Include(t => t.AssignedBy)
            .Where(t => t.DueDate.HasValue && t.DueDate.Value < now
                && t.Status != Domain.Enums.TaskStatus.Completed
                && t.Status != Domain.Enums.TaskStatus.Cancelled);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(t => t.DueDate)
            .Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .ProjectTo<TaskItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<TaskItemDto>>.Success(items, "Overdue tasks retrieved", total);
    }
}
