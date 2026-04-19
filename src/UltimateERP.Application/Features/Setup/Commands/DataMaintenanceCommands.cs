using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Setup.DTOs;
using UltimateERP.Application.Interfaces;

namespace UltimateERP.Application.Features.Setup.Commands;

public record MergeLedgersCommand(MergeLedgersDto Merge) : IRequest<ApiResponse<MergeResultDto>>;

public class MergeLedgersHandler : IRequestHandler<MergeLedgersCommand, ApiResponse<MergeResultDto>>
{
    private readonly IApplicationDbContext _db;

    public MergeLedgersHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<MergeResultDto>> Handle(MergeLedgersCommand request, CancellationToken ct)
    {
        var source = await _db.Ledgers.FindAsync(new object[] { request.Merge.SourceLedgerId }, ct);
        var target = await _db.Ledgers.FindAsync(new object[] { request.Merge.TargetLedgerId }, ct);

        if (source is null) return ApiResponse<MergeResultDto>.Failure("Source ledger not found");
        if (target is null) return ApiResponse<MergeResultDto>.Failure("Target ledger not found");
        if (source.Id == target.Id) return ApiResponse<MergeResultDto>.Failure("Source and target cannot be the same");

        var details = await _db.VoucherDetails
            .Where(vd => vd.LedgerId == source.Id)
            .ToListAsync(ct);

        foreach (var detail in details)
            detail.LedgerId = target.Id;

        target.OpeningBalance += source.OpeningBalance;
        target.DebitAmount += source.DebitAmount;
        target.CreditAmount += source.CreditAmount;
        target.ClosingBalance += source.ClosingBalance;

        source.IsDeleted = true;
        source.IsActive = false;

        await _db.SaveChangesAsync(ct);

        return ApiResponse<MergeResultDto>.Success(new MergeResultDto
        {
            IsSuccess = true,
            RecordsUpdated = details.Count,
            Message = $"Merged ledger '{source.Name}' into '{target.Name}'. {details.Count} voucher details updated."
        }, "Ledgers merged successfully");
    }
}

public record MergeProductsCommand(MergeProductsDto Merge) : IRequest<ApiResponse<MergeResultDto>>;

public class MergeProductsHandler : IRequestHandler<MergeProductsCommand, ApiResponse<MergeResultDto>>
{
    private readonly IApplicationDbContext _db;

    public MergeProductsHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<MergeResultDto>> Handle(MergeProductsCommand request, CancellationToken ct)
    {
        var source = await _db.Products.FindAsync(new object[] { request.Merge.SourceProductId }, ct);
        var target = await _db.Products.FindAsync(new object[] { request.Merge.TargetProductId }, ct);

        if (source is null) return ApiResponse<MergeResultDto>.Failure("Source product not found");
        if (target is null) return ApiResponse<MergeResultDto>.Failure("Target product not found");
        if (source.Id == target.Id) return ApiResponse<MergeResultDto>.Failure("Source and target cannot be the same");

        var stocks = await _db.Stocks
            .Where(s => s.ProductId == source.Id)
            .ToListAsync(ct);

        int updatedCount = 0;
        foreach (var stock in stocks)
        {
            stock.ProductId = target.Id;
            updatedCount++;
        }

        source.IsDeleted = true;
        source.IsActive = false;

        await _db.SaveChangesAsync(ct);

        return ApiResponse<MergeResultDto>.Success(new MergeResultDto
        {
            IsSuccess = true,
            RecordsUpdated = updatedCount,
            Message = $"Merged product '{source.Name}' into '{target.Name}'. {updatedCount} stock records updated."
        }, "Products merged successfully");
    }
}

public record RenumberVouchersCommand(RenumberVouchersDto Renumber) : IRequest<ApiResponse<MergeResultDto>>;

public class RenumberVouchersHandler : IRequestHandler<RenumberVouchersCommand, ApiResponse<MergeResultDto>>
{
    private readonly IApplicationDbContext _db;

    public RenumberVouchersHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<MergeResultDto>> Handle(RenumberVouchersCommand request, CancellationToken ct)
    {
        var vouchers = await _db.Vouchers
            .Where(v => v.VoucherTypeId == request.Renumber.DocumentTypeId
                        && v.VoucherDate >= request.Renumber.StartDate
                        && v.VoucherDate <= request.Renumber.EndDate
                        && !v.IsCancelled)
            .OrderBy(v => v.VoucherDate)
            .ThenBy(v => v.Id)
            .ToListAsync(ct);

        int number = request.Renumber.StartNumber;
        foreach (var voucher in vouchers)
        {
            voucher.VoucherNumber = number.ToString().PadLeft(6, '0');
            number++;
        }

        await _db.SaveChangesAsync(ct);

        return ApiResponse<MergeResultDto>.Success(new MergeResultDto
        {
            IsSuccess = true,
            RecordsUpdated = vouchers.Count,
            Message = $"Renumbered {vouchers.Count} vouchers from {request.Renumber.StartNumber}"
        }, "Vouchers renumbered successfully");
    }
}
