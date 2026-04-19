using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Sales.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Domain.Enums;

namespace UltimateERP.Application.Features.Sales.Commands;

// ══════════════════════════════════════════════════════════════════════
// SALES QUOTATION
// ══════════════════════════════════════════════════════════════════════

public record CreateSalesQuotationCommand(CreateSalesQuotationDto Dto) : IRequest<ApiResponse<SalesQuotationDto>>;

public class CreateSalesQuotationValidator : AbstractValidator<CreateSalesQuotationCommand>
{
    public CreateSalesQuotationValidator()
    {
        RuleFor(x => x.Dto.CustomerId).GreaterThan(0);
        RuleFor(x => x.Dto.Items).NotEmpty();
        RuleForEach(x => x.Dto.Items).ChildRules(d =>
        {
            d.RuleFor(x => x.ProductId).GreaterThan(0);
            d.RuleFor(x => x.Qty).GreaterThan(0);
            d.RuleFor(x => x.Rate).GreaterThanOrEqualTo(0);
        });
    }
}

public class CreateSalesQuotationHandler : IRequestHandler<CreateSalesQuotationCommand, ApiResponse<SalesQuotationDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateSalesQuotationHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SalesQuotationDto>> Handle(CreateSalesQuotationCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var customer = await _db.Customers.FindAsync(new object[] { dto.CustomerId }, ct);
        if (customer is null) return ApiResponse<SalesQuotationDto>.Failure("Customer not found");

        var entity = new SalesQuotation
        {
            QuotationNumber = $"SQ-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            QuotationDate = dto.Date,
            CustomerId = dto.CustomerId,
            GodownId = dto.GodownId,
            ValidUntil = dto.ValidUntil,
            Remarks = dto.Remarks,
            Status = SalesQuotationStatus.Draft
        };

        decimal totalAmount = 0, totalDiscount = 0, totalTax = 0;
        int lineNum = 1;
        foreach (var item in dto.Items)
        {
            var amount = item.Qty * item.Rate;
            var discount = amount * (item.DiscountPercent ?? 0) / 100;
            var taxable = amount - discount;
            var tax = taxable * (item.TaxPercent ?? 0) / 100;

            entity.Details.Add(new SalesQuotationDetail
            {
                LineNumber = lineNum++,
                ProductId = item.ProductId,
                Quantity = item.Qty,
                Rate = item.Rate,
                Amount = amount,
                DiscountPercent = item.DiscountPercent ?? 0,
                DiscountAmount = discount,
                TaxPercent = item.TaxPercent ?? 0,
                TaxAmount = tax,
                NetAmount = taxable + tax
            });
            totalAmount += amount;
            totalDiscount += discount;
            totalTax += tax;
        }

        entity.TotalAmount = totalAmount;
        entity.DiscountAmount = totalDiscount;
        entity.TaxAmount = totalTax;
        entity.NetAmount = totalAmount - totalDiscount + totalTax;

        _db.SalesQuotations.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.SalesQuotations
            .Include(q => q.Customer)
            .Include(q => q.Details).ThenInclude(d => d.Product)
            .FirstAsync(q => q.Id == entity.Id, ct);
        return ApiResponse<SalesQuotationDto>.Success(_mapper.Map<SalesQuotationDto>(saved), "Sales quotation created");
    }
}

public record ApproveSalesQuotationCommand(int Id) : IRequest<ApiResponse<SalesQuotationDto>>;

public class ApproveSalesQuotationHandler : IRequestHandler<ApproveSalesQuotationCommand, ApiResponse<SalesQuotationDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public ApproveSalesQuotationHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SalesQuotationDto>> Handle(ApproveSalesQuotationCommand request, CancellationToken ct)
    {
        var entity = await _db.SalesQuotations
            .Include(q => q.Customer)
            .Include(q => q.Details).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(q => q.Id == request.Id, ct);
        if (entity is null) return ApiResponse<SalesQuotationDto>.Failure("Quotation not found");
        if (entity.Status != SalesQuotationStatus.Draft && entity.Status != SalesQuotationStatus.Pending)
            return ApiResponse<SalesQuotationDto>.Failure("Quotation cannot be approved in current status");

        entity.Status = SalesQuotationStatus.Approved;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<SalesQuotationDto>.Success(_mapper.Map<SalesQuotationDto>(entity), "Quotation approved");
    }
}

