using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimateERP.Domain.Entities.Inventory;

namespace UltimateERP.Infrastructure.Persistence.Configurations.Inventory;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Code).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(300).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(50);
        builder.Property(e => e.HSNCode).HasMaxLength(20);
        builder.Property(e => e.StandardCost).HasPrecision(18, 4);
        builder.Property(e => e.MRP).HasPrecision(18, 4);
        builder.Property(e => e.WholesaleRate).HasPrecision(18, 4);
        builder.Property(e => e.RetailRate).HasPrecision(18, 4);
        builder.Property(e => e.OpeningStock).HasPrecision(18, 4);
        builder.Property(e => e.OpeningValue).HasPrecision(18, 4);
        builder.Property(e => e.ReorderLevel).HasPrecision(18, 4);
        builder.Property(e => e.MaximumLevel).HasPrecision(18, 4);
        builder.Property(e => e.TaxRate).HasPrecision(8, 4);

        builder.HasOne(e => e.ProductGroup)
            .WithMany(e => e.Products)
            .HasForeignKey(e => e.ProductGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ProductGroupConfiguration : IEntityTypeConfiguration<ProductGroup>
{
    public void Configure(EntityTypeBuilder<ProductGroup> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Code).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(50);

        builder.HasOne(e => e.ParentGroup)
            .WithMany(e => e.ChildGroups)
            .HasForeignKey(e => e.ParentGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class GodownConfiguration : IEntityTypeConfiguration<Godown>
{
    public void Configure(EntityTypeBuilder<Godown> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Code).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(50);
        builder.Property(e => e.GodownType).HasMaxLength(50);

        builder.HasOne(e => e.ParentGodown)
            .WithMany(e => e.ChildGodowns)
            .HasForeignKey(e => e.ParentGodownId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.ProductId, e.GodownId });
        builder.Property(e => e.BatchNumber).HasMaxLength(100);
        builder.Property(e => e.Quantity).HasPrecision(18, 4);
        builder.Property(e => e.Rate).HasPrecision(18, 4);
        builder.Property(e => e.Value).HasPrecision(18, 4);

        builder.HasOne(e => e.Product)
            .WithMany(e => e.Stocks)
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Godown)
            .WithMany(e => e.Stocks)
            .HasForeignKey(e => e.GodownId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Rack)
            .WithMany()
            .HasForeignKey(e => e.RackId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PurchaseInvoiceConfiguration : IEntityTypeConfiguration<PurchaseInvoice>
{
    public void Configure(EntityTypeBuilder<PurchaseInvoice> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.InvoiceNumber);
        builder.HasIndex(e => e.InvoiceDate);
        builder.Property(e => e.InvoiceNumber).HasMaxLength(50).IsRequired();
        builder.Property(e => e.InvoiceDateBS).HasMaxLength(20);
        builder.Property(e => e.ReferenceNumber).HasMaxLength(100);
        builder.Property(e => e.TotalAmount).HasPrecision(18, 4);
        builder.Property(e => e.DiscountAmount).HasPrecision(18, 4);
        builder.Property(e => e.TaxAmount).HasPrecision(18, 4);
        builder.Property(e => e.NetAmount).HasPrecision(18, 4);

        builder.HasOne(e => e.Vendor)
            .WithMany()
            .HasForeignKey(e => e.VendorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PurchaseInvoiceDetailConfiguration : IEntityTypeConfiguration<PurchaseInvoiceDetail>
{
    public void Configure(EntityTypeBuilder<PurchaseInvoiceDetail> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Quantity).HasPrecision(18, 4);
        builder.Property(e => e.Rate).HasPrecision(18, 4);
        builder.Property(e => e.Amount).HasPrecision(18, 4);
        builder.Property(e => e.DiscountPercent).HasPrecision(8, 4);
        builder.Property(e => e.DiscountAmount).HasPrecision(18, 4);
        builder.Property(e => e.TaxPercent).HasPrecision(8, 4);
        builder.Property(e => e.TaxAmount).HasPrecision(18, 4);
        builder.Property(e => e.NetAmount).HasPrecision(18, 4);
        builder.Property(e => e.BatchNumber).HasMaxLength(100);

        builder.HasOne(e => e.PurchaseInvoice)
            .WithMany(e => e.Details)
            .HasForeignKey(e => e.PurchaseInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Product)
            .WithMany()
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class SalesInvoiceConfiguration : IEntityTypeConfiguration<SalesInvoice>
{
    public void Configure(EntityTypeBuilder<SalesInvoice> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.InvoiceNumber);
        builder.HasIndex(e => e.InvoiceDate);
        builder.Property(e => e.InvoiceNumber).HasMaxLength(50).IsRequired();
        builder.Property(e => e.InvoiceDateBS).HasMaxLength(20);
        builder.Property(e => e.ReferenceNumber).HasMaxLength(100);
        builder.Property(e => e.TotalAmount).HasPrecision(18, 4);
        builder.Property(e => e.DiscountAmount).HasPrecision(18, 4);
        builder.Property(e => e.TaxAmount).HasPrecision(18, 4);
        builder.Property(e => e.NetAmount).HasPrecision(18, 4);

        builder.HasOne(e => e.Customer)
            .WithMany()
            .HasForeignKey(e => e.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class SalesInvoiceDetailConfiguration : IEntityTypeConfiguration<SalesInvoiceDetail>
{
    public void Configure(EntityTypeBuilder<SalesInvoiceDetail> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Quantity).HasPrecision(18, 4);
        builder.Property(e => e.Rate).HasPrecision(18, 4);
        builder.Property(e => e.Amount).HasPrecision(18, 4);
        builder.Property(e => e.DiscountPercent).HasPrecision(8, 4);
        builder.Property(e => e.DiscountAmount).HasPrecision(18, 4);
        builder.Property(e => e.TaxPercent).HasPrecision(8, 4);
        builder.Property(e => e.TaxAmount).HasPrecision(18, 4);
        builder.Property(e => e.NetAmount).HasPrecision(18, 4);
        builder.Property(e => e.CostAmount).HasPrecision(18, 4);
        builder.Property(e => e.ProfitAmount).HasPrecision(18, 4);
        builder.Property(e => e.BatchNumber).HasMaxLength(100);

        builder.HasOne(e => e.SalesInvoice)
            .WithMany(e => e.Details)
            .HasForeignKey(e => e.SalesInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Product)
            .WithMany()
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class StockTransferConfiguration : IEntityTypeConfiguration<StockTransfer>
{
    public void Configure(EntityTypeBuilder<StockTransfer> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.TransferNumber);
        builder.Property(e => e.TransferNumber).HasMaxLength(50).IsRequired();
        builder.Property(e => e.TotalQuantity).HasPrecision(18, 4);
        builder.Property(e => e.TotalValue).HasPrecision(18, 4);

        builder.HasOne(e => e.FromGodown)
            .WithMany()
            .HasForeignKey(e => e.FromGodownId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ToGodown)
            .WithMany()
            .HasForeignKey(e => e.ToGodownId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class BOMConfiguration : IEntityTypeConfiguration<BOM>
{
    public void Configure(EntityTypeBuilder<BOM> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.TotalCost).HasPrecision(18, 4);

        builder.HasOne(e => e.Product)
            .WithMany()
            .HasForeignKey(e => e.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class BOMDetailConfiguration : IEntityTypeConfiguration<BOMDetail>
{
    public void Configure(EntityTypeBuilder<BOMDetail> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Quantity).HasPrecision(18, 4);
        builder.Property(e => e.Rate).HasPrecision(18, 4);
        builder.Property(e => e.Amount).HasPrecision(18, 4);

        builder.HasOne(e => e.BOM)
            .WithMany(e => e.Details)
            .HasForeignKey(e => e.BOMId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.ComponentProduct)
            .WithMany()
            .HasForeignKey(e => e.ComponentProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
