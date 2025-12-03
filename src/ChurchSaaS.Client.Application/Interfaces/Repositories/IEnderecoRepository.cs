using ChurchSaaS.Admin.Domain.Entities;

namespace ChurchSaaS.Client.Application.Interfaces.Repositories;

public interface IEnderecoRepository
{
    Task<IReadOnlyList<Endereco>> ListByCidadeAsync(int cidadeId, CancellationToken cancellationToken = default);
}