// ══════════════════════════════════════════════════════════════════════
// SALES ORDER
// ══════════════════════════════════════════════════════════════════════

public record CreateSalesOrderCommand(CreateSalesOrderDto Dto) : IRequest<ApiResponse<SalesOrderDto>>;

public class CreateSalesOrderValidator : AbstractValidator<CreateSalesOrderCommand>
{
    public CreateSalesOrderValidator()
    {
        RuleFor(x => x.Dto.CustomerId).GreaterThan(0);
        RuleFor(x => x.Dto.Items).NotEmpty();
        RuleForEach(x => x.Dto.Items).ChildRules(d =>
        {
            d.RuleFor(x => x.ProductId).GreaterThan(0);
            d.RuleFor(x => x.Qty).GreaterThan(0);
            d.RuleFor(x => x.Rate).GreaterThanOrEqualTo(0);
        });
    }
}

public class CreateSalesOrderHandler : IRequestHandler<CreateSalesOrderCommand, ApiResponse<SalesOrderDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateSalesOrderHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SalesOrderDto>> Handle(CreateSalesOrderCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var customer = await _db.Customers.FindAsync(new object[] { dto.CustomerId }, ct);
        if (customer is null) return ApiResponse<SalesOrderDto>.Failure("Customer not found");

        // If referencing a quotation, mark it as converted
        if (dto.SalesQuotationId.HasValue)
        {
            var quotation = await _db.SalesQuotations.FindAsync(new object[] { dto.SalesQuotationId.Value }, ct);
            if (quotation is null) return ApiResponse<SalesOrderDto>.Failure("Referenced quotation not found");
            if (quotation.Status == SalesQuotationStatus.Approved)
                quotation.Status = SalesQuotationStatus.Converted;
        }

        var entity = new SalesOrder
        {
            OrderNumber = $"SO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            OrderDate = dto.Date,
            CustomerId = dto.CustomerId,
            SalesQuotationId = dto.SalesQuotationId,
            GodownId = dto.GodownId,
            ExpectedDeliveryDate = dto.ExpectedDeliveryDate,
            Remarks = dto.Remarks,
            Status = SalesOrderStatus.Draft
        };

        decimal totalAmount = 0, totalDiscount = 0, totalTax = 0;
        int lineNum = 1;
        foreach (var item in dto.Items)
        {
            var amount = item.Qty * item.Rate;
            var discount = amount * (item.DiscountPercent ?? 0) / 100;
            var taxable = amount - discount;
            var tax = taxable * (item.TaxPercent ?? 0) / 100;

            entity.Details.Add(new SalesOrderDetail
            {
                LineNumber = lineNum++,
                ProductId = item.ProductId,
                Quantity = item.Qty,
                Rate = item.Rate,
                Amount = amount,
                DiscountPercent = item.DiscountPercent ?? 0,
                DiscountAmount = discount,
                TaxPercent = item.TaxPercent ?? 0,
                TaxAmount = tax,
                NetAmount = taxable + tax
            });
            totalAmount += amount;
            totalDiscount += discount;
            totalTax += tax;
        }

        entity.TotalAmount = totalAmount;
        entity.DiscountAmount = totalDiscount;
        entity.TaxAmount = totalTax;
        entity.NetAmount = totalAmount - totalDiscount + totalTax;

        _db.SalesOrders.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.SalesOrders
            .Include(o => o.Customer)
            .Include(o => o.Details).ThenInclude(d => d.Product)
            .FirstAsync(o => o.Id == entity.Id, ct);
        return ApiResponse<SalesOrderDto>.Success(_mapper.Map<SalesOrderDto>(saved), "Sales order created");
    }
}

public record ApproveSalesOrderCommand(int Id) : IRequest<ApiResponse<SalesOrderDto>>;

