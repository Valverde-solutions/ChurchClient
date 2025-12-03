using ChurchSaaS.Admin.Application.Abstractions;
using ChurchSaaS.Admin.Application.Interfaces.Repositories;
using ChurchSaaS.Admin.Domain.Entities;
using FluentValidation;
using MediatR;

namespace ChurchSaaS.Admin.Application.Commands.Plans;

public sealed record CreatePlanCommand(
    string Code,
    string Name,
    string? Description,
    decimal BasePrice,
    decimal PricePerAdditionalTenant,
    decimal PricePerAdditionalMember,
    int? MaxTenants,
    int? MaxMembers,
    bool HasFinance,
    bool HasEvents,
    bool HasCommunication,
    bool HasKidsCheckin,
    string RequestedBy) : IRequest<Result<Guid>>;

public sealed class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, Result<Guid>>
{
    private readonly IPlanRepository _repository;
    private readonly IValidator<CreatePlanCommand> _validator;

    public CreatePlanCommandHandler(IPlanRepository repository, IValidator<CreatePlanCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid>> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToArray();
            return Result<Guid>.Fail("Validation failed.", errors);
        }

        if (await _repository.ExistsByCodeAsync(request.Code, cancellationToken))
        {
            return Result<Guid>.Fail("A plan with this code already exists.");
        }

        try
        {
            var plan = Plan.Create(
                request.Code,
                request.Name,
                request.Description,
                request.BasePrice,
                request.PricePerAdditionalTenant,
                request.PricePerAdditionalMember,
                request.MaxTenants,
                request.MaxMembers,
                request.HasFinance,
                request.HasEvents,
                request.HasCommunication,
                request.HasKidsCheckin,
                request.RequestedBy);

            await _repository.AddAsync(plan, cancellationToken);

            return Result<Guid>.Ok(plan.Id, "Plan created.");
        }
        catch (Exception ex)
        {
            return Result<Guid>.Fail("Could not create plan.", new[] { ex.Message });
        }
    }
}
