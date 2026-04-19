using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Assets;

// ── AssetGroup Query ──────────────────────────────────────────────────────────

public record GetAllAssetGroupsQuery(string? Search, int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<AssetGroupDto>>>;

public class GetAllAssetGroupsHandler : IRequestHandler<GetAllAssetGroupsQuery, ApiResponse<List<AssetGroupDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetAllAssetGroupsHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<AssetGroupDto>>> Handle(GetAllAssetGroupsQuery r, CancellationToken ct)
    {
        var q = _db.AssetGroups.AsQueryable();
        if (!string.IsNullOrWhiteSpace(r.Search))
        {
            var s = r.Search.ToLower();
            q = q.Where(x => x.Name!.ToLower().Contains(s) || (x.Code != null && x.Code.ToLower().Contains(s)));
        }
        var total = await q.CountAsync(ct);
        var items = await q.OrderBy(x => x.Name)
            .Skip((r.Page - 1) * r.PageSize).Take(r.PageSize)
            .Select(x => new AssetGroupDto { Id = x.Id, Name = x.Name!, Code = x.Code, Description = x.Description, IsActive = x.IsActive })
            .ToListAsync(ct);
        return ApiResponse<List<AssetGroupDto>>.Success(items, "Success", total);
    }
}

// ── AssetType Query ───────────────────────────────────────────────────────────

public record GetAllAssetTypesQuery(string? Search, int? AssetGroupId, int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<AssetTypeDto>>>;

public class GetAllAssetTypesHandler : IRequestHandler<GetAllAssetTypesQuery, ApiResponse<List<AssetTypeDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetAllAssetTypesHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<AssetTypeDto>>> Handle(GetAllAssetTypesQuery r, CancellationToken ct)
    {
        var q = _db.AssetTypes.Include(x => x.AssetGroup).AsQueryable();
        if (!string.IsNullOrWhiteSpace(r.Search))
        {
            var s = r.Search.ToLower();
            q = q.Where(x => x.Name!.ToLower().Contains(s) || (x.Code != null && x.Code.ToLower().Contains(s)));
        }
        if (r.AssetGroupId.HasValue) q = q.Where(x => x.AssetGroupId == r.AssetGroupId);
        var total = await q.CountAsync(ct);
        var items = await q.OrderBy(x => x.Name)
            .Skip((r.Page - 1) * r.PageSize).Take(r.PageSize)
            .Select(x => new AssetTypeDto { Id = x.Id, Name = x.Name!, Code = x.Code, AssetGroupId = x.AssetGroupId, AssetGroupName = x.AssetGroup != null ? x.AssetGroup.Name : null, DepreciationRate = x.DepreciationRate, UsefulLifeYears = x.UsefulLifeYears, DepreciationMethod = x.DepreciationMethod.ToString(), IsActive = x.IsActive })
            .ToListAsync(ct);
        return ApiResponse<List<AssetTypeDto>>.Success(items, "Success", total);
    }
}

// ── AssetModel Query ──────────────────────────────────────────────────────────

public record GetAllAssetModelsQuery(string? Search, int? AssetTypeId, int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<AssetModelDto>>>;

public class GetAllAssetModelsHandler : IRequestHandler<GetAllAssetModelsQuery, ApiResponse<List<AssetModelDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetAllAssetModelsHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<AssetModelDto>>> Handle(GetAllAssetModelsQuery r, CancellationToken ct)
    {
        var q = _db.AssetModels.Include(x => x.AssetType).AsQueryable();
        if (!string.IsNullOrWhiteSpace(r.Search))
        {
            var s = r.Search.ToLower();
            q = q.Where(x => x.Name!.ToLower().Contains(s) || (x.Code != null && x.Code.ToLower().Contains(s)));
        }
        if (r.AssetTypeId.HasValue) q = q.Where(x => x.AssetTypeId == r.AssetTypeId);
        var total = await q.CountAsync(ct);
        var items = await q.OrderBy(x => x.Name)
            .Skip((r.Page - 1) * r.PageSize).Take(r.PageSize)
            .Select(x => new AssetModelDto { Id = x.Id, Name = x.Name!, Code = x.Code, AssetTypeId = x.AssetTypeId, AssetTypeName = x.AssetType != null ? x.AssetType.Name : null, Manufacturer = x.Manufacturer, Specifications = x.Specifications, IsActive = x.IsActive })
            .ToListAsync(ct);
        return ApiResponse<List<AssetModelDto>>.Success(items, "Success", total);
    }
}

// ── Asset Query ───────────────────────────────────────────────────────────────