public class ApproveSalesOrderHandler : IRequestHandler<ApproveSalesOrderCommand, ApiResponse<SalesOrderDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public ApproveSalesOrderHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SalesOrderDto>> Handle(ApproveSalesOrderCommand request, CancellationToken ct)
    {
        var entity = await _db.SalesOrders
            .Include(o => o.Customer)
            .Include(o => o.Details).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == request.Id, ct);
        if (entity is null) return ApiResponse<SalesOrderDto>.Failure("Order not found");
        if (entity.Status != SalesOrderStatus.Draft && entity.Status != SalesOrderStatus.Pending)
            return ApiResponse<SalesOrderDto>.Failure("Order cannot be approved in current status");

        entity.Status = SalesOrderStatus.Approved;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<SalesOrderDto>.Success(_mapper.Map<SalesOrderDto>(entity), "Order approved");
    }
}

public record CancelSalesOrderCommand(int Id) : IRequest<ApiResponse<SalesOrderDto>>;

public class CancelSalesOrderHandler : IRequestHandler<CancelSalesOrderCommand, ApiResponse<SalesOrderDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CancelSalesOrderHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SalesOrderDto>> Handle(CancelSalesOrderCommand request, CancellationToken ct)
    {
        var entity = await _db.SalesOrders
            .Include(o => o.Customer)
            .Include(o => o.Details).ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.Id == request.Id, ct);
        if (entity is null) return ApiResponse<SalesOrderDto>.Failure("Order not found");
        if (entity.Status == SalesOrderStatus.Delivered)
            return ApiResponse<SalesOrderDto>.Failure("Fully delivered order cannot be cancelled");

        entity.Status = SalesOrderStatus.Cancelled;
        await _db.SaveChangesAsync(ct);
        return ApiResponse<SalesOrderDto>.Success(_mapper.Map<SalesOrderDto>(entity), "Order cancelled");
    }
}

// ══════════════════════════════════════════════════════════════════════
// SALES ALLOTMENT
// ══════════════════════════════════════════════════════════════════════

public record CreateSalesAllotmentCommand(CreateSalesAllotmentDto Dto) : IRequest<ApiResponse<SalesAllotmentDto>>;

public class CreateSalesAllotmentHandler : IRequestHandler<CreateSalesAllotmentCommand, ApiResponse<SalesAllotmentDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateSalesAllotmentHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SalesAllotmentDto>> Handle(CreateSalesAllotmentCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var customer = await _db.Customers.FindAsync(new object[] { dto.CustomerId }, ct);
        if (customer is null) return ApiResponse<SalesAllotmentDto>.Failure("Customer not found");

        var entity = new SalesAllotment
        {
            AllotmentNumber = $"SA-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            AllotmentDate = dto.Date,
            CustomerId = dto.CustomerId,
            ProductId = dto.ProductId,
            AllottedQuantity = dto.AllocatedQty,
            GodownId = dto.GodownId,
            Status = SalesAllotmentStatus.Pending
        };

        _db.SalesAllotments.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.SalesAllotments
            .Include(a => a.Customer)
            .Include(a => a.Product)
            .FirstAsync(a => a.Id == entity.Id, ct);
        return ApiResponse<SalesAllotmentDto>.Success(_mapper.Map<SalesAllotmentDto>(saved), "Allotment created");
    }
}

// ══════════════════════════════════════════════════════════════════════
// SALES DELIVERY NOTE (uses DispatchOrder)
// ══════════════════════════════════════════════════════════════════════

public record CreateSalesDeliveryNoteCommand(CreateSalesDeliveryNoteDto Dto) : IRequest<ApiResponse<SalesDeliveryNoteDto>>;

