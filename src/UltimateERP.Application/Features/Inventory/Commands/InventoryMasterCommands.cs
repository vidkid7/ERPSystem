using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Inventory.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Inventory.Commands;

// ══════════════════════════════════════════════════════════════════════
// RACK
// ══════════════════════════════════════════════════════════════════════

public record CreateRackCommand(CreateRackDto Dto) : IRequest<ApiResponse<RackDto>>;

public class CreateRackValidator : AbstractValidator<CreateRackCommand>
{
    public CreateRackValidator()
    {
        RuleFor(x => x.Dto.GodownId).GreaterThan(0);
    }
}

public class CreateRackHandler : IRequestHandler<CreateRackCommand, ApiResponse<RackDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateRackHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<RackDto>> Handle(CreateRackCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var godown = await _db.Godowns.FindAsync(new object[] { dto.GodownId }, ct);
        if (godown is null) return ApiResponse<RackDto>.Failure("Godown not found");

        var entity = new Rack
        {
            Code = dto.Code,
            Name = dto.Name,
            GodownId = dto.GodownId,
            Location = dto.Location
        };

        _db.Racks.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.Racks
            .Include(r => r.Godown)
            .FirstAsync(r => r.Id == entity.Id, ct);
        return ApiResponse<RackDto>.Success(_mapper.Map<RackDto>(saved), "Rack created");
    }
}

public record UpdateRackCommand(int Id, CreateRackDto Dto) : IRequest<ApiResponse<RackDto>>;

public class UpdateRackHandler : IRequestHandler<UpdateRackCommand, ApiResponse<RackDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public UpdateRackHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<RackDto>> Handle(UpdateRackCommand request, CancellationToken ct)
    {
        var entity = await _db.Racks.Include(r => r.Godown).FirstOrDefaultAsync(r => r.Id == request.Id, ct);
        if (entity is null) return ApiResponse<RackDto>.Failure("Rack not found");

        entity.Code = request.Dto.Code;
        entity.Name = request.Dto.Name;
        entity.GodownId = request.Dto.GodownId;
        entity.Location = request.Dto.Location;
        await _db.SaveChangesAsync(ct);

        return ApiResponse<RackDto>.Success(_mapper.Map<RackDto>(entity), "Rack updated");
    }
}

// ══════════════════════════════════════════════════════════════════════
// INDENT
// ══════════════════════════════════════════════════════════════════════

public record CreateIndentCommand(CreateIndentDto Dto) : IRequest<ApiResponse<IndentDto>>;

public class CreateIndentValidator : AbstractValidator<CreateIndentCommand>
{
    public CreateIndentValidator()
    {
        RuleFor(x => x.Dto.Items).NotEmpty();
        RuleForEach(x => x.Dto.Items).ChildRules(d =>
        {
            d.RuleFor(x => x.ProductId).GreaterThan(0);
            d.RuleFor(x => x.RequestedQty).GreaterThan(0);
        });
    }
}

public class CreateIndentHandler : IRequestHandler<CreateIndentCommand, ApiResponse<IndentDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateIndentHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<IndentDto>> Handle(CreateIndentCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var entity = new Indent
        {
            IndentNumber = $"IND-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            IndentDate = dto.Date,
            RequestedByEmployeeId = dto.RequestedByEmployeeId,
            GodownId = dto.GodownId,
            Remarks = dto.Remarks,
            Status = IndentStatus.Pending
        };

        int lineNum = 1;
        foreach (var item in dto.Items)
        {
            entity.Details.Add(new IndentDetail
            {
                LineNumber = lineNum++,
                ProductId = item.ProductId,
                RequestedQuantity = item.RequestedQty,
                Remarks = item.Remarks
            });
        }

        _db.Indents.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.Indents
            .Include(i => i.RequestedByEmployee)
            .Include(i => i.Details).ThenInclude(d => d.Product)
            .FirstAsync(i => i.Id == entity.Id, ct);
        return ApiResponse<IndentDto>.Success(_mapper.Map<IndentDto>(saved), "Indent created");
    }
}

public record ApproveIndentCommand(int Id) : IRequest<ApiResponse<IndentDto>>;

public class ApproveIndentHandler : IRequestHandler<ApproveIndentCommand, ApiResponse<IndentDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public ApproveIndentHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<IndentDto>> Handle(ApproveIndentCommand request, CancellationToken ct)
    {
        var entity = await _db.Indents
            .Include(i => i.RequestedByEmployee)
            .Include(i => i.Details).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(i => i.Id == request.Id, ct);
        if (entity is null) return ApiResponse<IndentDto>.Failure("Indent not found");
        if (entity.Status != IndentStatus.Pending)
            return ApiResponse<IndentDto>.Failure("Indent cannot be approved in current status");

        entity.Status = IndentStatus.Approved;
        foreach (var detail in entity.Details)
            detail.ApprovedQuantity = detail.RequestedQuantity;
        await _db.SaveChangesAsync(ct);

        return ApiResponse<IndentDto>.Success(_mapper.Map<IndentDto>(entity), "Indent approved");
    }
}

// ══════════════════════════════════════════════════════════════════════
// GATE PASS
// ══════════════════════════════════════════════════════════════════════

public record CreateGatePassCommand(CreateGatePassDto Dto) : IRequest<ApiResponse<GatePassDto>>;

