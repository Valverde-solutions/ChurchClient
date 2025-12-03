using ChurchSaaS.Admin.Domain.Entities;
using ChurchSaaS.Client.Application.Interfaces.Repositories;
using ChurchSaaS.Admin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChurchSaaS.Admin.Infrastructure.Repositories;

public class CidadeRepository : ICidadeRepository
{
    private readonly AdminDbContext _context;

    public CidadeRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Cidade>> ListByEstadoAsync(int estadoId, CancellationToken cancellationToken = default)
    {
        return await _context.Cidades
            .AsNoTracking()
            .Where(c => c.EstadoId == estadoId)
            .OrderBy(c => c.Nome)
            .ToListAsync(cancellationToken);
    }
}
