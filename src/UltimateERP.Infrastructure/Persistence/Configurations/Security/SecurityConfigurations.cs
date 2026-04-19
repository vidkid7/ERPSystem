using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimateERP.Domain.Entities.Security;

namespace UltimateERP.Infrastructure.Persistence.Configurations.Security;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Username).IsUnique();
        builder.Property(e => e.Username).HasMaxLength(100).IsRequired();
        builder.Property(e => e.PasswordHash).HasMaxLength(500).IsRequired();
        builder.Property(e => e.PasswordSalt).HasMaxLength(200);
        builder.Property(e => e.Email).HasMaxLength(200);
        builder.Property(e => e.Phone).HasMaxLength(50);
        builder.Property(e => e.UserType).HasMaxLength(50);
        builder.Property(e => e.LastLoginIP).HasMaxLength(50);

        builder.HasOne(e => e.Branch)
            .WithMany()
            .HasForeignKey(e => e.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class UserGroupMemberConfiguration : IEntityTypeConfiguration<UserGroupMember>
{
    public void Configure(EntityTypeBuilder<UserGroupMember> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.UserGroupId, e.UserId }).IsUnique();

        builder.HasOne(e => e.UserGroup)
            .WithMany(e => e.Members)
            .HasForeignKey(e => e.UserGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.User)
            .WithMany(e => e.UserGroupMembers)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class EntityPermissionConfiguration : IEntityTypeConfiguration<EntityPermission>
{
    public void Configure(EntityTypeBuilder<EntityPermission> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.UserId, e.EntityId }).IsUnique();

        builder.HasOne(e => e.User)
            .WithMany(e => e.EntityPermissions)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class BranchAccessConfiguration : IEntityTypeConfiguration<BranchAccess>
{
    public void Configure(EntityTypeBuilder<BranchAccess> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.UserId, e.BranchId }).IsUnique();

        builder.HasOne(e => e.User)
            .WithMany(e => e.BranchAccesses)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Branch)
            .WithMany()
            .HasForeignKey(e => e.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class GodownAccessConfiguration : IEntityTypeConfiguration<GodownAccess>
{
    public void Configure(EntityTypeBuilder<GodownAccess> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.UserId, e.GodownId }).IsUnique();

        builder.HasOne(e => e.User)
            .WithMany(e => e.GodownAccesses)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Godown)
            .WithMany()
            .HasForeignKey(e => e.GodownId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
