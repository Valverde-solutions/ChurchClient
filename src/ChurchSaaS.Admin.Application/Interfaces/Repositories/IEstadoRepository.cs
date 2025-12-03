using ChurchSaaS.Admin.Domain.Entities;

namespace ChurchSaaS.Admin.Application.Interfaces.Repositories;

public interface IEstadoRepository
{
    Task<IReadOnlyList<Estado>> ListAsync(CancellationToken cancellationToken = default);
}
