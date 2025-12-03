using ChurchSaaS.Admin.Application.Commands.Plans;
using FluentValidation;

namespace ChurchSaaS.Admin.Application.Validators;

public sealed class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
{
    public CreatePlanCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.BasePrice)
            .GreaterThanOrEqualTo(0).WithMessage("Base price cannot be negative.");

        RuleFor(x => x.PricePerAdditionalTenant)
            .GreaterThanOrEqualTo(0).WithMessage("Price per additional tenant cannot be negative.");

        RuleFor(x => x.PricePerAdditionalMember)
            .GreaterThanOrEqualTo(0).WithMessage("Price per additional member cannot be negative.");

        RuleFor(x => x.MaxTenants)
            .GreaterThanOrEqualTo(0).When(x => x.MaxTenants.HasValue);

        RuleFor(x => x.MaxMembers)
            .GreaterThanOrEqualTo(0).When(x => x.MaxMembers.HasValue);

        RuleFor(x => x.RequestedBy)
            .NotEmpty().WithMessage("RequestedBy is required.")
            .MaximumLength(100);
    }
}
