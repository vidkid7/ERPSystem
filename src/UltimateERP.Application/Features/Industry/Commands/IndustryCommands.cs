using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Industry.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.IndustrySpecific;

namespace UltimateERP.Application.Features.Industry.Commands;

// ── Create Dairy Purchase ─────────────────────────────────────────────

public record CreateDairyPurchaseCommand(CreateDairyPurchaseDto Dto) : IRequest<ApiResponse<DairyPurchaseInvoiceDto>>;

public class CreateDairyPurchaseValidator : AbstractValidator<CreateDairyPurchaseCommand>
{
    public CreateDairyPurchaseValidator()
    {
        RuleFor(x => x.Dto.InvoiceNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.VendorId).GreaterThan(0);
        RuleFor(x => x.Dto.BranchId).GreaterThan(0);
        RuleFor(x => x.Dto.TotalQuantityLitre).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Dto.NetAmount).GreaterThanOrEqualTo(0);
    }
}

public class CreateDairyPurchaseHandler : IRequestHandler<CreateDairyPurchaseCommand, ApiResponse<DairyPurchaseInvoiceDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateDairyPurchaseHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<DairyPurchaseInvoiceDto>> Handle(CreateDairyPurchaseCommand request, CancellationToken ct)
    {
        if (await _db.DairyPurchaseInvoices.AnyAsync(d => d.InvoiceNumber == request.Dto.InvoiceNumber, ct))
            return ApiResponse<DairyPurchaseInvoiceDto>.Failure($"Dairy purchase invoice '{request.Dto.InvoiceNumber}' already exists");

        var entity = _mapper.Map<DairyPurchaseInvoice>(request.Dto);
        _db.DairyPurchaseInvoices.Add(entity);
        await _db.SaveChangesAsync(ct);

        var created = await _db.DairyPurchaseInvoices
            .Include(d => d.Vendor)
            .FirstAsync(d => d.Id == entity.Id, ct);

        return ApiResponse<DairyPurchaseInvoiceDto>.Success(_mapper.Map<DairyPurchaseInvoiceDto>(created), "Dairy purchase invoice created");
    }
}

// ── Create Tea Purchase ───────────────────────────────────────────────

public record CreateTeaPurchaseCommand(CreateTeaPurchaseDto Dto) : IRequest<ApiResponse<TeaPurchaseInvoiceDto>>;

public class CreateTeaPurchaseValidator : AbstractValidator<CreateTeaPurchaseCommand>
{
    public CreateTeaPurchaseValidator()
    {
        RuleFor(x => x.Dto.InvoiceNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.VendorId).GreaterThan(0);
        RuleFor(x => x.Dto.BranchId).GreaterThan(0);
        RuleFor(x => x.Dto.Quantity).GreaterThan(0);
        RuleFor(x => x.Dto.Rate).GreaterThanOrEqualTo(0);
    }
}

public class CreateTeaPurchaseHandler : IRequestHandler<CreateTeaPurchaseCommand, ApiResponse<TeaPurchaseInvoiceDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateTeaPurchaseHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<TeaPurchaseInvoiceDto>> Handle(CreateTeaPurchaseCommand request, CancellationToken ct)
    {
        if (await _db.TeaPurchaseInvoices.AnyAsync(t => t.InvoiceNumber == request.Dto.InvoiceNumber, ct))
            return ApiResponse<TeaPurchaseInvoiceDto>.Failure($"Tea purchase invoice '{request.Dto.InvoiceNumber}' already exists");

        var entity = _mapper.Map<TeaPurchaseInvoice>(request.Dto);
        _db.TeaPurchaseInvoices.Add(entity);
        await _db.SaveChangesAsync(ct);

        var created = await _db.TeaPurchaseInvoices
            .Include(t => t.Vendor)
            .FirstAsync(t => t.Id == entity.Id, ct);

        return ApiResponse<TeaPurchaseInvoiceDto>.Success(_mapper.Map<TeaPurchaseInvoiceDto>(created), "Tea purchase invoice created");
    }
}

// ── Create Petrol Pump Transaction ────────────────────────────────────

public record CreatePetrolPumpTransactionCommand(CreatePetrolPumpTransactionDto Dto) : IRequest<ApiResponse<PetrolPumpTransactionDto>>;

public class CreatePetrolPumpTransactionValidator : AbstractValidator<CreatePetrolPumpTransactionCommand>
{
    public CreatePetrolPumpTransactionValidator()
    {
        RuleFor(x => x.Dto.TransactionNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.ProductId).GreaterThan(0);
        RuleFor(x => x.Dto.BranchId).GreaterThan(0);
        RuleFor(x => x.Dto.QuantityDispensed).GreaterThan(0);
        RuleFor(x => x.Dto.Rate).GreaterThanOrEqualTo(0);
    }
}

public class CreatePetrolPumpTransactionHandler : IRequestHandler<CreatePetrolPumpTransactionCommand, ApiResponse<PetrolPumpTransactionDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreatePetrolPumpTransactionHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PetrolPumpTransactionDto>> Handle(CreatePetrolPumpTransactionCommand request, CancellationToken ct)
    {
        if (await _db.PetrolPumpTransactions.AnyAsync(t => t.TransactionNumber == request.Dto.TransactionNumber, ct))
            return ApiResponse<PetrolPumpTransactionDto>.Failure($"Petrol pump transaction '{request.Dto.TransactionNumber}' already exists");

        var entity = _mapper.Map<PetrolPumpTransaction>(request.Dto);
        _db.PetrolPumpTransactions.Add(entity);
        await _db.SaveChangesAsync(ct);

        var created = await _db.PetrolPumpTransactions
            .Include(t => t.Product)
            .Include(t => t.Customer)
            .FirstAsync(t => t.Id == entity.Id, ct);

        return ApiResponse<PetrolPumpTransactionDto>.Success(_mapper.Map<PetrolPumpTransactionDto>(created), "Petrol pump transaction created");
    }
}
