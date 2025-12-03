using ChurchSaaS.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchSaaS.Admin.Infrastructure.Configurations;

public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
{
    public void Configure(EntityTypeBuilder<Estado> builder)
    {
        builder.ToTable("Estado");
        builder.HasKey(e => e.Id).HasName("PK_Estado");

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Nome)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.Sigla)
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(e => e.Ativo)
            .HasDefaultValue(true)
            .IsRequired();
    }
}
