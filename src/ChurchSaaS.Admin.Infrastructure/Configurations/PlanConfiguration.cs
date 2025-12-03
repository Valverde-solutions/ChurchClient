using ChurchSaaS.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchSaaS.Admin.Infrastructure.Configurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.ToTable("Plan");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.BasePrice)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(p => p.PricePerAdditionalTenant)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(p => p.PricePerAdditionalMember)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(p => p.MaxTenants);
        builder.Property(p => p.MaxMembers);

        builder.Property(p => p.HasFinance).IsRequired();
        builder.Property(p => p.HasEvents).IsRequired();
        builder.Property(p => p.HasCommunication).IsRequired();
        builder.Property(p => p.HasKidsCheckin).IsRequired();

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.CreatedBy).HasMaxLength(100);
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.UpdatedBy).HasMaxLength(100);
        builder.Property(p => p.DeletedAt);
        builder.Property(p => p.DeletedBy).HasMaxLength(100);

        builder.HasIndex(p => p.Code).IsUnique();
    }
}
