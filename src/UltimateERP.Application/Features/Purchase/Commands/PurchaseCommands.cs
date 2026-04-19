using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Common.Models;
using UltimateERP.Application.Features.Purchase.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Application.Features.Purchase.Commands;

public record CreatePurchaseInvoiceCommand(CreatePurchaseInvoiceDto Invoice) : IRequest<ApiResponse<PurchaseInvoiceDto>>;

public class CreatePurchaseInvoiceValidator : AbstractValidator<CreatePurchaseInvoiceCommand>
{
    public CreatePurchaseInvoiceValidator()
    {
        RuleFor(x => x.Invoice.VendorId).GreaterThan(0);
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

public class CreatePurchaseInvoiceHandler : IRequestHandler<CreatePurchaseInvoiceCommand, ApiResponse<PurchaseInvoiceDto>>
{
    private readonly IApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreatePurchaseInvoiceHandler(IApplicationDbContext db, IMapper mapper) { _db = db; _mapper = mapper; }

    public async Task<ApiResponse<PurchaseInvoiceDto>> Handle(CreatePurchaseInvoiceCommand request, CancellationToken ct)
    {
        var dto = request.Invoice;

        var vendor = await _db.Vendors.FindAsync(new object[] { dto.VendorId }, ct);
        if (vendor is null) return ApiResponse<PurchaseInvoiceDto>.Failure("Vendor not found");

        var godown = await _db.Godowns.FindAsync(new object[] { dto.GodownId }, ct);
        if (godown is null) return ApiResponse<PurchaseInvoiceDto>.Failure("Godown not found");

        var invoice = new PurchaseInvoice
        {
            InvoiceNumber = $"PI-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4]}",
            InvoiceDate = dto.InvoiceDate,
            VendorId = dto.VendorId,
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

            invoice.Details.Add(new PurchaseInvoiceDetail
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

            // Update stock
            var stock = await _db.Stocks.FirstOrDefaultAsync(
                s => s.ProductId == detail.ProductId && s.GodownId == dto.GodownId, ct);

            if (stock is not null)
            {
                stock.Quantity += detail.Quantity;
                stock.Rate = detail.Rate;
            }
            else
            {
                _db.Stocks.Add(new Stock
                {
                    ProductId = detail.ProductId,
                    GodownId = dto.GodownId,
                    Quantity = detail.Quantity,
                    Rate = detail.Rate
                });
            }
        }

        invoice.TotalAmount = totalAmount;
        invoice.DiscountAmount = totalDiscount;
        invoice.TaxAmount = totalTax;
        invoice.NetAmount = totalAmount - totalDiscount + totalTax;

        _db.PurchaseInvoices.Add(invoice);
        await _db.SaveChangesAsync(ct);

        var saved = await _db.PurchaseInvoices
            .Include(p => p.Vendor).Include(p => p.Godown)
            .Include(p => p.Details).ThenInclude(d => d.Product)
            .FirstAsync(p => p.Id == invoice.Id, ct);

        return ApiResponse<PurchaseInvoiceDto>.Success(_mapper.Map<PurchaseInvoiceDto>(saved), "Purchase invoice created");
    }
}
