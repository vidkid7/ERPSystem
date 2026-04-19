using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Manufacturing.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Manufacturing.Commands;

// Create BOM
public record CreateBOMCommand(CreateBOMDto Dto) : IRequest<ApiResponse<BOMDto>>;

public class CreateBOMHandler : IRequestHandler<CreateBOMCommand, ApiResponse<BOMDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateBOMHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<BOMDto>> Handle(CreateBOMCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var bom = new BOM
        {
            ProductId = dto.ProductId,
            BOMDate = dto.BOMDate,
            TotalCost = dto.TotalCost
        };

        for (int i = 0; i < dto.Details.Count; i++)
        {
            var d = dto.Details[i];
            bom.Details.Add(new BOMDetail
            {
                LineNumber = i + 1,
                ComponentProductId = d.ComponentProductId,
                Quantity = d.Quantity,
                UnitId = d.UnitId,
                Rate = d.Rate,
                Amount = d.Amount
            });
        }

        _db.BOMs.Add(bom);
        await _db.SaveChangesAsync(ct);

        var result = await _db.BOMs
            .Include(b => b.Product)
            .Include(b => b.Details).ThenInclude(d => d.ComponentProduct)
            .FirstAsync(b => b.Id == bom.Id, ct);

        return ApiResponse<BOMDto>.Success(_mapper.Map<BOMDto>(result), "BOM created");
    }
}

// Create Production Order
public record CreateProductionOrderCommand(CreateProductionOrderDto Dto) : IRequest<ApiResponse<ProductionOrderDto>>;

public class CreateProductionOrderHandler : IRequestHandler<CreateProductionOrderCommand, ApiResponse<ProductionOrderDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateProductionOrderHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ProductionOrderDto>> Handle(CreateProductionOrderCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var order = new ProductionOrder
        {
            OrderNumber = dto.OrderNumber,
            OrderDate = dto.OrderDate,
            OrderDateBS = dto.OrderDateBS,
            FinishedProductId = dto.FinishedProductId,
            BOMId = dto.BOMId,
            PlannedQuantity = dto.PlannedQuantity,
            GodownId = dto.GodownId,
            CostClassId = dto.CostClassId,
            BranchId = dto.BranchId,
            Status = ProductionOrderStatus.Pending
        };

        _db.ProductionOrders.Add(order);
        await _db.SaveChangesAsync(ct);

        var result = await _db.ProductionOrders
            .Include(p => p.FinishedProduct)
            .FirstAsync(p => p.Id == order.Id, ct);

        return ApiResponse<ProductionOrderDto>.Success(_mapper.Map<ProductionOrderDto>(result), "Production order created");
    }
}

// Complete Production Order
public record CompleteProductionOrderCommand(CompleteProductionOrderDto Dto) : IRequest<ApiResponse<ProductionOrderDto>>;

public class CompleteProductionOrderHandler : IRequestHandler<CompleteProductionOrderCommand, ApiResponse<ProductionOrderDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CompleteProductionOrderHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ProductionOrderDto>> Handle(CompleteProductionOrderCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var order = await _db.ProductionOrders
            .Include(p => p.FinishedProduct)
            .FirstOrDefaultAsync(p => p.Id == dto.ProductionOrderId, ct);

        if (order == null)
            return ApiResponse<ProductionOrderDto>.Failure("Production order not found");

        order.Status = ProductionOrderStatus.Completed;
        order.ProducedQuantity = dto.ProducedQuantity;
        order.CompletionDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<ProductionOrderDto>.Success(_mapper.Map<ProductionOrderDto>(order), "Production order completed");
    }
}
