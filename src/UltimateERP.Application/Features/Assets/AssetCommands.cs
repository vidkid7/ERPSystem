using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Assets;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Assets;

// ── AssetGroup Commands ───────────────────────────────────────────────────────

public record CreateAssetGroupCommand(string Name, string? Code, string? Description) : IRequest<ApiResponse<int>>;

public class CreateAssetGroupHandler : IRequestHandler<CreateAssetGroupCommand, ApiResponse<int>>
{
    private readonly IApplicationDbContext _db;
    public CreateAssetGroupHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<int>> Handle(CreateAssetGroupCommand r, CancellationToken ct)
    {
        var entity = new AssetGroup { Name = r.Name, Code = r.Code, Description = r.Description };
        _db.AssetGroups.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<int>.Success(entity.Id, "Asset group created");
    }
}

public record UpdateAssetGroupCommand(int Id, string Name, string? Code, string? Description) : IRequest<ApiResponse<bool>>;

public class UpdateAssetGroupHandler : IRequestHandler<UpdateAssetGroupCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _db;
    public UpdateAssetGroupHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<bool>> Handle(UpdateAssetGroupCommand r, CancellationToken ct)
    {
        var entity = await _db.AssetGroups.FindAsync(new object[] { r.Id }, ct);
        if (entity is null) return ApiResponse<bool>.Failure("Asset group not found");
        entity.Name = r.Name; entity.Code = r.Code; entity.Description = r.Description;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<bool>.Success(true, "Updated");
    }
}

public record DeleteAssetGroupCommand(int Id) : IRequest<ApiResponse<bool>>;

public class DeleteAssetGroupHandler : IRequestHandler<DeleteAssetGroupCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _db;
    public DeleteAssetGroupHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<bool>> Handle(DeleteAssetGroupCommand r, CancellationToken ct)
    {
        var entity = await _db.AssetGroups.FindAsync(new object[] { r.Id }, ct);
        if (entity is null) return ApiResponse<bool>.Failure("Not found");
        entity.IsDeleted = true;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<bool>.Success(true, "Deleted");
    }
}

// ── AssetType Commands ────────────────────────────────────────────────────────

public record CreateAssetTypeCommand(string Name, string? Code, int? AssetGroupId, decimal DepreciationRate, int UsefulLifeYears, DepreciationMethod DepreciationMethod) : IRequest<ApiResponse<int>>;

public class CreateAssetTypeHandler : IRequestHandler<CreateAssetTypeCommand, ApiResponse<int>>
{
    private readonly IApplicationDbContext _db;
    public CreateAssetTypeHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<int>> Handle(CreateAssetTypeCommand r, CancellationToken ct)
    {
        var entity = new AssetType { Name = r.Name, Code = r.Code, AssetGroupId = r.AssetGroupId, DepreciationRate = r.DepreciationRate, UsefulLifeYears = r.UsefulLifeYears, DepreciationMethod = r.DepreciationMethod };
        _db.AssetTypes.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<int>.Success(entity.Id, "Asset type created");
    }
}

public record UpdateAssetTypeCommand(int Id, string Name, string? Code, int? AssetGroupId, decimal DepreciationRate, int UsefulLifeYears, DepreciationMethod DepreciationMethod) : IRequest<ApiResponse<bool>>;

public class UpdateAssetTypeHandler : IRequestHandler<UpdateAssetTypeCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _db;
    public UpdateAssetTypeHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<bool>> Handle(UpdateAssetTypeCommand r, CancellationToken ct)
    {
        var entity = await _db.AssetTypes.FindAsync(new object[] { r.Id }, ct);
        if (entity is null) return ApiResponse<bool>.Failure("Asset type not found");
        entity.Name = r.Name; entity.Code = r.Code; entity.AssetGroupId = r.AssetGroupId;
        entity.DepreciationRate = r.DepreciationRate; entity.UsefulLifeYears = r.UsefulLifeYears; entity.DepreciationMethod = r.DepreciationMethod;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<bool>.Success(true, "Updated");
    }
}

// ── AssetModel Commands ───────────────────────────────────────────────────────

