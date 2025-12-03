using ChurchSaaS.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchSaaS.Admin.Infrastructure.Configurations;

public class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
{
    public void Configure(EntityTypeBuilder<Endereco> builder)
    {
        builder.ToTable("Endereco");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Logradouro)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Numero)
            .HasMaxLength(20);

        builder.Property(e => e.Complemento)
            .HasMaxLength(100);

        builder.Property(e => e.Bairro)
            .HasMaxLength(100);

        builder.Property(e => e.Cep)
            .HasMaxLength(15);

        builder.Property(e => e.CidadeId)
            .IsRequired();

        builder.HasIndex(e => e.CidadeId);
    }
}
