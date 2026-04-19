using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Purchase.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Purchase.Commands;

// ── Purchase Quotation ───────────────────────────────────────────────

public record CreatePurchaseQuotationCommand(CreatePurchaseQuotationDto Dto)
    : IRequest<ApiResponse<PurchaseQuotationDto>>;

public class CreatePurchaseQuotationValidator : AbstractValidator<CreatePurchaseQuotationCommand>
{
    public CreatePurchaseQuotationValidator()
    {
        RuleFor(x => x.Dto.VendorId).GreaterThan(0);
        RuleFor(x => x.Dto.Items).NotEmpty().WithMessage("Quotation must have at least one item");
        RuleForEach(x => x.Dto.Items).ChildRules(i =>
        {
            i.RuleFor(x => x.ProductId).GreaterThan(0);
            i.RuleFor(x => x.Qty).GreaterThan(0);
            i.RuleFor(x => x.Rate).GreaterThanOrEqualTo(0);
        });
    }
}

public class CreatePurchaseQuotationHandler : IRequestHandler<CreatePurchaseQuotationCommand, ApiResponse<PurchaseQuotationDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreatePurchaseQuotationHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PurchaseQuotationDto>> Handle(CreatePurchaseQuotationCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        var vendor = await _db.Vendors.FindAsync(new object[] { dto.VendorId }, ct);
        if (vendor is null) return ApiResponse<PurchaseQuotationDto>.Failure("Vendor not found");

        var entity = new PurchaseQuotation
        {
            QuotationNumber = $"PQ-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            QuotationDate = dto.Date,
            VendorId = dto.VendorId,
            ValidUntil = dto.ValidUntil,
            Remarks = dto.Remarks,
            Status = PurchaseDocumentStatus.Draft
        };

        decimal total = 0;
        int lineNum = 1;
        foreach (var item in dto.Items)
        {
            var amount = item.Qty * item.Rate;
            entity.Details.Add(new PurchaseQuotationDetail
            {
                LineNumber = lineNum++,
                ProductId = item.ProductId,
                Quantity = item.Qty,
                Rate = item.Rate,
                Amount = amount
            });
            total += amount;
        }
        entity.TotalAmount = total;

        _db.PurchaseQuotations.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.PurchaseQuotations
            .Include(q => q.Vendor).Include(q => q.Details).ThenInclude(d => d.Product)
            .FirstAsync(q => q.Id == entity.Id, ct);

        return ApiResponse<PurchaseQuotationDto>.Success(_mapper.Map<PurchaseQuotationDto>(saved), "Purchase quotation created");
    }
}

public record ApprovePurchaseQuotationCommand(int Id) : IRequest<ApiResponse<PurchaseQuotationDto>>;

public class ApprovePurchaseQuotationHandler : IRequestHandler<ApprovePurchaseQuotationCommand, ApiResponse<PurchaseQuotationDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ApprovePurchaseQuotationHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PurchaseQuotationDto>> Handle(ApprovePurchaseQuotationCommand request, CancellationToken ct)
    {
        var entity = await _db.PurchaseQuotations
            .Include(q => q.Vendor).Include(q => q.Details).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(q => q.Id == request.Id, ct);

        if (entity is null) return ApiResponse<PurchaseQuotationDto>.Failure("Purchase quotation not found");
        if (entity.Status != PurchaseDocumentStatus.Draft && entity.Status != PurchaseDocumentStatus.Pending)
            return ApiResponse<PurchaseQuotationDto>.Failure("Only draft or pending quotations can be approved");

        entity.Status = PurchaseDocumentStatus.Approved;
        entity.ModifiedDate = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);

        return ApiResponse<PurchaseQuotationDto>.Success(_mapper.Map<PurchaseQuotationDto>(entity), "Purchase quotation approved");
    }
}

// ── Purchase Order ───────────────────────────────────────────────────

public record CreatePurchaseOrderCommand(CreatePurchaseOrderDto Dto)
    : IRequest<ApiResponse<PurchaseOrderDto>>;

public class CreatePurchaseOrderValidator : AbstractValidator<CreatePurchaseOrderCommand>
{
    public CreatePurchaseOrderValidator()
    {
        RuleFor(x => x.Dto.VendorId).GreaterThan(0);
        RuleFor(x => x.Dto.Items).NotEmpty().WithMessage("Order must have at least one item");
        RuleForEach(x => x.Dto.Items).ChildRules(i =>
        {
            i.RuleFor(x => x.ProductId).GreaterThan(0);
            i.RuleFor(x => x.Qty).GreaterThan(0);
            i.RuleFor(x => x.Rate).GreaterThanOrEqualTo(0);
        });
    }
}

