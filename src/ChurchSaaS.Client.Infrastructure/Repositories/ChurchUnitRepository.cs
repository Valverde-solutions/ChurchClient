using ChurchSaaS.Admin.Infrastructure.Persistence;
using ChurchSaaS.Client.Application.Interfaces.Repositories;
using ChurchSaaS.Client.Domain.Churches;
using ChurchSaaS.Client.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChurchSaaS.Admin.Infrastructure.Repositories;

public class ChurchUnitRepository : IChurchUnitRepository
{
    private readonly AdminDbContext _context;

    public ChurchUnitRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<ChurchUnit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ChurchUnits
            .Include(c => c.Parent)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task AddAsync(ChurchUnit churchUnit, CancellationToken cancellationToken = default)
    {
        return _context.ChurchUnits.AddAsync(churchUnit, cancellationToken).AsTask();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
