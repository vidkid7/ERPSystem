using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Loyalty.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Loyalty;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Loyalty.Commands;

// Accrue Points
public record AccruePointsCommand(AccruePointsDto Accrual) : IRequest<ApiResponse<MembershipPointDto>>;

public class AccruePointsValidator : AbstractValidator<AccruePointsCommand>
{
    public AccruePointsValidator()
    {
        RuleFor(x => x.Accrual.CustomerId).GreaterThan(0);
        RuleFor(x => x.Accrual.Points).GreaterThan(0);
    }
}

public class AccruePointsHandler : IRequestHandler<AccruePointsCommand, ApiResponse<MembershipPointDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public AccruePointsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<MembershipPointDto>> Handle(AccruePointsCommand request, CancellationToken ct)
    {
        var dto = request.Accrual;
        var lastEntry = await _db.MembershipPoints
            .Where(m => m.CustomerId == dto.CustomerId)
            .OrderByDescending(m => m.TransactionDate)
            .FirstOrDefaultAsync(ct);

        var currentBalance = lastEntry?.Balance ?? 0;

        var point = new MembershipPoint
        {
            CustomerId = dto.CustomerId,
            TransactionDate = DateTime.UtcNow,
            TransactionType = LoyaltyTransactionType.Earn,
            Points = dto.Points,
            SalesInvoiceId = dto.SalesInvoiceId,
            Balance = currentBalance + dto.Points
        };

        _db.MembershipPoints.Add(point);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<MembershipPointDto>.Success(_mapper.Map<MembershipPointDto>(point), "Points accrued");
    }
}

// Redeem Points
public record RedeemPointsCommand(RedeemPointsDto Redemption) : IRequest<ApiResponse<MembershipPointDto>>;

public class RedeemPointsValidator : AbstractValidator<RedeemPointsCommand>
{
    public RedeemPointsValidator()
    {
        RuleFor(x => x.Redemption.CustomerId).GreaterThan(0);
        RuleFor(x => x.Redemption.Points).GreaterThan(0);
    }
}

public class RedeemPointsHandler : IRequestHandler<RedeemPointsCommand, ApiResponse<MembershipPointDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public RedeemPointsHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<MembershipPointDto>> Handle(RedeemPointsCommand request, CancellationToken ct)
    {
        var dto = request.Redemption;
        var lastEntry = await _db.MembershipPoints
            .Where(m => m.CustomerId == dto.CustomerId)
            .OrderByDescending(m => m.TransactionDate)
            .FirstOrDefaultAsync(ct);

        var currentBalance = lastEntry?.Balance ?? 0;
        if (currentBalance < dto.Points)
            return ApiResponse<MembershipPointDto>.Failure($"Insufficient points. Available: {currentBalance}");

        var point = new MembershipPoint
        {
            CustomerId = dto.CustomerId,
            TransactionDate = DateTime.UtcNow,
            TransactionType = LoyaltyTransactionType.Redeem,
            Points = dto.Points,
            Balance = currentBalance - dto.Points
        };

        _db.MembershipPoints.Add(point);
        await _db.SaveChangesAsync(ct);
        return ApiResponse<MembershipPointDto>.Success(_mapper.Map<MembershipPointDto>(point), "Points redeemed");
    }
}
