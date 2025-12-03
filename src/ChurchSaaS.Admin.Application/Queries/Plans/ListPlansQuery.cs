using ChurchSaaS.Admin.Application.Abstractions;
using ChurchSaaS.Admin.Application.Interfaces.Repositories;
using ChurchSaaS.Admin.Domain.Entities;
using MediatR;

namespace ChurchSaaS.Admin.Application.Queries.Plans;

public sealed record ListPlansQuery() : IRequest<Result<IReadOnlyList<Plan>>>;

public sealed class ListPlansQueryHandler : IRequestHandler<ListPlansQuery, Result<IReadOnlyList<Plan>>>
{
    private readonly IPlanRepository _repository;

    public ListPlansQueryHandler(IPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyList<Plan>>> Handle(ListPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _repository.ListAsync(cancellationToken);
        return Result<IReadOnlyList<Plan>>.Ok(plans);
    }
}