public class CreateGatePassHandler : IRequestHandler<CreateGatePassCommand, ApiResponse<GatePassDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateGatePassHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<GatePassDto>> Handle(CreateGatePassCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var gpType = dto.Type?.Equals("Outward", StringComparison.OrdinalIgnoreCase) == true
            ? GatePassType.Outward : GatePassType.Inward;

        var entity = new GatePass
        {
            GatePassNumber = $"GP-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            GatePassDate = dto.Date,
            GatePassType = gpType,
            PartyName = dto.PersonName,
            VehicleNumber = dto.VehicleNo,
            Purpose = dto.Purpose,
            Description = dto.Description,
            GodownId = dto.GodownId
        };

        _db.GatePasses.Add(entity);
        await _db.SaveChangesAsync(ct);

        return ApiResponse<GatePassDto>.Success(_mapper.Map<GatePassDto>(entity), "Gate pass created");
    }
}

public record ApproveGatePassCommand(int Id) : IRequest<ApiResponse<GatePassDto>>;

public class ApproveGatePassHandler : IRequestHandler<ApproveGatePassCommand, ApiResponse<GatePassDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public ApproveGatePassHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<GatePassDto>> Handle(ApproveGatePassCommand request, CancellationToken ct)
    {
        var entity = await _db.GatePasses.FindAsync(new object[] { request.Id }, ct);
        if (entity is null) return ApiResponse<GatePassDto>.Failure("Gate pass not found");
        if (entity.IsApproved) return ApiResponse<GatePassDto>.Failure("Gate pass already approved");

        entity.IsApproved = true;
        await _db.SaveChangesAsync(ct);

        return ApiResponse<GatePassDto>.Success(_mapper.Map<GatePassDto>(entity), "Gate pass approved");
    }
}

// ══════════════════════════════════════════════════════════════════════
// STOCK DEMAND
// ══════════════════════════════════════════════════════════════════════

public record CreateStockDemandCommand(CreateStockDemandDto Dto) : IRequest<ApiResponse<StockDemandDto>>;

public class CreateStockDemandHandler : IRequestHandler<CreateStockDemandCommand, ApiResponse<StockDemandDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateStockDemandHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<StockDemandDto>> Handle(CreateStockDemandCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var entity = new StockDemand
        {
            DemandNumber = $"SD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            DemandDate = dto.Date,
            GodownId = dto.GodownId,
            JobCardId = dto.JobCardId,
            CostClassId = dto.CostClassId,
            Status = StockDemandStatus.Pending
        };

        _db.StockDemands.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.StockDemands
            .Include(d => d.Godown)
            .FirstAsync(d => d.Id == entity.Id, ct);
        return ApiResponse<StockDemandDto>.Success(_mapper.Map<StockDemandDto>(saved), "Stock demand created");
    }
}

// ══════════════════════════════════════════════════════════════════════
// LANDED COST
// ══════════════════════════════════════════════════════════════════════

public record CreateLandedCostCommand(CreateLandedCostDto Dto) : IRequest<ApiResponse<LandedCostDto>>;

public class CreateLandedCostValidator : AbstractValidator<CreateLandedCostCommand>
{
    public CreateLandedCostValidator()
    {
        RuleFor(x => x.Dto.PurchaseInvoiceId).GreaterThan(0);
        RuleFor(x => x.Dto.Amount).GreaterThan(0);
        RuleFor(x => x.Dto.CostType).NotEmpty();
    }
}

public class CreateLandedCostHandler : IRequestHandler<CreateLandedCostCommand, ApiResponse<LandedCostDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateLandedCostHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<LandedCostDto>> Handle(CreateLandedCostCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var invoice = await _db.PurchaseInvoices
            .Include(p => p.Details)
            .FirstOrDefaultAsync(p => p.Id == dto.PurchaseInvoiceId, ct);
        if (invoice is null) return ApiResponse<LandedCostDto>.Failure("Purchase invoice not found");

        var allocationType = dto.AllocationMethod?.Equals("ByQuantity", StringComparison.OrdinalIgnoreCase) == true
            ? LandedCostAllocationType.ByQuantity
            : LandedCostAllocationType.ByValue;

        var entity = new LandedCost
        {
            PurchaseInvoiceId = dto.PurchaseInvoiceId,
            CostType = dto.CostType,
            Amount = dto.Amount,
            AllocationType = allocationType,
            Description = dto.Description
        };

        // Allocate cost to purchase invoice detail lines
        if (invoice.Details.Any())
        {
            if (allocationType == LandedCostAllocationType.ByQuantity)
            {
                var totalQty = invoice.Details.Sum(d => d.Quantity);
                if (totalQty > 0)
                {
                    foreach (var detail in invoice.Details)
                    {
                        var share = dto.Amount * (detail.Quantity / totalQty);
                        detail.Rate += Math.Round(share / detail.Quantity, 4);
                        detail.Amount = detail.Quantity * detail.Rate;
                    }
                }
            }
            else
            {
                var totalValue = invoice.Details.Sum(d => d.Amount);
                if (totalValue > 0)
                {
                    foreach (var detail in invoice.Details)
                    {
                        var share = dto.Amount * (detail.Amount / totalValue);
                        detail.Rate += Math.Round(share / detail.Quantity, 4);
                        detail.Amount = detail.Quantity * detail.Rate;
                    }
                }
            }
        }

        _db.LandedCosts.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.LandedCosts
            .Include(l => l.PurchaseInvoice)
            .FirstAsync(l => l.Id == entity.Id, ct);
        return ApiResponse<LandedCostDto>.Success(_mapper.Map<LandedCostDto>(saved), "Landed cost created and allocated");
    }
}
