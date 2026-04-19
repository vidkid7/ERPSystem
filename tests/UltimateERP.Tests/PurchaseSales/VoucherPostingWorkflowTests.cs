using Microsoft.EntityFrameworkCore;
using UltimateERP.Application.Features.Reporting.Services;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Infrastructure.Persistence;

namespace UltimateERP.Tests.PurchaseSales;

/// <summary>
/// Integration tests for VoucherPostingService — cross-module Purchase/Sales → Account workflow.
/// Uses EF Core InMemory database to verify debit/credit entries are generated correctly.
/// </summary>
public class VoucherPostingWorkflowTests : IDisposable
{
    private readonly ERPDbContext _db;
    private readonly VoucherPostingService _service;

    public VoucherPostingWorkflowTests()
    {
        var opts = new DbContextOptionsBuilder<ERPDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new ERPDbContext(opts);
        _service = new VoucherPostingService(_db);
    }

    public void Dispose() => _db.Dispose();

    // ── Helpers ────────────────────────────────────────────────────────────

    private async Task<(Ledger purchaseLedger, Ledger vendorLedger, Vendor vendor)> SeedVendorLedgers()
    {
        var purchaseLedger = new Ledger { Id = 10, Name = "Purchase Account", Code = "PUR", LedgerGroupId = 1, LedgerGroup = new LedgerGroup { Id = 1, Name = "Expenses" } };
        var vendorLedger   = new Ledger { Id = 20, Name = "Vendor Ledger",    Code = "VEN", LedgerGroupId = 2, LedgerGroup = new LedgerGroup { Id = 2, Name = "Creditors" } };
        var vendor         = new Vendor { Id = 1,  Name = "Test Supplier",   LedgerId = vendorLedger.Id, Ledger = vendorLedger };

        _db.LedgerGroups.AddRange(purchaseLedger.LedgerGroup, vendorLedger.LedgerGroup);
        _db.Ledgers.AddRange(purchaseLedger, vendorLedger);
        _db.Vendors.Add(vendor);
        await _db.SaveChangesAsync();
        return (purchaseLedger, vendorLedger, vendor);
    }

    private async Task<(Ledger salesLedger, Ledger customerLedger, Customer customer)> SeedCustomerLedgers()
    {
        var salesLedger    = new Ledger { Id = 30, Name = "Sales Account",    Code = "SAL", LedgerGroupId = 3, LedgerGroup = new LedgerGroup { Id = 3, Name = "Income" } };
        var customerLedger = new Ledger { Id = 40, Name = "Customer Ledger",  Code = "CUS", LedgerGroupId = 4, LedgerGroup = new LedgerGroup { Id = 4, Name = "Debtors" } };
        var customer       = new Customer { Id = 1, Name = "Test Customer",   LedgerId = customerLedger.Id, Ledger = customerLedger };

        _db.LedgerGroups.AddRange(salesLedger.LedgerGroup, customerLedger.LedgerGroup);
        _db.Ledgers.AddRange(salesLedger, customerLedger);
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();
        return (salesLedger, customerLedger, customer);
    }

    // ── Purchase Invoice Posting Tests ─────────────────────────────────────

    [Fact]
    public async Task PostPurchaseInvoice_ValidInvoice_CreatesVoucherWithTwoEntries()
    {
        var (purchaseLedger, vendorLedger, vendor) = await SeedVendorLedgers();

        var invoice = new PurchaseInvoice
        {
            Id = 1,
            InvoiceNumber = "PI-001",
            InvoiceDate = DateTime.Today,
            VendorId = vendor.Id,
            Vendor = vendor,
            NetAmount = 50000m,
            IsPosted = false
        };
        _db.PurchaseInvoices.Add(invoice);
        await _db.SaveChangesAsync();

        var voucher = await _service.PostPurchaseInvoice(1, purchaseLedger.Id, CancellationToken.None);

        Assert.NotNull(voucher);
        Assert.Equal(2, voucher!.Details.Count);
        Assert.Equal(50000m, voucher.TotalDebit);
        Assert.Equal(50000m, voucher.TotalCredit);
    }