public record CreateAssetModelCommand(string Name, string? Code, int? AssetTypeId, string? Manufacturer, string? Specifications) : IRequest<ApiResponse<int>>;

public class CreateAssetModelHandler : IRequestHandler<CreateAssetModelCommand, ApiResponse<int>>
{
    private readonly IApplicationDbContext _db;
    public CreateAssetModelHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<int>> Handle(CreateAssetModelCommand r, CancellationToken ct)
    {
        var entity = new AssetModel { Name = r.Name, Code = r.Code, AssetTypeId = r.AssetTypeId, Manufacturer = r.Manufacturer, Specifications = r.Specifications };
        _db.AssetModels.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<int>.Success(entity.Id, "Asset model created");
    }
}

public record UpdateAssetModelCommand(int Id, string Name, string? Code, int? AssetTypeId, string? Manufacturer, string? Specifications) : IRequest<ApiResponse<bool>>;

public class UpdateAssetModelHandler : IRequestHandler<UpdateAssetModelCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _db;
    public UpdateAssetModelHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<bool>> Handle(UpdateAssetModelCommand r, CancellationToken ct)
    {
        var entity = await _db.AssetModels.FindAsync(new object[] { r.Id }, ct);
        if (entity is null) return ApiResponse<bool>.Failure("Asset model not found");
        entity.Name = r.Name; entity.Code = r.Code; entity.AssetTypeId = r.AssetTypeId;
        entity.Manufacturer = r.Manufacturer; entity.Specifications = r.Specifications;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<bool>.Success(true, "Updated");
    }
}

// ── Asset Commands ────────────────────────────────────────────────────────────

public record CreateAssetCommand(string AssetCode, string? Name, int? AssetModelId, int? AssetCategoryId, DateTime? PurchaseDate, decimal PurchaseCost, string? Location, string? SerialNumber, int? AssignedToEmployeeId, string? Notes) : IRequest<ApiResponse<int>>;

public class CreateAssetHandler : IRequestHandler<CreateAssetCommand, ApiResponse<int>>
{
    private readonly IApplicationDbContext _db;
    public CreateAssetHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<int>> Handle(CreateAssetCommand r, CancellationToken ct)
    {
        var exists = await _db.Assets.AnyAsync(a => a.AssetCode == r.AssetCode, ct);
        if (exists) return ApiResponse<int>.Failure($"Asset code '{r.AssetCode}' already exists");
        var entity = new Asset { AssetCode = r.AssetCode, Name = r.Name, AssetModelId = r.AssetModelId, AssetCategoryId = r.AssetCategoryId, PurchaseDate = r.PurchaseDate, PurchaseCost = r.PurchaseCost, CurrentValue = r.PurchaseCost, Location = r.Location, SerialNumber = r.SerialNumber, AssignedToEmployeeId = r.AssignedToEmployeeId, Notes = r.Notes, Status = AssetStatus.Available };
        _db.Assets.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<int>.Success(entity.Id, "Asset created");
    }
}

public record UpdateAssetCommand(int Id, string AssetCode, string? Name, int? AssetModelId, int? AssetCategoryId, DateTime? PurchaseDate, decimal PurchaseCost, decimal CurrentValue, string? Location, string? SerialNumber, AssetStatus Status, int? AssignedToEmployeeId, string? Notes) : IRequest<ApiResponse<bool>>;

public class UpdateAssetHandler : IRequestHandler<UpdateAssetCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _db;
    public UpdateAssetHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<bool>> Handle(UpdateAssetCommand r, CancellationToken ct)
    {
        var entity = await _db.Assets.FindAsync(new object[] { r.Id }, ct);
        if (entity is null) return ApiResponse<bool>.Failure("Asset not found");
        entity.AssetCode = r.AssetCode; entity.Name = r.Name; entity.AssetModelId = r.AssetModelId;
        entity.AssetCategoryId = r.AssetCategoryId; entity.PurchaseDate = r.PurchaseDate;
        entity.PurchaseCost = r.PurchaseCost; entity.CurrentValue = r.CurrentValue;
        entity.Location = r.Location; entity.SerialNumber = r.SerialNumber;
        entity.Status = r.Status; entity.AssignedToEmployeeId = r.AssignedToEmployeeId; entity.Notes = r.Notes;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<bool>.Success(true, "Updated");
    }
}

