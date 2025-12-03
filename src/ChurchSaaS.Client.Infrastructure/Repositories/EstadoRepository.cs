using ChurchSaaS.Admin.Domain.Entities;
using ChurchSaaS.Client.Application.Interfaces.Repositories;
using ChurchSaaS.Admin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChurchSaaS.Admin.Infrastructure.Repositories;

public class EstadoRepository : IEstadoRepository
{
    private readonly AdminDbContext _context;

    public EstadoRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Estado>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Estados
            .AsNoTracking()
            .OrderBy(e => e.Nome)
            .ToListAsync(cancellationToken);
    }
}
