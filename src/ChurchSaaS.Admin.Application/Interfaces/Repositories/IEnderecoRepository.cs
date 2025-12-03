using ChurchSaaS.Admin.Domain.Entities;

namespace ChurchSaaS.Admin.Application.Interfaces.Repositories;

public interface IEnderecoRepository
{
    Task<IReadOnlyList<Endereco>> ListByCidadeAsync(int cidadeId, CancellationToken cancellationToken = default);
}
