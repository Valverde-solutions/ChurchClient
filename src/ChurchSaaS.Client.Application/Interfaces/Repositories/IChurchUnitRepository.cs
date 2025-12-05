using ChurchSaaS.Client.Domain.Churches;
using ChurchSaaS.Client.Domain.Entities;

namespace ChurchSaaS.Client.Application.Interfaces.Repositories;

public interface IChurchUnitRepository
{
    Task<ChurchUnit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(ChurchUnit churchUnit, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
