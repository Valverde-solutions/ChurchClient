using ChurchSaaS.Admin.Application.Interfaces.Repositories;
using ChurchSaaS.Admin.Domain.Entities;
using ChurchSaaS.Admin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChurchSaaS.Admin.Infrastructure.Repositories;

public class ChurchClientRepository : IChurchClientRepository
{
    private readonly AdminDbContext _context;

    public ChurchClientRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ChurchClient churchClient, CancellationToken cancellationToken = default)
    {
        await _context.ChurchClients.AddAsync(churchClient, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsWithEmailAsync(string email, CancellationToken cancellationToken = default)
        => _context.ChurchClients.AnyAsync(c => c.MainContactEmail == email, cancellationToken);

    public Task<bool> ExistsWithDocumentAsync(string document, CancellationToken cancellationToken = default)
        => _context.ChurchClients.AnyAsync(c => c.Document != null && c.Document == document, cancellationToken);

    public async Task<IReadOnlyList<ChurchClient>> ListAsync(CancellationToken cancellationToken = default)
        => await _context.ChurchClients
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
}