public class CreateSalesDeliveryNoteHandler : IRequestHandler<CreateSalesDeliveryNoteCommand, ApiResponse<SalesDeliveryNoteDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateSalesDeliveryNoteHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SalesDeliveryNoteDto>> Handle(CreateSalesDeliveryNoteCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var customer = await _db.Customers.FindAsync(new object[] { dto.CustomerId }, ct);
        if (customer is null) return ApiResponse<SalesDeliveryNoteDto>.Failure("Customer not found");

        var dispatch = new DispatchOrder
        {
            DispatchNumber = $"DN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            DispatchDate = dto.Date,
            CustomerId = dto.CustomerId,
            GodownId = dto.GodownId,
            Status = DispatchStatus.Pending
        };

        foreach (var item in dto.Items)
        {
            dispatch.Sections.Add(new DispatchSection
            {
                SalesInvoiceId = dto.SalesInvoiceId,
                ProductId = item.ProductId,
                Quantity = item.Qty,
                GodownId = item.GodownId ?? dto.GodownId
            });
        }

        _db.DispatchOrders.Add(dispatch);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.DispatchOrders
            .Include(d => d.Customer)
            .Include(d => d.Sections).ThenInclude(s => s.Product)
            .FirstAsync(d => d.Id == dispatch.Id, ct);
        return ApiResponse<SalesDeliveryNoteDto>.Success(_mapper.Map<SalesDeliveryNoteDto>(saved), "Delivery note created");
    }
}

// ══════════════════════════════════════════════════════════════════════
// SALES RETURN
// ══════════════════════════════════════════════════════════════════════

public record CreateSalesReturnCommand(CreateSalesReturnDto Dto) : IRequest<ApiResponse<SalesReturnDto>>;

public class CreateSalesReturnValidator : AbstractValidator<CreateSalesReturnCommand>
{
    public CreateSalesReturnValidator()
    {
        RuleFor(x => x.Dto.CustomerId).GreaterThan(0);
        RuleFor(x => x.Dto.Items).NotEmpty();
        RuleForEach(x => x.Dto.Items).ChildRules(d =>
        {
            d.RuleFor(x => x.ProductId).GreaterThan(0);
            d.RuleFor(x => x.Qty).GreaterThan(0);
        });
    }
}

public class CreateSalesReturnHandler : IRequestHandler<CreateSalesReturnCommand, ApiResponse<SalesReturnDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateSalesReturnHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SalesReturnDto>> Handle(CreateSalesReturnCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var customer = await _db.Customers.FindAsync(new object[] { dto.CustomerId }, ct);
        if (customer is null) return ApiResponse<SalesReturnDto>.Failure("Customer not found");

        var entity = new SalesReturn
        {
            ReturnNumber = $"SR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            ReturnDate = dto.Date,
            CustomerId = dto.CustomerId,
            SalesInvoiceId = dto.SalesInvoiceId,
            GodownId = dto.GodownId,
            Reason = dto.Reason,
            Status = SalesReturnStatus.Draft
        };

        decimal totalAmount = 0, totalTax = 0;
        int lineNum = 1;
        foreach (var item in dto.Items)
        {
            var amount = item.Qty * item.Rate;
            var tax = amount * (item.TaxPercent ?? 0) / 100;

            entity.Details.Add(new SalesReturnDetail
            {
                LineNumber = lineNum++,
                ProductId = item.ProductId,
                Quantity = item.Qty,
                Rate = item.Rate,
                Amount = amount,
                TaxPercent = item.TaxPercent ?? 0,
                TaxAmount = tax,
                NetAmount = amount + tax,
                Reason = item.Reason
            });
            totalAmount += amount;
            totalTax += tax;

            // Add stock back
            var godownId = dto.GodownId ?? 0;
            if (godownId > 0)
            {
                var stock = await _db.Stocks.FirstOrDefaultAsync(
                    s => s.ProductId == item.ProductId && s.GodownId == godownId, ct);
                if (stock is not null)
                    stock.Quantity += item.Qty;
            }
        }

        entity.TotalAmount = totalAmount;
        entity.TaxAmount = totalTax;
        entity.NetAmount = totalAmount + totalTax;

        _db.SalesReturns.Add(entity);

        // Auto-create credit note for the return
        var creditNote = new SalesCreditNote
        {
            NoteNumber = $"SCN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            NoteDate = dto.Date,
            CustomerId = dto.CustomerId,
            SalesInvoiceId = dto.SalesInvoiceId,
            Amount = totalAmount,
            TaxAmount = totalTax,
            NetAmount = totalAmount + totalTax,
            Reason = $"Auto-generated from sales return",
            Status = DebitCreditNoteStatus.Draft
        };
        _db.SalesCreditNotes.Add(creditNote);

        await _db.SaveChangesAsync(ct);

        // Link credit note to return after save
        creditNote.SalesReturnId = entity.Id;
        await _db.SaveChangesAsync(ct);

        var saved = await _db.SalesReturns
            .Include(r => r.Customer)
            .Include(r => r.Details).ThenInclude(d => d.Product)
            .FirstAsync(r => r.Id == entity.Id, ct);
        return ApiResponse<SalesReturnDto>.Success(_mapper.Map<SalesReturnDto>(saved), "Sales return created with credit note");
    }
}

