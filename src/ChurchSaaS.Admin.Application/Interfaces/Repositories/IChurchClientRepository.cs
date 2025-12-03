using ChurchSaaS.Admin.Domain.Entities;

namespace ChurchSaaS.Admin.Application.Interfaces.Repositories;

public interface IChurchClientRepository
{
    Task AddAsync(ChurchClient churchClient, CancellationToken cancellationToken = default);
    Task<bool> ExistsWithEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsWithDocumentAsync(string document, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ChurchClient>> ListAsync(CancellationToken cancellationToken = default);
}
