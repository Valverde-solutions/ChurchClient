using ChurchSaaS.Client.Domain.Abstractions;
using ChurchSaaS.Client.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchSaaS.Admin.Infrastructure.Configurations;

public sealed class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
{
    public void Configure(EntityTypeBuilder<Endereco> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.TenantId)
            .HasConversion(
                tenantId => tenantId.Value,
                guid => new TenantId(guid))
            .IsRequired();

        builder.Property(e => e.Logradouro)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Numero)
            .HasMaxLength(50);

        builder.Property(e => e.Complemento)
            .HasMaxLength(150);

        builder.Property(e => e.Bairro)
            .HasMaxLength(150);

        builder.Property(e => e.Cep)
            .HasMaxLength(20);
    }
}
