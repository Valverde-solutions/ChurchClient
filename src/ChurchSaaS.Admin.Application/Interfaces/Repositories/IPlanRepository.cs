using ChurchSaaS.Admin.Domain.Entities;

namespace ChurchSaaS.Admin.Application.Interfaces.Repositories;

public interface IPlanRepository
{
    Task AddAsync(Plan plan, CancellationToken cancellationToken = default);
    Task<Plan?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Plan>> ListAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Plan plan, CancellationToken cancellationToken = default);
}
