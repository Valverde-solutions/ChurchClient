using ChurchSaaS.Admin.Application.Abstractions;
using ChurchSaaS.Admin.Application.Interfaces.Repositories;
using ChurchSaaS.Admin.Domain.Entities;
using MediatR;

namespace ChurchSaaS.Admin.Application.Queries.ChurchClients;

public sealed record ListChurchClientsQuery() : IRequest<Result<IReadOnlyList<ChurchClient>>>;

public sealed class ListChurchClientsQueryHandler : IRequestHandler<ListChurchClientsQuery, Result<IReadOnlyList<ChurchClient>>>
{
    private readonly IChurchClientRepository _repository;

    public ListChurchClientsQueryHandler(IChurchClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyList<ChurchClient>>> Handle(ListChurchClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await _repository.ListAsync(cancellationToken);
        return Result<IReadOnlyList<ChurchClient>>.Ok(clients);
    }
}