    [Fact]
    public async Task PostPurchaseInvoice_ValidInvoice_DebitsPurchaseAccountCreditVendor()
    {
        var (purchaseLedger, vendorLedger, vendor) = await SeedVendorLedgers();

        var invoice = new PurchaseInvoice
        {
            Id = 2,
            InvoiceNumber = "PI-002",
            InvoiceDate = DateTime.Today,
            VendorId = vendor.Id,
            Vendor = vendor,
            NetAmount = 25000m,
            IsPosted = false
        };
        _db.PurchaseInvoices.Add(invoice);
        await _db.SaveChangesAsync();

        var voucher = await _service.PostPurchaseInvoice(2, purchaseLedger.Id, CancellationToken.None);

        Assert.NotNull(voucher);
        var debitLine  = voucher!.Details.Single(d => d.DebitAmount > 0);
        var creditLine = voucher.Details.Single(d => d.CreditAmount > 0);

        Assert.Equal(purchaseLedger.Id, debitLine.LedgerId);
        Assert.Equal(25000m,            debitLine.DebitAmount);
        Assert.Equal(vendorLedger.Id,   creditLine.LedgerId);
        Assert.Equal(25000m,            creditLine.CreditAmount);
    }

    [Fact]
    public async Task PostPurchaseInvoice_ValidInvoice_MarksInvoiceAsPosted()
    {
        var (purchaseLedger, _, vendor) = await SeedVendorLedgers();

        var invoice = new PurchaseInvoice
        {
            Id = 3,
            InvoiceNumber = "PI-003",
            InvoiceDate = DateTime.Today,
            VendorId = vendor.Id,
            Vendor = vendor,
            NetAmount = 10000m,
            IsPosted = false
        };
        _db.PurchaseInvoices.Add(invoice);
        await _db.SaveChangesAsync();

        await _service.PostPurchaseInvoice(3, purchaseLedger.Id, CancellationToken.None);

        var reloaded = await _db.PurchaseInvoices.FindAsync(3);
        Assert.True(reloaded!.IsPosted);
        Assert.NotNull(reloaded.PostedDate);
    }

