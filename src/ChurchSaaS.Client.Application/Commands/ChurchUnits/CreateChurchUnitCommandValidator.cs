using ChurchSaaS.Client.Domain.Churches;
using FluentValidation;

namespace ChurchSaaS.Client.Application.Commands.ChurchUnits;

public sealed class CreateChurchUnitCommandValidator : AbstractValidator<CreateChurchUnitCommand>
{
    public CreateChurchUnitCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.LegalName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.MainContactName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.MainContactEmail).NotEmpty().EmailAddress();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Code).MaximumLength(50);
        RuleFor(x => x.CreatedByUserId).NotEmpty();

        When(x => x.Address is not null, () =>
        {
            RuleFor(x => x.Address!.Logradouro).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Address!.Numero).MaximumLength(50);
            RuleFor(x => x.Address!.Complemento).MaximumLength(150);
            RuleFor(x => x.Address!.Bairro).MaximumLength(150);
            RuleFor(x => x.Address!.Cep).MaximumLength(20);
            RuleFor(x => x.Address!.CidadeId).GreaterThan(0);
        });

        RuleFor(x => x.User.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.User.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.User.DisplayName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.User.Role)
            .NotEmpty()
            .Must(role => AllowedRoles.Contains(role))
            .WithMessage($"Role must be one of: {string.Join(", ", AllowedRoles)}");
    }

    private static readonly string[] AllowedRoles =
    {
        "PlatformAdmin",
        "PlatformSupport",
        "ChurchAdmin",
        "Pastor",
        "Leader",
        "Member"
    };
}