public record GetAllAssetsQuery(string? Search, AssetStatus? Status, int? CategoryId, int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<AssetItemDto>>>;

public class GetAllAssetsHandler : IRequestHandler<GetAllAssetsQuery, ApiResponse<List<AssetItemDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetAllAssetsHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<AssetItemDto>>> Handle(GetAllAssetsQuery r, CancellationToken ct)
    {
        var q = _db.Assets
            .Include(x => x.AssetModel)
            .Include(x => x.AssetCategory)
            .Include(x => x.AssignedToEmployee)
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(r.Search))
        {
            var s = r.Search.ToLower();
            q = q.Where(x => x.AssetCode.ToLower().Contains(s) || (x.Name != null && x.Name.ToLower().Contains(s)));
        }
        if (r.Status.HasValue) q = q.Where(x => x.Status == r.Status.Value);
        if (r.CategoryId.HasValue) q = q.Where(x => x.AssetCategoryId == r.CategoryId.Value);
        var total = await q.CountAsync(ct);
        var items = await q.OrderBy(x => x.AssetCode)
            .Skip((r.Page - 1) * r.PageSize).Take(r.PageSize)
            .Select(x => new AssetItemDto { Id = x.Id, AssetCode = x.AssetCode, Name = x.Name, AssetModelId = x.AssetModelId, AssetModelName = x.AssetModel != null ? x.AssetModel.Name : null, AssetCategoryId = x.AssetCategoryId, AssetCategoryName = x.AssetCategory != null ? x.AssetCategory.Name : null, PurchaseDate = x.PurchaseDate, PurchaseCost = x.PurchaseCost, CurrentValue = x.CurrentValue, Location = x.Location, SerialNumber = x.SerialNumber, Status = x.Status.ToString(), AssignedToEmployeeId = x.AssignedToEmployeeId, AssignedToEmployeeName = x.AssignedToEmployee != null ? x.AssignedToEmployee.Name : null, Notes = x.Notes, IsActive = x.IsActive })
            .ToListAsync(ct);
        return ApiResponse<List<AssetItemDto>>.Success(items, "Success", total);
    }
}

// ── AssetCategory Query ───────────────────────────────────────────────────────

public record GetAllAssetCategoriesQuery(string? Search, int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<AssetCategoryDto>>>;

public class GetAllAssetCategoriesHandler : IRequestHandler<GetAllAssetCategoriesQuery, ApiResponse<List<AssetCategoryDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetAllAssetCategoriesHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<AssetCategoryDto>>> Handle(GetAllAssetCategoriesQuery r, CancellationToken ct)
    {
        var q = _db.AssetCategories.Include(x => x.ParentCategory).AsQueryable();
        if (!string.IsNullOrWhiteSpace(r.Search))
        {
            var s = r.Search.ToLower();
            q = q.Where(x => x.Name!.ToLower().Contains(s) || (x.Code != null && x.Code.ToLower().Contains(s)));
        }
        var total = await q.CountAsync(ct);
        var items = await q.OrderBy(x => x.Name)
            .Skip((r.Page - 1) * r.PageSize).Take(r.PageSize)
            .Select(x => new AssetCategoryDto { Id = x.Id, Name = x.Name!, Code = x.Code, ParentCategoryId = x.ParentCategoryId, ParentCategoryName = x.ParentCategory != null ? x.ParentCategory.Name : null, IsActive = x.IsActive })
            .ToListAsync(ct);
        return ApiResponse<List<AssetCategoryDto>>.Success(items, "Success", total);
    }
}

// ── AssetTransaction Query ────────────────────────────────────────────────────

public record GetAssetTransactionsQuery(int AssetId, int Page = 1, int PageSize = 20) : IRequest<ApiResponse<List<AssetTransactionDto>>>;

public class GetAssetTransactionsHandler : IRequestHandler<GetAssetTransactionsQuery, ApiResponse<List<AssetTransactionDto>>>
{
    private readonly IApplicationDbContext _db;
    public GetAssetTransactionsHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<AssetTransactionDto>>> Handle(GetAssetTransactionsQuery r, CancellationToken ct)
    {
        var q = _db.AssetTransactions
            .Include(x => x.Asset)
            .Include(x => x.FromEmployee)
            .Include(x => x.ToEmployee)
            .Where(x => x.AssetId == r.AssetId);
        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(x => x.TransactionDate)
            .Skip((r.Page - 1) * r.PageSize).Take(r.PageSize)
            .Select(x => new AssetTransactionDto { Id = x.Id, AssetId = x.AssetId, AssetCode = x.Asset != null ? x.Asset.AssetCode : null, AssetName = x.Asset != null ? x.Asset.Name : null, TransactionType = x.TransactionType.ToString(), TransactionDate = x.TransactionDate, FromEmployeeId = x.FromEmployeeId, FromEmployeeName = x.FromEmployee != null ? x.FromEmployee.Name : null, ToEmployeeId = x.ToEmployeeId, ToEmployeeName = x.ToEmployee != null ? x.ToEmployee.Name : null, Remarks = x.Remarks, Amount = x.Amount, DocumentNo = x.DocumentNo })
            .ToListAsync(ct);
        return ApiResponse<List<AssetTransactionDto>>.Success(items, "Success", total);
    }
}
