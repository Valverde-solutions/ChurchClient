using ChurchSaaS.Admin.Domain.Entities;
using ChurchSaaS.Client.Domain.Abstractions;
using ChurchSaaS.Client.Domain.Churches;
using ChurchSaaS.Client.Domain.Entities;
using ChurchSaaS.Client.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChurchSaaS.Admin.Infrastructure.Persistence;

public class AdminDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
    {
    }

    public DbSet<Estado> Estados => Set<Estado>();
    public DbSet<Cidade> Cidades => Set<Cidade>();
    public DbSet<Endereco> Enderecos => Set<Endereco>();
    public DbSet<ChurchUnit> ChurchUnits => Set<ChurchUnit>();
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Map TenantId value object to Guid for Identity users
        builder.Entity<ApplicationUser>()
            .Property(u => u.TenantId)
            .HasConversion(
                tenantId => tenantId.HasValue ? tenantId.Value.Value : (Guid?)null,
                guid => guid.HasValue ? new TenantId(guid.Value) : (TenantId?)null);

        builder.ApplyConfigurationsFromAssembly(typeof(AdminDbContext).Assembly);
    }
}
