using ChurchSaaS.Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChurchSaaS.Admin.Infrastructure.Configurations;

public class CidadeConfiguration : IEntityTypeConfiguration<Cidade>
{
    public void Configure(EntityTypeBuilder<Cidade> builder)
    {
        builder.ToTable("Cidade");
        builder.HasKey(c => c.Id).HasName("PK_Cidade_1");

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nome)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(c => c.EstadoId)
            .IsRequired();

        builder.Property(c => c.Ativo)
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasIndex(c => c.EstadoId).HasDatabaseName("IX_Cidade_EstadoId_1");
    }
}
