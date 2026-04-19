using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimateERP.Domain.Entities.Account;
using UltimateERP.Domain.Entities.Inventory;
using UltimateERP.Domain.Entities.HR;
using UltimateERP.Domain.Entities.HMS;

namespace UltimateERP.Infrastructure.Persistence.Configurations;

public class VoucherIndexConfiguration : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.HasIndex(e => e.VoucherDate);
        builder.HasIndex(e => e.VoucherTypeId);
    }
}

public class PurchaseInvoiceIndexConfiguration : IEntityTypeConfiguration<PurchaseInvoice>
{
    public void Configure(EntityTypeBuilder<PurchaseInvoice> builder)
    {
        builder.HasIndex(e => e.InvoiceDate);
    }
}

public class SalesInvoiceIndexConfiguration : IEntityTypeConfiguration<SalesInvoice>
{
    public void Configure(EntityTypeBuilder<SalesInvoice> builder)
    {
        builder.HasIndex(e => e.InvoiceDate);
    }
}

public class ProductIndexConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasIndex(e => e.Code);
    }
}

public class LedgerIndexConfiguration : IEntityTypeConfiguration<Ledger>
{
    public void Configure(EntityTypeBuilder<Ledger> builder)
    {
        builder.HasIndex(e => e.Code);
    }
}

public class CustomerIndexConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasIndex(e => e.Code);
    }
}

public class StockIndexConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasIndex(e => new { e.ProductId, e.GodownId });
    }
}

public class EmployeeIndexConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasIndex(e => e.Email);
    }
}

public class PatientIndexConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasIndex(e => e.Phone);
    }
}
