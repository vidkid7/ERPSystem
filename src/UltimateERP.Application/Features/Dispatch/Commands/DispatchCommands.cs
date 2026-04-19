using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Dispatch.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Dispatch.Commands;

// ── Create Dispatch Order ─────────────────────────────────────────────

public record CreateDispatchOrderCommand(CreateDispatchOrderDto Dto) : IRequest<ApiResponse<DispatchOrderDto>>;

public class CreateDispatchOrderValidator : AbstractValidator<CreateDispatchOrderCommand>
{
    public CreateDispatchOrderValidator()
    {
        RuleFor(x => x.Dto.DispatchNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.CustomerId).GreaterThan(0);
        RuleFor(x => x.Dto.BranchId).GreaterThan(0);
        RuleFor(x => x.Dto.Sections).NotEmpty().WithMessage("At least one section is required");
        RuleForEach(x => x.Dto.Sections).ChildRules(section =>
        {
            section.RuleFor(s => s.ProductId).GreaterThan(0);
            section.RuleFor(s => s.Quantity).GreaterThan(0);
        });
    }
}

public class CreateDispatchOrderHandler : IRequestHandler<CreateDispatchOrderCommand, ApiResponse<DispatchOrderDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateDispatchOrderHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<DispatchOrderDto>> Handle(CreateDispatchOrderCommand request, CancellationToken ct)
    {
        if (await _db.DispatchOrders.AnyAsync(d => d.DispatchNumber == request.Dto.DispatchNumber, ct))
            return ApiResponse<DispatchOrderDto>.Failure($"Dispatch number '{request.Dto.DispatchNumber}' already exists");

        var entity = new DispatchOrder
        {
            DispatchNumber = request.Dto.DispatchNumber,
            DispatchDate = request.Dto.DispatchDate,
            DispatchDateBS = request.Dto.DispatchDateBS,
            CustomerId = request.Dto.CustomerId,
            GodownId = request.Dto.GodownId,
            CostClassId = request.Dto.CostClassId,
            BranchId = request.Dto.BranchId,
            Status = DispatchStatus.Pending,
            Sections = request.Dto.Sections.Select(s => new DispatchSection
            {
                SalesInvoiceId = s.SalesInvoiceId,
                ProductId = s.ProductId,
                Quantity = s.Quantity,
                GodownId = s.GodownId
            }).ToList()
        };

        _db.DispatchOrders.Add(entity);
        await _db.SaveChangesAsync(ct);

        var created = await _db.DispatchOrders
            .Include(d => d.Customer)
            .Include(d => d.Sections).ThenInclude(s => s.Product)
            .FirstAsync(d => d.Id == entity.Id, ct);

        return ApiResponse<DispatchOrderDto>.Success(_mapper.Map<DispatchOrderDto>(created), "Dispatch order created");
    }
}

// ── Approve Dispatch ──────────────────────────────────────────────────

public record ApproveDispatchCommand(ApproveDispatchDto Dto) : IRequest<ApiResponse<DispatchOrderDto>>;

public class ApproveDispatchValidator : AbstractValidator<ApproveDispatchCommand>
{
    public ApproveDispatchValidator()
    {
        RuleFor(x => x.Dto.DispatchOrderId).GreaterThan(0);
    }
}

public class ApproveDispatchHandler : IRequestHandler<ApproveDispatchCommand, ApiResponse<DispatchOrderDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ApproveDispatchHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<DispatchOrderDto>> Handle(ApproveDispatchCommand request, CancellationToken ct)
    {
        var entity = await _db.DispatchOrders
            .Include(d => d.Customer)
            .Include(d => d.Sections).ThenInclude(s => s.Product)
            .FirstOrDefaultAsync(d => d.Id == request.Dto.DispatchOrderId, ct);

        if (entity is null)
            return ApiResponse<DispatchOrderDto>.Failure("Dispatch order not found");

        if (entity.Status != DispatchStatus.Pending)
            return ApiResponse<DispatchOrderDto>.Failure($"Dispatch order is already '{entity.Status}' and cannot be approved");

        entity.Status = DispatchStatus.Dispatched;
        await _db.SaveChangesAsync(ct);

        return ApiResponse<DispatchOrderDto>.Success(_mapper.Map<DispatchOrderDto>(entity), "Dispatch order approved");
    }
}