public class CreatePurchaseOrderHandler : IRequestHandler<CreatePurchaseOrderCommand, ApiResponse<PurchaseOrderDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreatePurchaseOrderHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PurchaseOrderDto>> Handle(CreatePurchaseOrderCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        var vendor = await _db.Vendors.FindAsync(new object[] { dto.VendorId }, ct);
        if (vendor is null) return ApiResponse<PurchaseOrderDto>.Failure("Vendor not found");

        if (dto.QuotationRef.HasValue)
        {
            var quotation = await _db.PurchaseQuotations.FindAsync(new object[] { dto.QuotationRef.Value }, ct);
            if (quotation is null) return ApiResponse<PurchaseOrderDto>.Failure("Referenced quotation not found");
        }

        var entity = new PurchaseOrder
        {
            OrderNumber = $"PO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            OrderDate = dto.Date,
            VendorId = dto.VendorId,
            PurchaseQuotationId = dto.QuotationRef,
            ExpectedDeliveryDate = dto.ExpectedDeliveryDate,
            Remarks = dto.Remarks,
            Status = PurchaseDocumentStatus.Draft
        };

        decimal total = 0;
        int lineNum = 1;
        foreach (var item in dto.Items)
        {
            var amount = item.Qty * item.Rate;
            entity.Details.Add(new PurchaseOrderDetail
            {
                LineNumber = lineNum++,
                ProductId = item.ProductId,
                Quantity = item.Qty,
                Rate = item.Rate,
                Amount = amount
            });
            total += amount;
        }
        entity.TotalAmount = total;

        _db.PurchaseOrders.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.PurchaseOrders
            .Include(o => o.Vendor).Include(o => o.Details).ThenInclude(d => d.Product)
            .FirstAsync(o => o.Id == entity.Id, ct);

        return ApiResponse<PurchaseOrderDto>.Success(_mapper.Map<PurchaseOrderDto>(saved), "Purchase order created");
    }
}

public record ApprovePurchaseOrderCommand(int Id) : IRequest<ApiResponse<PurchaseOrderDto>>;

public class ApprovePurchaseOrderHandler : IRequestHandler<ApprovePurchaseOrderCommand, ApiResponse<PurchaseOrderDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ApprovePurchaseOrderHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PurchaseOrderDto>> Handle(ApprovePurchaseOrderCommand request, CancellationToken ct)
    {
        var entity = await _db.PurchaseOrders
            .Include(o => o.Vendor).Include(o => o.Details).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == request.Id, ct);

        if (entity is null) return ApiResponse<PurchaseOrderDto>.Failure("Purchase order not found");
        if (entity.Status != PurchaseDocumentStatus.Draft && entity.Status != PurchaseDocumentStatus.Pending)
            return ApiResponse<PurchaseOrderDto>.Failure("Only draft or pending orders can be approved");

        entity.Status = PurchaseDocumentStatus.Approved;
        entity.ModifiedDate = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);

        return ApiResponse<PurchaseOrderDto>.Success(_mapper.Map<PurchaseOrderDto>(entity), "Purchase order approved");
    }
}

// ── Receipt Note (GRN) ──────────────────────────────────────────────

public record CreateReceiptNoteCommand(CreateReceiptNoteDto Dto)
    : IRequest<ApiResponse<ReceiptNoteDto>>;

public class CreateReceiptNoteValidator : AbstractValidator<CreateReceiptNoteCommand>
{
    public CreateReceiptNoteValidator()
    {
        RuleFor(x => x.Dto.VendorId).GreaterThan(0);
        RuleFor(x => x.Dto.Items).NotEmpty().WithMessage("Receipt note must have at least one item");
        RuleForEach(x => x.Dto.Items).ChildRules(i =>
        {
            i.RuleFor(x => x.ProductId).GreaterThan(0);
            i.RuleFor(x => x.ReceivedQty).GreaterThan(0);
        });
    }
}

