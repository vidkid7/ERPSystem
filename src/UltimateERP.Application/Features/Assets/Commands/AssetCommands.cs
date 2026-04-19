using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Assets.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Assets;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Assets.Commands;

// Register Asset
public record RegisterAssetCommand(RegisterAssetDto Asset) : IRequest<ApiResponse<AssetDto>>;

public class RegisterAssetValidator : AbstractValidator<RegisterAssetCommand>
{
    public RegisterAssetValidator()
    {
        RuleFor(x => x.Asset.AssetCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Asset.AssetName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Asset.PurchaseValue).GreaterThanOrEqualTo(0);
    }
}

public class RegisterAssetHandler : IRequestHandler<RegisterAssetCommand, ApiResponse<AssetDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public RegisterAssetHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<AssetDto>> Handle(RegisterAssetCommand request, CancellationToken ct)
    {
        var dto = request.Asset;
        var exists = await _db.Assets.AnyAsync(a => a.AssetCode == dto.AssetCode, ct);
        if (exists) return ApiResponse<AssetDto>.Failure($"Asset code {dto.AssetCode} already exists");

        var asset = new AssetMaster
        {
            AssetCode = dto.AssetCode,
            AssetName = dto.AssetName,
            AssetTypeId = dto.AssetTypeId,
            AssetGroupId = dto.AssetGroupId,
            AssetCategoryId = dto.AssetCategoryId,
            PurchaseDate = dto.PurchaseDate,
            PurchaseValue = dto.PurchaseValue,
            VendorId = dto.VendorId,
            SerialNumber = dto.SerialNumber,
            Location = dto.Location,
            Status = AssetStatus.Available
        };

        _db.Assets.Add(asset);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<AssetDto>.Success(_mapper.Map<AssetDto>(asset), "Asset registered");
    }
}

// Calculate Depreciation (straight-line method)
public record CalculateDepreciationCommand(int AssetId, decimal UsefulLifeYears) : IRequest<ApiResponse<DepreciationResultDto>>;

public class CalculateDepreciationHandler : IRequestHandler<CalculateDepreciationCommand, ApiResponse<DepreciationResultDto>>
{
    private readonly IApplicationDbContext _db;
    public CalculateDepreciationHandler(IApplicationDbContext db) { _db = db; }

    public async Task<ApiResponse<DepreciationResultDto>> Handle(CalculateDepreciationCommand request, CancellationToken ct)
    {
        var asset = await _db.Assets.FindAsync(new object[] { request.AssetId }, ct);
        if (asset is null) return ApiResponse<DepreciationResultDto>.Failure("Asset not found");
        if (request.UsefulLifeYears <= 0) return ApiResponse<DepreciationResultDto>.Failure("Useful life must be greater than 0");

        var annualDepreciation = asset.PurchaseValue / request.UsefulLifeYears;
        var yearsUsed = asset.PurchaseDate.HasValue
            ? (decimal)(DateTime.UtcNow - asset.PurchaseDate.Value).TotalDays / 365.25m
            : 0;
        var accumulated = Math.Min(annualDepreciation * yearsUsed, asset.PurchaseValue);
        var nbv = asset.PurchaseValue - accumulated;

        var result = new DepreciationResultDto
        {
            AssetId = asset.Id,
            AssetCode = asset.AssetCode,
            AssetName = asset.AssetName,
            PurchaseValue = asset.PurchaseValue,
            AnnualDepreciation = Math.Round(annualDepreciation, 2),
            AccumulatedDepreciation = Math.Round(accumulated, 2),
            NetBookValue = Math.Round(nbv, 2)
        };

        return ApiResponse<DepreciationResultDto>.Success(result, "Depreciation calculated");
    }
}
