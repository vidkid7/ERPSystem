using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimateERP.Domain.Entities.Account;

namespace UltimateERP.Infrastructure.Persistence.Configurations.Account;

public class LedgerGroupConfiguration : IEntityTypeConfiguration<LedgerGroup>
{
    public void Configure(EntityTypeBuilder<LedgerGroup> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Code).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(50);
        builder.Property(e => e.TypeOfGroup).HasMaxLength(100);
        builder.Property(e => e.Prefix).HasMaxLength(20);
        builder.Property(e => e.Suffix).HasMaxLength(20);

        builder.HasOne(e => e.ParentGroup)
            .WithMany(e => e.ChildGroups)
            .HasForeignKey(e => e.ParentGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class LedgerConfiguration : IEntityTypeConfiguration<Ledger>
{
    public void Configure(EntityTypeBuilder<Ledger> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Code).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(50);
        builder.Property(e => e.OpeningBalance).HasPrecision(18, 4);
        builder.Property(e => e.DebitAmount).HasPrecision(18, 4);
        builder.Property(e => e.CreditAmount).HasPrecision(18, 4);
        builder.Property(e => e.ClosingBalance).HasPrecision(18, 4);
        builder.Property(e => e.CreditLimit).HasPrecision(18, 4);
        builder.Property(e => e.PANNumber).HasMaxLength(50);
        builder.Property(e => e.VATNumber).HasMaxLength(50);
        builder.Property(e => e.Phone).HasMaxLength(50);
        builder.Property(e => e.Email).HasMaxLength(200);

        builder.HasOne(e => e.LedgerGroup)
            .WithMany(e => e.Ledgers)
            .HasForeignKey(e => e.LedgerGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.VoucherNumber, e.VoucherTypeId }).IsUnique();
        builder.HasIndex(e => e.VoucherDate);
        builder.Property(e => e.VoucherNumber).HasMaxLength(50).IsRequired();
        builder.Property(e => e.VoucherDateBS).HasMaxLength(20);
        builder.Property(e => e.ReferenceNumber).HasMaxLength(100);
        builder.Property(e => e.CommonNarration).HasMaxLength(500);
        builder.Property(e => e.CancelReason).HasMaxLength(500);
        builder.Property(e => e.TotalDebit).HasPrecision(18, 4);
        builder.Property(e => e.TotalCredit).HasPrecision(18, 4);

        builder.HasOne(e => e.CostClass)
            .WithMany()
            .HasForeignKey(e => e.CostClassId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class VoucherDetailConfiguration : IEntityTypeConfiguration<VoucherDetail>
{
    public void Configure(EntityTypeBuilder<VoucherDetail> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.DebitAmount).HasPrecision(18, 4);
        builder.Property(e => e.CreditAmount).HasPrecision(18, 4);
        builder.Property(e => e.ExchangeRate).HasPrecision(18, 6);
        builder.Property(e => e.Narration).HasMaxLength(500);

        builder.HasOne(e => e.Voucher)
            .WithMany(e => e.Details)
            .HasForeignKey(e => e.VoucherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Ledger)
            .WithMany(e => e.VoucherDetails)
            .HasForeignKey(e => e.LedgerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Code).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(200);
        builder.Property(e => e.Code).HasMaxLength(50);
        builder.Property(e => e.CreditLimit).HasPrecision(18, 4);
        builder.Property(e => e.Phone).HasMaxLength(50);
        builder.Property(e => e.Email).HasMaxLength(200);

        builder.HasOne(e => e.Ledger)
            .WithOne(e => e.Customer)
            .HasForeignKey<Customer>(e => e.LedgerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Code).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(200);
        builder.Property(e => e.Code).HasMaxLength(50);
        builder.Property(e => e.PANNumber).HasMaxLength(50);
        builder.Property(e => e.VATNumber).HasMaxLength(50);
        builder.Property(e => e.Phone).HasMaxLength(50);
        builder.Property(e => e.Email).HasMaxLength(200);

        builder.HasOne(e => e.Ledger)
            .WithOne(e => e.Vendor)
            .HasForeignKey<Vendor>(e => e.LedgerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PDCConfiguration : IEntityTypeConfiguration<PDC>
{
    public void Configure(EntityTypeBuilder<PDC> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ChequeNumber).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Amount).HasPrecision(18, 4);
        builder.Property(e => e.BankName).HasMaxLength(200);
        builder.HasIndex(e => e.ChequeDate);

        builder.HasOne(e => e.Voucher)
            .WithMany(e => e.PDCs)
            .HasForeignKey(e => e.VoucherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Ledger)
            .WithMany()
            .HasForeignKey(e => e.LedgerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