public class CreateReceiptNoteHandler : IRequestHandler<CreateReceiptNoteCommand, ApiResponse<ReceiptNoteDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateReceiptNoteHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<ReceiptNoteDto>> Handle(CreateReceiptNoteCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        var vendor = await _db.Vendors.FindAsync(new object[] { dto.VendorId }, ct);
        if (vendor is null) return ApiResponse<ReceiptNoteDto>.Failure("Vendor not found");

        var entity = new ReceiptNote
        {
            GRNNumber = $"GRN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            ReceiptDate = dto.Date,
            VendorId = dto.VendorId,
            PurchaseOrderId = dto.PurchaseOrderRef,
            GodownId = dto.GodownId,
            Remarks = dto.Remarks,
            Status = PurchaseDocumentStatus.Approved // GRN is approved on creation
        };

        int lineNum = 1;
        foreach (var item in dto.Items)
        {
            entity.Details.Add(new ReceiptNoteDetail
            {
                LineNumber = lineNum++,
                ProductId = item.ProductId,
                ReceivedQuantity = item.ReceivedQty,
                AcceptedQuantity = item.AcceptedQty,
                RejectedQuantity = item.RejectedQty,
                Rate = item.Rate
            });

            // Update stock with accepted quantity
            if (item.AcceptedQty > 0 && dto.GodownId.HasValue)
            {
                var stock = await _db.Stocks.FirstOrDefaultAsync(
                    s => s.ProductId == item.ProductId && s.GodownId == dto.GodownId.Value, ct);

                if (stock is not null)
                {
                    stock.Quantity += item.AcceptedQty;
                    stock.Rate = item.Rate;
                }
                else
                {
                    _db.Stocks.Add(new Stock
                    {
                        ProductId = item.ProductId,
                        GodownId = dto.GodownId.Value,
                        Quantity = item.AcceptedQty,
                        Rate = item.Rate
                    });
                }
            }

            // Update PO received quantities
            if (dto.PurchaseOrderRef.HasValue)
            {
                var poDetail = await _db.PurchaseOrderDetails
                    .FirstOrDefaultAsync(d => d.PurchaseOrderId == dto.PurchaseOrderRef.Value && d.ProductId == item.ProductId, ct);
                if (poDetail is not null)
                    poDetail.ReceivedQuantity += item.AcceptedQty;
            }
        }

        _db.ReceiptNotes.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.ReceiptNotes
            .Include(r => r.Vendor).Include(r => r.Details).ThenInclude(d => d.Product)
            .FirstAsync(r => r.Id == entity.Id, ct);

        return ApiResponse<ReceiptNoteDto>.Success(_mapper.Map<ReceiptNoteDto>(saved), "Receipt note (GRN) created");
    }
}

// ── Purchase Return ──────────────────────────────────────────────────

public record CreatePurchaseReturnCommand(CreatePurchaseReturnDto Dto)
    : IRequest<ApiResponse<PurchaseReturnDto>>;

public class CreatePurchaseReturnValidator : AbstractValidator<CreatePurchaseReturnCommand>
{
    public CreatePurchaseReturnValidator()
    {
        RuleFor(x => x.Dto.VendorId).GreaterThan(0);
        RuleFor(x => x.Dto.Items).NotEmpty().WithMessage("Return must have at least one item");
        RuleForEach(x => x.Dto.Items).ChildRules(i =>
        {
            i.RuleFor(x => x.ProductId).GreaterThan(0);
            i.RuleFor(x => x.ReturnQty).GreaterThan(0);
        });
    }
}

public class CreatePurchaseReturnHandler : IRequestHandler<CreatePurchaseReturnCommand, ApiResponse<PurchaseReturnDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreatePurchaseReturnHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PurchaseReturnDto>> Handle(CreatePurchaseReturnCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        var vendor = await _db.Vendors.FindAsync(new object[] { dto.VendorId }, ct);
        if (vendor is null) return ApiResponse<PurchaseReturnDto>.Failure("Vendor not found");

        var entity = new PurchaseReturn
        {
            ReturnNumber = $"PR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            ReturnDate = dto.Date,
            VendorId = dto.VendorId,
            InvoiceReference = dto.InvoiceRef,
            GodownId = dto.GodownId,
            Remarks = dto.Remarks,
            Status = PurchaseDocumentStatus.Approved
        };

        decimal total = 0;
        int lineNum = 1;
        foreach (var item in dto.Items)
        {
            var amount = item.ReturnQty * item.Rate;
            entity.Details.Add(new PurchaseReturnDetail
            {
                LineNumber = lineNum++,
                ProductId = item.ProductId,
                ReturnQuantity = item.ReturnQty,
                Rate = item.Rate,
                Amount = amount,
                Reason = item.Reason
            });
            total += amount;

            // Reduce stock
            if (dto.GodownId.HasValue)
            {
                var stock = await _db.Stocks.FirstOrDefaultAsync(
                    s => s.ProductId == item.ProductId && s.GodownId == dto.GodownId.Value, ct);
                if (stock is not null)
                    stock.Quantity -= item.ReturnQty;
            }
        }
        entity.TotalAmount = total;

        _db.PurchaseReturns.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.PurchaseReturns
            .Include(r => r.Vendor).Include(r => r.Details).ThenInclude(d => d.Product)
            .FirstAsync(r => r.Id == entity.Id, ct);

        return ApiResponse<PurchaseReturnDto>.Success(_mapper.Map<PurchaseReturnDto>(saved), "Purchase return created");
    }
}

