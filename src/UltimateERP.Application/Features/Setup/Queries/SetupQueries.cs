using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Setup.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Setup.Queries;

// ── Branch Queries ────────────────────────────────────────────────────

public record GetBranchesQuery(string? Search, int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<BranchDto>>>;

public class GetBranchesHandler : IRequestHandler<GetBranchesQuery, ApiResponse<List<BranchDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetBranchesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<BranchDto>>> Handle(GetBranchesQuery request, CancellationToken ct)
    {
        var query = _db.Branches.Where(b => b.IsActive);
        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(b => b.Name.Contains(request.Search) || b.Code.Contains(request.Search));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(b => b.Code)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<BranchDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<BranchDto>>.Success(items, "Branches retrieved", total);
    }
}

public record GetBranchByIdQuery(int Id) : IRequest<ApiResponse<BranchDto>>;

public class GetBranchByIdHandler : IRequestHandler<GetBranchByIdQuery, ApiResponse<BranchDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetBranchByIdHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<BranchDto>> Handle(GetBranchByIdQuery request, CancellationToken ct)
    {
        var entity = await _db.Branches.FindAsync(new object[] { request.Id }, ct);
        if (entity is null) return ApiResponse<BranchDto>.Failure("Branch not found");
        return ApiResponse<BranchDto>.Success(_mapper.Map<BranchDto>(entity));
    }
}

// ── CostClass Queries ─────────────────────────────────────────────────

public record GetCostClassesQuery(string? Search) : IRequest<ApiResponse<List<CostClassDto>>>;

public class GetCostClassesHandler : IRequestHandler<GetCostClassesQuery, ApiResponse<List<CostClassDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetCostClassesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<CostClassDto>>> Handle(GetCostClassesQuery request, CancellationToken ct)
    {
        var query = _db.CostClasses.Where(c => c.IsActive);
        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(c => c.Name.Contains(request.Search) || c.Code.Contains(request.Search));

        var items = await query.OrderBy(c => c.Code).ProjectTo<CostClassDto>(_mapper.ConfigurationProvider).ToListAsync(ct);
        return ApiResponse<List<CostClassDto>>.Success(items, "CostClasses retrieved", items.Count);
    }
}

// ── DocumentType Queries ──────────────────────────────────────────────

public record GetDocumentTypesQuery(string? Module) : IRequest<ApiResponse<List<DocumentTypeDto>>>;

public class GetDocumentTypesHandler : IRequestHandler<GetDocumentTypesQuery, ApiResponse<List<DocumentTypeDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetDocumentTypesHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<DocumentTypeDto>>> Handle(GetDocumentTypesQuery request, CancellationToken ct)
    {
        var query = _db.DocumentTypes.Where(d => d.IsActive);
        if (!string.IsNullOrEmpty(request.Module))
            query = query.Where(d => d.Module == request.Module);

        var items = await query.OrderBy(d => d.Code).ProjectTo<DocumentTypeDto>(_mapper.ConfigurationProvider).ToListAsync(ct);
        return ApiResponse<List<DocumentTypeDto>>.Success(items, "DocumentTypes retrieved", items.Count);
    }
}

// ── EntityNumbering Queries ───────────────────────────────────────────

public record GetEntityNumberingsQuery : IRequest<ApiResponse<List<EntityNumberingDto>>>;

public class GetEntityNumberingsHandler : IRequestHandler<GetEntityNumberingsQuery, ApiResponse<List<EntityNumberingDto>>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetEntityNumberingsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<List<EntityNumberingDto>>> Handle(GetEntityNumberingsQuery request, CancellationToken ct)
    {
        var items = await _db.EntityNumberings
            .Where(e => e.IsActive)
            .OrderBy(e => e.EntityId)
            .ProjectTo<EntityNumberingDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);

        return ApiResponse<List<EntityNumberingDto>>.Success(items, "EntityNumberings retrieved", items.Count);
    }
}
