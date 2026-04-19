using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Inventory.Services;
using UltimateERP.Application.Features.Sales.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Application.Features.Sales.Commands;

public record CreateSalesInvoiceCommand(CreateSalesInvoiceDto Invoice) : IRequest<ApiResponse<SalesInvoiceDto>>;

public class CreateSalesInvoiceValidator : AbstractValidator<CreateSalesInvoiceCommand>
{
    public CreateSalesInvoiceValidator()
    {
        RuleFor(x => x.Invoice.CustomerId).GreaterThan(0);
        RuleFor(x => x.Invoice.GodownId).GreaterThan(0);
        RuleFor(x => x.Invoice.Details).NotEmpty().WithMessage("Invoice must have at least one detail line");
        RuleForEach(x => x.Invoice.Details).ChildRules(d =>
        {
            d.RuleFor(x => x.ProductId).GreaterThan(0);
            d.RuleFor(x => x.Quantity).GreaterThan(0);
            d.RuleFor(x => x.Rate).GreaterThanOrEqualTo(0);
        });
    }
}

public class CreateSalesInvoiceHandler : IRequestHandler<CreateSalesInvoiceCommand, ApiResponse<SalesInvoiceDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateSalesInvoiceHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<SalesInvoiceDto>> Handle(CreateSalesInvoiceCommand request, CancellationToken ct)
    {
        var dto = request.Invoice;

        var customer = await _db.Customers.FindAsync(new object[] { dto.CustomerId }, ct);
        if (customer is null) return ApiResponse<SalesInvoiceDto>.Failure("Customer not found");

        // Check stock availability for all items
        foreach (var detail in dto.Details)
        {
            var stock = await _db.Stocks.FirstOrDefaultAsync(
                s => s.ProductId == detail.ProductId && s.GodownId == dto.GodownId, ct);
            var currentQty = stock?.Quantity ?? 0;

            if (!StockCostingService.HasSufficientStock(currentQty, detail.Quantity))
                return ApiResponse<SalesInvoiceDto>.Failure(
                    $"Insufficient stock for product ID {detail.ProductId}. Available: {currentQty}, Required: {detail.Quantity}");
        }

        var invoice = new SalesInvoice
        {
            InvoiceNumber = $"SI-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            InvoiceDate = dto.InvoiceDate,
            CustomerId = dto.CustomerId,
            GodownId = dto.GodownId,
            ReferenceNumber = dto.ReferenceNumber,
        };

        decimal totalAmount = 0, totalDiscount = 0, totalTax = 0;
        int lineNum = 1;

        foreach (var detail in dto.Details)
        {
            var amount = detail.Quantity * detail.Rate;
            var discount = amount * (detail.DiscountPercent ?? 0) / 100;
            var taxable = amount - discount;
            var tax = taxable * (detail.TaxPercent ?? 0) / 100;

            invoice.Details.Add(new SalesInvoiceDetail
            {
                LineNumber = lineNum++,
                ProductId = detail.ProductId,
                Quantity = detail.Quantity,
                Rate = detail.Rate,
                Amount = amount,
                DiscountPercent = detail.DiscountPercent ?? 0,
                DiscountAmount = discount,
                TaxPercent = detail.TaxPercent ?? 0,
                TaxAmount = tax,
                NetAmount = taxable + tax
            });

            totalAmount += amount;
            totalDiscount += discount;
            totalTax += tax;

            // Reduce stock
            var stock = await _db.Stocks.FirstOrDefaultAsync(
                s => s.ProductId == detail.ProductId && s.GodownId == dto.GodownId, ct);
            if (stock is not null)
                stock.Quantity -= detail.Quantity;
        }

        invoice.TotalAmount = totalAmount;
        invoice.DiscountAmount = totalDiscount;
        invoice.TaxAmount = totalTax;
        invoice.NetAmount = totalAmount - totalDiscount + totalTax;

        _db.SalesInvoices.Add(invoice);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.SalesInvoices
            .Include(s => s.Customer).Include(s => s.Godown)
            .Include(s => s.Details).ThenInclude(d => d.Product)
            .FirstAsync(s => s.Id == invoice.Id, ct);

        return ApiResponse<SalesInvoiceDto>.Success(_mapper.Map<SalesInvoiceDto>(saved), "Sales invoice created");
    }
}
