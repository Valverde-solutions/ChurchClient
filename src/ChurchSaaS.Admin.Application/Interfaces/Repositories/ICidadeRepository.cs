using ChurchSaaS.Admin.Domain.Entities;

namespace ChurchSaaS.Admin.Application.Interfaces.Repositories;

public interface ICidadeRepository
{
    Task<IReadOnlyList<Cidade>> ListByEstadoAsync(int estadoId, CancellationToken cancellationToken = default);
}