    [Fact]
    public async Task PostPurchaseInvoice_AlreadyPosted_ReturnsNull()
    {
        var (purchaseLedger, _, vendor) = await SeedVendorLedgers();

        var invoice = new PurchaseInvoice
        {
            Id = 4,
            InvoiceNumber = "PI-004",
            InvoiceDate = DateTime.Today,
            VendorId = vendor.Id,
            Vendor = vendor,
            NetAmount = 10000m,
            IsPosted = true // already posted
        };
        _db.PurchaseInvoices.Add(invoice);
        await _db.SaveChangesAsync();

        var result = await _service.PostPurchaseInvoice(4, purchaseLedger.Id, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task PostPurchaseInvoice_NotFound_ReturnsNull()
    {
        var result = await _service.PostPurchaseInvoice(999, 10, CancellationToken.None);
        Assert.Null(result);
    }

    [Fact]
    public async Task PostPurchaseInvoice_VoucherNumberContainsInvoiceNumber()
    {
        var (purchaseLedger, _, vendor) = await SeedVendorLedgers();

        var invoice = new PurchaseInvoice
        {
            Id = 5,
            InvoiceNumber = "PI-VNUM",
            InvoiceDate = DateTime.Today,
            VendorId = vendor.Id,
            Vendor = vendor,
            NetAmount = 1000m,
            IsPosted = false
        };
        _db.PurchaseInvoices.Add(invoice);
        await _db.SaveChangesAsync();

        var voucher = await _service.PostPurchaseInvoice(5, purchaseLedger.Id, CancellationToken.None);

        Assert.Contains("PI-VNUM", voucher!.VoucherNumber);
    }

    // ── Sales Invoice Posting Tests ─────────────────────────────────────────

    [Fact]
    public async Task PostSalesInvoice_ValidInvoice_CreatesVoucherWithTwoEntries()
    {
        var (salesLedger, customerLedger, customer) = await SeedCustomerLedgers();

        var invoice = new SalesInvoice
        {
            Id = 1,
            InvoiceNumber = "SI-001",
            InvoiceDate = DateTime.Today,
            CustomerId = customer.Id,
            Customer = customer,
            NetAmount = 75000m,
            IsPosted = false
        };
        _db.SalesInvoices.Add(invoice);
        await _db.SaveChangesAsync();

        var voucher = await _service.PostSalesInvoice(1, salesLedger.Id, CancellationToken.None);

        Assert.NotNull(voucher);
        Assert.Equal(2, voucher!.Details.Count);
        Assert.Equal(75000m, voucher.TotalDebit);
        Assert.Equal(75000m, voucher.TotalCredit);
    }

    [Fact]
    public async Task PostSalesInvoice_ValidInvoice_DebitsCustomerCreditsSales()
    {
        var (salesLedger, customerLedger, customer) = await SeedCustomerLedgers();

        var invoice = new SalesInvoice
        {
            Id = 2,
            InvoiceNumber = "SI-002",
            InvoiceDate = DateTime.Today,
            CustomerId = customer.Id,
            Customer = customer,
            NetAmount = 30000m,
            IsPosted = false
        };
        _db.SalesInvoices.Add(invoice);
        await _db.SaveChangesAsync();

        var voucher = await _service.PostSalesInvoice(2, salesLedger.Id, CancellationToken.None);

        Assert.NotNull(voucher);
        var debitLine  = voucher!.Details.Single(d => d.DebitAmount > 0);
        var creditLine = voucher.Details.Single(d => d.CreditAmount > 0);

        Assert.Equal(customerLedger.Id, debitLine.LedgerId);
        Assert.Equal(30000m,            debitLine.DebitAmount);
        Assert.Equal(salesLedger.Id,    creditLine.LedgerId);
        Assert.Equal(30000m,            creditLine.CreditAmount);
    }

    [Fact]
    public async Task PostSalesInvoice_AlreadyPosted_ReturnsNull()
    {
        var (salesLedger, _, customer) = await SeedCustomerLedgers();

        var invoice = new SalesInvoice
        {
            Id = 3,
            InvoiceNumber = "SI-003",
            InvoiceDate = DateTime.Today,
            CustomerId = customer.Id,
            Customer = customer,
            NetAmount = 5000m,
            IsPosted = true
        };
        _db.SalesInvoices.Add(invoice);
        await _db.SaveChangesAsync();

        var result = await _service.PostSalesInvoice(3, salesLedger.Id, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task PostSalesInvoice_NotFound_ReturnsNull()
    {
        var result = await _service.PostSalesInvoice(999, 30, CancellationToken.None);
        Assert.Null(result);
    }

    [Fact]
    public async Task PostSalesInvoice_ValidInvoice_MarksInvoiceAsPosted()
    {
        var (salesLedger, _, customer) = await SeedCustomerLedgers();

        var invoice = new SalesInvoice
        {
            Id = 4,
            InvoiceNumber = "SI-004",
            InvoiceDate = DateTime.Today,
            CustomerId = customer.Id,
            Customer = customer,
            NetAmount = 8000m,
            IsPosted = false
        };
        _db.SalesInvoices.Add(invoice);
        await _db.SaveChangesAsync();

        await _service.PostSalesInvoice(4, salesLedger.Id, CancellationToken.None);

        var reloaded = await _db.SalesInvoices.FindAsync(4);
        Assert.True(reloaded!.IsPosted);
        Assert.NotNull(reloaded.PostedDate);
    }
}
