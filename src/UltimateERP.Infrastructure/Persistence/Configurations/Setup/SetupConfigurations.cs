using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimateERP.Domain.Entities.Setup;

namespace UltimateERP.Infrastructure.Persistence.Configurations.Setup;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Code).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(50);
        builder.Property(e => e.Phone).HasMaxLength(50);
        builder.Property(e => e.Email).HasMaxLength(200);
        builder.Property(e => e.City).HasMaxLength(100);
        builder.Property(e => e.State).HasMaxLength(100);
        builder.Property(e => e.Country).HasMaxLength(100);
    }
}

public class CostClassConfiguration : IEntityTypeConfiguration<CostClass>
{
    public void Configure(EntityTypeBuilder<CostClass> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Code).IsUnique();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Code).HasMaxLength(50);
        builder.Property(e => e.StartMiti).HasMaxLength(20);
        builder.Property(e => e.EndMiti).HasMaxLength(20);
    }
}

public class EntityNumberingConfiguration : IEntityTypeConfiguration<EntityNumbering>
{
    public void Configure(EntityTypeBuilder<EntityNumbering> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Prefix).HasMaxLength(20);
        builder.Property(e => e.Suffix).HasMaxLength(20);
    }
}