// ── Purchase Debit Note ──────────────────────────────────────────────

public record CreatePurchaseDebitNoteCommand(CreatePurchaseDebitNoteDto Dto)
    : IRequest<ApiResponse<PurchaseDebitNoteDto>>;

public class CreatePurchaseDebitNoteValidator : AbstractValidator<CreatePurchaseDebitNoteCommand>
{
    public CreatePurchaseDebitNoteValidator()
    {
        RuleFor(x => x.Dto.VendorId).GreaterThan(0);
        RuleFor(x => x.Dto.Amount).GreaterThan(0);
    }
}

public class CreatePurchaseDebitNoteHandler : IRequestHandler<CreatePurchaseDebitNoteCommand, ApiResponse<PurchaseDebitNoteDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreatePurchaseDebitNoteHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PurchaseDebitNoteDto>> Handle(CreatePurchaseDebitNoteCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        var vendor = await _db.Vendors.FindAsync(new object[] { dto.VendorId }, ct);
        if (vendor is null) return ApiResponse<PurchaseDebitNoteDto>.Failure("Vendor not found");

        var entity = new PurchaseDebitNote
        {
            NoteNumber = $"PDN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            NoteDate = dto.Date,
            VendorId = dto.VendorId,
            InvoiceReference = dto.InvoiceRef,
            Amount = dto.Amount,
            Reason = dto.Reason,
            Status = PurchaseDocumentStatus.Approved
        };

        _db.PurchaseDebitNotes.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.PurchaseDebitNotes
            .Include(n => n.Vendor)
            .FirstAsync(n => n.Id == entity.Id, ct);

        return ApiResponse<PurchaseDebitNoteDto>.Success(_mapper.Map<PurchaseDebitNoteDto>(saved), "Purchase debit note created");
    }
}

// ── Purchase Credit Note ─────────────────────────────────────────────

public record CreatePurchaseCreditNoteCommand(CreatePurchaseCreditNoteDto Dto)
    : IRequest<ApiResponse<PurchaseCreditNoteDto>>;

public class CreatePurchaseCreditNoteValidator : AbstractValidator<CreatePurchaseCreditNoteCommand>
{
    public CreatePurchaseCreditNoteValidator()
    {
        RuleFor(x => x.Dto.VendorId).GreaterThan(0);
        RuleFor(x => x.Dto.Amount).GreaterThan(0);
    }
}

public class CreatePurchaseCreditNoteHandler : IRequestHandler<CreatePurchaseCreditNoteCommand, ApiResponse<PurchaseCreditNoteDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreatePurchaseCreditNoteHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PurchaseCreditNoteDto>> Handle(CreatePurchaseCreditNoteCommand request, CancellationToken ct)
    {
        var dto = request.Dto;

        var vendor = await _db.Vendors.FindAsync(new object[] { dto.VendorId }, ct);
        if (vendor is null) return ApiResponse<PurchaseCreditNoteDto>.Failure("Vendor not found");

        var entity = new PurchaseCreditNote
        {
            NoteNumber = $"PCN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            NoteDate = dto.Date,
            VendorId = dto.VendorId,
            InvoiceReference = dto.InvoiceRef,
            Amount = dto.Amount,
            Reason = dto.Reason,
            Status = PurchaseDocumentStatus.Approved
        };

        _db.PurchaseCreditNotes.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.PurchaseCreditNotes
            .Include(n => n.Vendor)
            .FirstAsync(n => n.Id == entity.Id, ct);

        return ApiResponse<PurchaseCreditNoteDto>.Success(_mapper.Map<PurchaseCreditNoteDto>(saved), "Purchase credit note created");
    }
}
