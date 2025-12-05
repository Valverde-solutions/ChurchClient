using ChurchSaaS.Admin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ChurchSaaS.Client.Infrastructure.Persistence;

/// <summary>
/// Design-time factory to enable EF Core migrations without needing the web host.
/// </summary>
public sealed class AdminDbContextFactory : IDesignTimeDbContextFactory<AdminDbContext>
{
    public AdminDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=churchsaas_admin;Username=postgres;Password=postgres";

        var builder = new DbContextOptionsBuilder<AdminDbContext>();
        builder.UseNpgsql(connectionString);

        return new AdminDbContext(builder.Options);
    }
}
