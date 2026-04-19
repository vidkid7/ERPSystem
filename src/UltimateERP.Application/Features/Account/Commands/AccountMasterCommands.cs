using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Account.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Account;

namespace UltimateERP.Application.Features.Account.Commands;

// ── PaymentTerm Commands ─────────────────────────────────────────────

public record CreatePaymentTermCommand(CreatePaymentTermDto Dto) : IRequest<ApiResponse<PaymentTermDto>>;

public class CreatePaymentTermValidator : AbstractValidator<CreatePaymentTermCommand>
{
    public CreatePaymentTermValidator()
    {
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.DueDays).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Dto.DiscountPercent).InclusiveBetween(0, 100);
    }
}

public class CreatePaymentTermHandler : IRequestHandler<CreatePaymentTermCommand, ApiResponse<PaymentTermDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreatePaymentTermHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PaymentTermDto>> Handle(CreatePaymentTermCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        if (!string.IsNullOrEmpty(dto.Code) && await _db.PaymentTerms.AnyAsync(p => p.Code == dto.Code, ct))
            return ApiResponse<PaymentTermDto>.Failure($"Payment term code '{dto.Code}' already exists");

        var entity = _mapper.Map<PaymentTerm>(dto);
        _db.PaymentTerms.Add(entity);
        await _db.SaveChangesAsync(ct);

        return ApiResponse<PaymentTermDto>.Success(_mapper.Map<PaymentTermDto>(entity), "Payment term created");
    }
}

public record UpdatePaymentTermCommand(UpdatePaymentTermDto Dto) : IRequest<ApiResponse<PaymentTermDto>>;

public class UpdatePaymentTermHandler : IRequestHandler<UpdatePaymentTermCommand, ApiResponse<PaymentTermDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public UpdatePaymentTermHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PaymentTermDto>> Handle(UpdatePaymentTermCommand request, CancellationToken ct)
    {
        var entity = await _db.PaymentTerms.FindAsync(new object[] { request.Dto.Id }, ct);
        if (entity is null) return ApiResponse<PaymentTermDto>.Failure("Payment term not found");

        entity.Code = request.Dto.Code;
        entity.Name = request.Dto.Name;
        entity.DueDays = request.Dto.DueDays;
        entity.DiscountPercent = request.Dto.DiscountPercent;
        entity.Description = request.Dto.Description;
        entity.ModifiedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<PaymentTermDto>.Success(_mapper.Map<PaymentTermDto>(entity), "Payment term updated");
    }
}

public record DeletePaymentTermCommand(int Id) : IRequest<ApiResponse<bool>>;

public class DeletePaymentTermHandler : IRequestHandler<DeletePaymentTermCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _db;

    public DeletePaymentTermHandler(IApplicationDbContext db) { _db = db; }

    public async Task<ApiResponse<bool>> Handle(DeletePaymentTermCommand request, CancellationToken ct)
    {
        var entity = await _db.PaymentTerms.FindAsync(new object[] { request.Id }, ct);
        if (entity is null) return ApiResponse<bool>.Failure("Payment term not found");

        entity.IsDeleted = true;
        entity.ModifiedDate = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<bool>.Success(true, "Payment term deleted");
    }
}

// ── PaymentMode Commands ─────────────────────────────────────────────

public record CreatePaymentModeCommand(CreatePaymentModeDto Dto) : IRequest<ApiResponse<PaymentModeDto>>;

public class CreatePaymentModeValidator : AbstractValidator<CreatePaymentModeCommand>
{
    public CreatePaymentModeValidator()
    {
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreatePaymentModeHandler : IRequestHandler<CreatePaymentModeCommand, ApiResponse<PaymentModeDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreatePaymentModeHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PaymentModeDto>> Handle(CreatePaymentModeCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        if (!string.IsNullOrEmpty(dto.Code) && await _db.PaymentModes.AnyAsync(p => p.Code == dto.Code, ct))
            return ApiResponse<PaymentModeDto>.Failure($"Payment mode code '{dto.Code}' already exists");

        var entity = _mapper.Map<PaymentMode>(dto);
        _db.PaymentModes.Add(entity);
        await _db.SaveChangesAsync(ct);

        return ApiResponse<PaymentModeDto>.Success(_mapper.Map<PaymentModeDto>(entity), "Payment mode created");
    }
}

public record UpdatePaymentModeCommand(UpdatePaymentModeDto Dto) : IRequest<ApiResponse<PaymentModeDto>>;

public class UpdatePaymentModeHandler : IRequestHandler<UpdatePaymentModeCommand, ApiResponse<PaymentModeDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public UpdatePaymentModeHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PaymentModeDto>> Handle(UpdatePaymentModeCommand request, CancellationToken ct)
    {
        var entity = await _db.PaymentModes.FindAsync(new object[] { request.Dto.Id }, ct);
        if (entity is null) return ApiResponse<PaymentModeDto>.Failure("Payment mode not found");

        entity.Code = request.Dto.Code;
        entity.Name = request.Dto.Name;
        entity.IsActive = request.Dto.IsActive;
        entity.ModifiedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return ApiResponse<PaymentModeDto>.Success(_mapper.Map<PaymentModeDto>(entity), "Payment mode updated");
    }
}

public record DeletePaymentModeCommand(int Id) : IRequest<ApiResponse<bool>>;

public class DeletePaymentModeHandler : IRequestHandler<DeletePaymentModeCommand, ApiResponse<bool>>
{
    private readonly IApplicationDbContext _db;

    public DeletePaymentModeHandler(IApplicationDbContext db) { _db = db; }

    public async Task<ApiResponse<bool>> Handle(DeletePaymentModeCommand request, CancellationToken ct)
    {
        var entity = await _db.PaymentModes.FindAsync(new object[] { request.Id }, ct);
        if (entity is null) return ApiResponse<bool>.Failure("Payment mode not found");

        entity.IsDeleted = true;
        entity.ModifiedDate = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<bool>.Success(true, "Payment mode deleted");
    }
}
