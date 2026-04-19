using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Inventory.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Application.Features.Inventory.Commands;

// ── Product Commands ──────────────────────────────────────────────────

public record CreateProductCommand(CreateProductDto Product) : IRequest<ApiResponse<ProductDto>>;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Product.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Product.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ApiResponse<ProductDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateProductHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ProductDto>> Handle(CreateProductCommand request, CancellationToken ct)
    {
        if (await _db.Products.AnyAsync(p => p.Code == request.Product.Code, ct))
            return ApiResponse<ProductDto>.Failure($"Product code '{request.Product.Code}' already exists");

        var entity = _mapper.Map<Product>(request.Product);
        _db.Products.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<ProductDto>.Success(_mapper.Map<ProductDto>(entity), "Product created");
    }
}

public record UpdateProductCommand(int Id, CreateProductDto Product) : IRequest<ApiResponse<ProductDto>>;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ApiResponse<ProductDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public UpdateProductHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ProductDto>> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var entity = await _db.Products.FindAsync(new object[] { request.Id }, ct);
        if (entity is null) return ApiResponse<ProductDto>.Failure("Product not found");

        _mapper.Map(request.Product, entity);
        entity.Id = request.Id;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<ProductDto>.Success(_mapper.Map<ProductDto>(entity), "Product updated");
    }
}

// ── ProductGroup Commands ─────────────────────────────────────────────

public record CreateProductGroupCommand(CreateProductGroupDto Group) : IRequest<ApiResponse<ProductGroupDto>>;

public class CreateProductGroupHandler : IRequestHandler<CreateProductGroupCommand, ApiResponse<ProductGroupDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateProductGroupHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ProductGroupDto>> Handle(CreateProductGroupCommand request, CancellationToken ct)
    {
        if (await _db.ProductGroups.AnyAsync(g => g.Code == request.Group.Code, ct))
            return ApiResponse<ProductGroupDto>.Failure($"Product group code '{request.Group.Code}' already exists");

        var entity = _mapper.Map<ProductGroup>(request.Group);
        _db.ProductGroups.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<ProductGroupDto>.Success(_mapper.Map<ProductGroupDto>(entity), "Product group created");
    }
}

// ── Godown Commands ───────────────────────────────────────────────────

public record CreateGodownCommand(CreateGodownDto Godown) : IRequest<ApiResponse<GodownDto>>;

public class CreateGodownHandler : IRequestHandler<CreateGodownCommand, ApiResponse<GodownDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateGodownHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<GodownDto>> Handle(CreateGodownCommand request, CancellationToken ct)
    {
        if (await _db.Godowns.AnyAsync(g => g.Code == request.Godown.Code, ct))
            return ApiResponse<GodownDto>.Failure($"Godown code '{request.Godown.Code}' already exists");

        var entity = _mapper.Map<Godown>(request.Godown);
        _db.Godowns.Add(entity);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<GodownDto>.Success(_mapper.Map<GodownDto>(entity), "Godown created");
    }
}
