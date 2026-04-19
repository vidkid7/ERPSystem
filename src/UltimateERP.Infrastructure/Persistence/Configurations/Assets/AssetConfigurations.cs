using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimateERP.Domain.Entities.Assets;

namespace UltimateERP.Infrastructure.Persistence.Configurations.Assets;

public class AssetGroupConfiguration : IEntityTypeConfiguration<AssetGroup>
{
    public void Configure(EntityTypeBuilder<AssetGroup> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(50);
        builder.Property(e => e.Description).HasMaxLength(500);
    }
}

public class AssetTypeConfiguration : IEntityTypeConfiguration<AssetType>
{
    public void Configure(EntityTypeBuilder<AssetType> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(50);
        builder.Property(e => e.DepreciationRate).HasPrecision(8, 4);

        builder.HasOne(e => e.AssetGroup)
            .WithMany(e => e.AssetTypes)
            .HasForeignKey(e => e.AssetGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class AssetModelConfiguration : IEntityTypeConfiguration<AssetModel>
{
    public void Configure(EntityTypeBuilder<AssetModel> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(50);
        builder.Property(e => e.Manufacturer).HasMaxLength(200);
        builder.Property(e => e.Specifications).HasMaxLength(1000);

        builder.HasOne(e => e.AssetType)
            .WithMany(e => e.AssetModels)
            .HasForeignKey(e => e.AssetTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.AssetCode).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(200);
        builder.Property(e => e.PurchaseCost).HasPrecision(18, 4);
        builder.Property(e => e.CurrentValue).HasPrecision(18, 4);
        builder.Property(e => e.Location).HasMaxLength(300);
        builder.Property(e => e.SerialNumber).HasMaxLength(100);
        builder.Property(e => e.Notes).HasMaxLength(1000);

        builder.HasOne(e => e.AssetModel)
            .WithMany(e => e.Assets)
            .HasForeignKey(e => e.AssetModelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.AssetCategory)
            .WithMany(e => e.Assets)
            .HasForeignKey(e => e.AssetCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.AssignedToEmployee)
            .WithMany()
            .HasForeignKey(e => e.AssignedToEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class AssetCategoryConfiguration : IEntityTypeConfiguration<AssetCategory>
{
    public void Configure(EntityTypeBuilder<AssetCategory> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(50);

        builder.HasOne(e => e.ParentCategory)
            .WithMany(e => e.ChildCategories)
            .HasForeignKey(e => e.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class AssetTransactionConfiguration : IEntityTypeConfiguration<AssetTransaction>
{
    public void Configure(EntityTypeBuilder<AssetTransaction> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.DocumentNo).HasMaxLength(50);
        builder.Property(e => e.Remarks).HasMaxLength(500);
        builder.Property(e => e.Amount).HasPrecision(18, 4);

        builder.HasOne(e => e.Asset)
            .WithMany(e => e.AssetTransactions)
            .HasForeignKey(e => e.AssetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.FromEmployee)
            .WithMany()
            .HasForeignKey(e => e.FromEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ToEmployee)
            .WithMany()
            .HasForeignKey(e => e.ToEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
