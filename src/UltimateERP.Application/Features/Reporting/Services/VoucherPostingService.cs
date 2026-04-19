using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Features.Reporting.DTOs;
using UltimateERP.Application.Interfaces;
using UltimateERP.Domain.Entities.Account;

namespace UltimateERP.Application.Features.Reporting.Services;

/// <summary>
/// Auto-generates accounting vouchers from inventory transactions (purchase/sales invoices).
/// </summary>
public class VoucherPostingService
{
    private readonly IApplicationDbContext _db;

    public VoucherPostingService(IApplicationDbContext db) => _db = db;

    /// <summary>
    /// Post a purchase invoice to the accounting ledger.
    /// Debit: Purchase A/C, Credit: Vendor (Creditor) A/C
    /// </summary>
    public async Task<Voucher?> PostPurchaseInvoice(int purchaseInvoiceId, int purchaseLedgerId, CancellationToken ct)
    {
        var invoice = await _db.PurchaseInvoices
            .Include(p => p.Vendor)
            .FirstOrDefaultAsync(p => p.Id == purchaseInvoiceId, ct);

        if (invoice is null || invoice.IsPosted) return null;

        var vendorLedgerId = invoice.Vendor.LedgerId;

        var voucher = new Voucher
        {
            VoucherTypeId = 1, // Purchase voucher type
            VoucherNumber = $"PV-{invoice.InvoiceNumber}",
            VoucherDate = invoice.InvoiceDate,
            ReferenceNumber = invoice.ReferenceNumber,
            CommonNarration = $"Purchase from {invoice.Vendor.Name} - {invoice.InvoiceNumber}",
            TotalDebit = invoice.NetAmount,
            TotalCredit = invoice.NetAmount,
            IsPosted = true,
            PostedDate = DateTime.UtcNow,
        };

        voucher.Details.Add(new VoucherDetail
        {
            LineNumber = 1,
            LedgerId = purchaseLedgerId,
            DebitAmount = invoice.NetAmount,
            CreditAmount = 0
        });

        voucher.Details.Add(new VoucherDetail
        {
            LineNumber = 2,
            LedgerId = vendorLedgerId,
            DebitAmount = 0,
            CreditAmount = invoice.NetAmount
        });

        _db.Vouchers.Add(voucher);
        invoice.IsPosted = true;
        invoice.PostedDate = DateTime.UtcNow;
        invoice.VoucherId = voucher.Id;

        await _db.SaveChangesAsync(ct);

        // Update VoucherId after save (EF sets the Id)
        invoice.VoucherId = voucher.Id;
        await _db.SaveChangesAsync(ct);

        return voucher;
    }

    /// <summary>
    /// Post a sales invoice to the accounting ledger.
    /// Debit: Customer (Debtor) A/C, Credit: Sales A/C
    /// </summary>
    public async Task<Voucher?> PostSalesInvoice(int salesInvoiceId, int salesLedgerId, CancellationToken ct)
    {
        var invoice = await _db.SalesInvoices
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.Id == salesInvoiceId, ct);

        if (invoice is null || invoice.IsPosted) return null;

        var customerLedgerId = invoice.Customer.LedgerId;

        var voucher = new Voucher
        {
            VoucherTypeId = 2, // Sales voucher type
            VoucherNumber = $"SV-{invoice.InvoiceNumber}",
            VoucherDate = invoice.InvoiceDate,
            ReferenceNumber = invoice.ReferenceNumber,
            CommonNarration = $"Sales to {invoice.Customer.Name} - {invoice.InvoiceNumber}",
            TotalDebit = invoice.NetAmount,
            TotalCredit = invoice.NetAmount,
            IsPosted = true,
            PostedDate = DateTime.UtcNow,
        };

        voucher.Details.Add(new VoucherDetail
        {
            LineNumber = 1,
            LedgerId = customerLedgerId,
            DebitAmount = invoice.NetAmount,
            CreditAmount = 0
        });

        voucher.Details.Add(new VoucherDetail
        {
            LineNumber = 2,
            LedgerId = salesLedgerId,
            DebitAmount = 0,
            CreditAmount = invoice.NetAmount
        });

        _db.Vouchers.Add(voucher);
        invoice.IsPosted = true;
        invoice.PostedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        invoice.VoucherId = voucher.Id;
        await _db.SaveChangesAsync(ct);

        return voucher;
    }
}
