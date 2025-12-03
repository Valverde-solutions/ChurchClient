using ChurchSaaS.Admin.Application.Abstractions;
using ChurchSaaS.Admin.Application.Interfaces.Repositories;
using ChurchSaaS.Admin.Domain.Entities;
using FluentValidation;
using MediatR;

namespace ChurchSaaS.Admin.Application.Commands.Plans;

public sealed record UpdatePlanCommand(
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

public sealed class UpdatePlanCommandHandler : IRequestHandler<UpdatePlanCommand, Result<Guid>>
{
    private readonly IPlanRepository _repository;
    private readonly IValidator<UpdatePlanCommand> _validator;

    public UpdatePlanCommandHandler(IPlanRepository repository, IValidator<UpdatePlanCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid>> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToArray();
            return Result<Guid>.Fail("Validation failed.", errors);
        }

        var plan = await _repository.GetByCodeAsync(request.Code, cancellationToken);
        if (plan is null)
        {
            return Result<Guid>.Fail("Plan not found.");
        }

        try
        {
            plan.Update(
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

            await _repository.UpdateAsync(plan, cancellationToken);
            return Result<Guid>.Ok(plan.Id, "Plan updated.");
        }
        catch (Exception ex)
        {
            return Result<Guid>.Fail("Could not update plan.", new[] { ex.Message });
        }
    }
}
