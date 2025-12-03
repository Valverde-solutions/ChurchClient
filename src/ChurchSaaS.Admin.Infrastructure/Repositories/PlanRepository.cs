using ChurchSaaS.Admin.Application.Interfaces.Repositories;
using ChurchSaaS.Admin.Domain.Entities;
using ChurchSaaS.Admin.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChurchSaaS.Admin.Infrastructure.Repositories;

public class PlanRepository : IPlanRepository
{
    private readonly AdminDbContext _context;

    public PlanRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Plan plan, CancellationToken cancellationToken = default)
    {
        await _context.Plans.AddAsync(plan, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Plan plan, CancellationToken cancellationToken = default)
    {
        _context.Plans.Update(plan);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<Plan?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalized = code.Trim().ToUpperInvariant();
        return _context.Plans.FirstOrDefaultAsync(p => p.Code == normalized, cancellationToken);
    }

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalized = code.Trim().ToUpperInvariant();
        return _context.Plans.AnyAsync(p => p.Code == normalized, cancellationToken);
    }

    public async Task<IReadOnlyList<Plan>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Plans
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }
}
