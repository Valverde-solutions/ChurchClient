using ChurchSaaS.Client.Domain.Abstractions;
using ChurchSaaS.Client.Domain.Churches;
using ChurchSaaS.Client.Domain.Entities;
using ChurchSaaS.Client.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchSaaS.Admin.Infrastructure.Configurations;

public sealed class ChurchUnitConfiguration : IEntityTypeConfiguration<ChurchUnit>
{
    public void Configure(EntityTypeBuilder<ChurchUnit> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.TenantId)
            .HasConversion(
                tenantId => tenantId.Value,
                guid => new TenantId(guid))
            .IsRequired();

        builder.Property(c => c.LegalName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.TradeName)
            .HasMaxLength(200);

        builder.Property(c => c.Code)
            .HasMaxLength(50);

        builder.Property(c => c.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(c => c.MainContactName)
            .HasMaxLength(200);

        builder.OwnsOne(c => c.MainContactEmail, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("MainContactEmail")
                .HasMaxLength(200);
        });

        builder.OwnsOne(c => c.MainContactPhone, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("MainContactPhone")
                .HasMaxLength(50);
        });

        builder.OwnsOne(c => c.Document, doc =>
        {
            doc.Property(d => d.Value)
                .HasColumnName("Document")
                .HasMaxLength(50);

            doc.Property(d => d.Type)
                .HasColumnName("DocumentType");
        });

        builder.OwnsOne(c => c.Address, address =>
        {
            address.Property(a => a.Logradouro)
                .HasColumnName("Address_Logradouro")
                .HasMaxLength(200);

            address.Property(a => a.Numero)
                .HasColumnName("Address_Numero")
                .HasMaxLength(50);

            address.Property(a => a.Complemento)
                .HasColumnName("Address_Complemento")
                .HasMaxLength(150);

            address.Property(a => a.Bairro)
                .HasColumnName("Address_Bairro")
                .HasMaxLength(150);

            address.Property(a => a.Cep)
                .HasColumnName("Address_Cep")
                .HasMaxLength(20);

            address.Property(a => a.CidadeId)
                .HasColumnName("Address_CidadeId");
        });

        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.LocalChurch)
            .WithMany()
            .HasForeignKey(c => c.LocalChurchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
