using ChurchSaaS.Admin.Domain.Entities;
using ChurchSaaS.Admin.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchSaaS.Admin.Infrastructure.Configurations;

public class ChurchClientConfiguration : IEntityTypeConfiguration<ChurchClient>
{
    public void Configure(EntityTypeBuilder<ChurchClient> builder)
    {
        builder.ToTable("ChurchClient");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LegalName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.TradeName)
            .HasMaxLength(200);

        builder.Property(x => x.PlanCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.MainContactName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.MainContactEmail)
            .IsRequired()
            .HasMaxLength(200)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value));

        builder.Property(x => x.MainContactPhone)
            .HasMaxLength(30)
            .HasConversion(
                phone => phone == null ? null : phone.Value,
                value => value == null ? null : PhoneNumber.Create(value));

        builder.Property(x => x.Document)
            .HasMaxLength(32)
            .HasConversion(
                document => document == null ? null : document.Value,
                value => value == null ? null : Document.Create(value));

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.TrialEndsAt);
        builder.Property(x => x.ActivatedAt);
        builder.Property(x => x.CancelledAt);
        builder.Property(x => x.ProductTenantId)
            .HasMaxLength(100);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).HasMaxLength(100);
        builder.Property(x => x.UpdatedAt);
        builder.Property(x => x.UpdatedBy).HasMaxLength(100);
        builder.Property(x => x.DeletedAt);
        builder.Property(x => x.DeletedBy).HasMaxLength(100);

        builder.HasOne(x => x.Address)
            .WithMany()
            .HasForeignKey("AddressId")
            .OnDelete(DeleteBehavior.SetNull);
    }
}