// ── AssetCategory Commands ────────────────────────────────────────────────────

public record CreateAssetCategoryCommand(string Name, string? Code, int? ParentCategoryId) : IRequest<ApiResponse<int>>;

public class CreateAssetCategoryHandler : IRequestHandler<CreateAssetCategoryCommand, ApiResponse<int>>
{
    private readonly IApplicationDbContext _db;
    public CreateAssetCategoryHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<int>> Handle(CreateAssetCategoryCommand r, CancellationToken ct)
    {
        var entity = new AssetCategory { Name = r.Name, Code = r.Code, ParentCategoryId = r.ParentCategoryId };
        _db.AssetCategories.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<int>.Success(entity.Id, "Asset category created");
    }
}

public record UpdateAssetCategoryCommand(int Id, string Name, string? Code, int? ParentCategoryId) : IRequest<ApiResponse<bool>>;

public class UpdateAssetCategoryHandler : IRequestHandler<UpdateAssetCategoryCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _db;
    public UpdateAssetCategoryHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<bool>> Handle(UpdateAssetCategoryCommand r, CancellationToken ct)
    {
        var entity = await _db.AssetCategories.FindAsync(new object[] { r.Id }, ct);
        if (entity is null) return ApiResponse<bool>.Failure("Asset category not found");
        entity.Name = r.Name; entity.Code = r.Code; entity.ParentCategoryId = r.ParentCategoryId;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<bool>.Success(true, "Updated");
    }
}

// ── AssetTransaction Commands ─────────────────────────────────────────────────

public record CreateAssetTransactionCommand(int AssetId, AssetTransactionType TransactionType, DateTime TransactionDate, int? FromEmployeeId, int? ToEmployeeId, string? Remarks, decimal Amount, string? DocumentNo) : IRequest<ApiResponse<int>>;

public class CreateAssetTransactionHandler : IRequestHandler<CreateAssetTransactionCommand, ApiResponse<int>>
{
    private readonly IApplicationDbContext _db;
    public CreateAssetTransactionHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<int>> Handle(CreateAssetTransactionCommand r, CancellationToken ct)
    {
        var asset = await _db.Assets.FindAsync(new object[] { r.AssetId }, ct);
        if (asset is null) return ApiResponse<int>.Failure("Asset not found");

        // Update asset status and current value based on transaction type
        switch (r.TransactionType)
        {
            case AssetTransactionType.Issue:
                asset.Status = AssetStatus.Issued;
                asset.AssignedToEmployeeId = r.ToEmployeeId;
                break;
            case AssetTransactionType.Return:
                asset.Status = AssetStatus.Available;
                asset.AssignedToEmployeeId = null;
                break;
            case AssetTransactionType.Transfer:
                asset.Status = AssetStatus.Issued;
                asset.AssignedToEmployeeId = r.ToEmployeeId;
                break;
            case AssetTransactionType.Damage:
                asset.Status = AssetStatus.Damaged;
                if (r.Amount > 0) asset.CurrentValue = Math.Max(0, asset.CurrentValue - r.Amount);
                break;
            case AssetTransactionType.Repair:
                asset.Status = AssetStatus.Available;
                break;
            case AssetTransactionType.Disposal:
                asset.Status = AssetStatus.Disposed;
                asset.CurrentValue = 0;
                asset.IsActive = false;
                break;
        }

        var transaction = new AssetTransaction
        {
            AssetId = r.AssetId,
            TransactionType = r.TransactionType,
            TransactionDate = r.TransactionDate,
            FromEmployeeId = r.FromEmployeeId,
            ToEmployeeId = r.ToEmployeeId,
            Remarks = r.Remarks,
            Amount = r.Amount,
            DocumentNo = r.DocumentNo
        };
        _db.AssetTransactions.Add(transaction);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<int>.Success(transaction.Id, "Asset transaction recorded");
    }
}
