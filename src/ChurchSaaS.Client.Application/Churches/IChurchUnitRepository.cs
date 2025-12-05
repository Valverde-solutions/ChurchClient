using ChurchSaaS.Client.Domain.Abstractions;
using ChurchSaaS.Client.Domain.Entities;

namespace ChurchSaaS.Client.Application.Churches;

public interface IChurchUnitRepository
{
    Task<ChurchUnit?> GetByIdAsync(Guid id, TenantId tenantId, CancellationToken cancellationToken = default);
    Task<bool> IsDescendantOrSelfAsync(Guid targetUnitId, Guid rootUnitId, TenantId tenantId, CancellationToken cancellationToken = default);
}