// ══════════════════════════════════════════════════════════════════════
// SALES DEBIT NOTE
// ══════════════════════════════════════════════════════════════════════

public record CreateSalesDebitNoteCommand(CreateSalesDebitNoteDto Dto) : IRequest<ApiResponse<SalesDebitNoteDto>>;

public class CreateSalesDebitNoteHandler : IRequestHandler<CreateSalesDebitNoteCommand, ApiResponse<SalesDebitNoteDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateSalesDebitNoteHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SalesDebitNoteDto>> Handle(CreateSalesDebitNoteCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var customer = await _db.Customers.FindAsync(new object[] { dto.CustomerId }, ct);
        if (customer is null) return ApiResponse<SalesDebitNoteDto>.Failure("Customer not found");

        var entity = new SalesDebitNote
        {
            NoteNumber = $"SDN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            NoteDate = dto.Date,
            CustomerId = dto.CustomerId,
            SalesInvoiceId = dto.SalesInvoiceId,
            Amount = dto.Amount,
            TaxAmount = dto.TaxAmount ?? 0,
            NetAmount = dto.Amount + (dto.TaxAmount ?? 0),
            Reason = dto.Reason,
            Status = DebitCreditNoteStatus.Draft
        };

        _db.SalesDebitNotes.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.SalesDebitNotes
            .Include(n => n.Customer)
            .FirstAsync(n => n.Id == entity.Id, ct);
        return ApiResponse<SalesDebitNoteDto>.Success(_mapper.Map<SalesDebitNoteDto>(saved), "Debit note created");
    }
}

// ══════════════════════════════════════════════════════════════════════
// SALES CREDIT NOTE
// ══════════════════════════════════════════════════════════════════════

public record CreateSalesCreditNoteCommand(CreateSalesCreditNoteDto Dto) : IRequest<ApiResponse<SalesCreditNoteDto>>;

public class CreateSalesCreditNoteHandler : IRequestHandler<CreateSalesCreditNoteCommand, ApiResponse<SalesCreditNoteDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;
    public CreateSalesCreditNoteHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SalesCreditNoteDto>> Handle(CreateSalesCreditNoteCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        var customer = await _db.Customers.FindAsync(new object[] { dto.CustomerId }, ct);
        if (customer is null) return ApiResponse<SalesCreditNoteDto>.Failure("Customer not found");

        var entity = new SalesCreditNote
        {
            NoteNumber = $"SCN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            NoteDate = dto.Date,
            CustomerId = dto.CustomerId,
            SalesInvoiceId = dto.SalesInvoiceId,
            SalesReturnId = dto.SalesReturnId,
            Amount = dto.Amount,
            TaxAmount = dto.TaxAmount ?? 0,
            NetAmount = dto.Amount + (dto.TaxAmount ?? 0),
            Reason = dto.Reason,
            Status = DebitCreditNoteStatus.Draft
        };

        _db.SalesCreditNotes.Add(entity);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.SalesCreditNotes
            .Include(n => n.Customer)
            .FirstAsync(n => n.Id == entity.Id, ct);
        return ApiResponse<SalesCreditNoteDto>.Success(_mapper.Map<SalesCreditNoteDto>(saved), "Credit note created");
    }
}
