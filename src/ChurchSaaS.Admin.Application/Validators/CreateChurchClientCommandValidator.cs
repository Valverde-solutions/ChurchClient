using ChurchSaaS.Admin.Application.Commands.ChurchClients;
using ChurchSaaS.Admin.Application.Interfaces.Repositories;
using ChurchSaaS.Admin.Domain.ValueObjects;
using FluentValidation;

namespace ChurchSaaS.Admin.Application.Validators;

public sealed class CreateChurchClientCommandValidator : AbstractValidator<CreateChurchClientCommand>
{
    private readonly IChurchClientRepository _repository;

    public CreateChurchClientCommandValidator(IChurchClientRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.LegalName)
            .NotEmpty().WithMessage("Legal name is required.")
            .MaximumLength(200);

        RuleFor(x => x.TradeName)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.TradeName));

        RuleFor(x => x.MainContactName)
            .NotEmpty().WithMessage("Main contact name is required.")
            .MaximumLength(150);

        RuleFor(x => x.MainContactEmail)
            .NotEmpty().WithMessage("Main contact email is required.")
            .MaximumLength(200)
            .Must(BeValidEmail).WithMessage("Invalid email format.");

        RuleFor(x => x.MainContactEmail)
            .MustAsync(EmailNotExists)
            .WithMessage("A client with this email already exists.");

        RuleFor(x => x.MainContactPhone)
            .Must(BeValidPhoneNumber)
            .When(x => !string.IsNullOrWhiteSpace(x.MainContactPhone))
            .WithMessage("Invalid phone number.");

        RuleFor(x => x.Document)
            .Must(BeValidDocument)
            .When(x => !string.IsNullOrWhiteSpace(x.Document))
            .WithMessage("Invalid document.");

        RuleFor(x => x.Document)
            .MustAsync(DocumentNotExists)
            .When(x => !string.IsNullOrWhiteSpace(x.Document))
            .WithMessage("A client with this document already exists.");

        RuleFor(x => x.PlanCode)
            .NotEmpty().WithMessage("Plan code is required.")
            .MaximumLength(50);

        RuleFor(x => x.Address)
            .SetValidator(new CreateChurchClientAddressValidator()!)
            .When(x => x.Address is not null);

        RuleFor(x => x.RequestedBy)
            .NotEmpty().WithMessage("RequestedBy is required.")
            .MaximumLength(100);
    }

    private static bool BeValidEmail(string? email)
        => !string.IsNullOrWhiteSpace(email) && TryValidate(() => Email.Create(email));

    private static bool BeValidPhoneNumber(string? phone)
        => !string.IsNullOrWhiteSpace(phone) && TryValidate(() => PhoneNumber.Create(phone));

    private static bool BeValidDocument(string? document)
        => !string.IsNullOrWhiteSpace(document) && TryValidate(() => Document.Create(document));

    private static bool TryValidate(Action action)
    {
        try
        {
            action();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> EmailNotExists(string email, CancellationToken cancellationToken)
        => !await _repository.ExistsWithEmailAsync(email.Trim(), cancellationToken);

    private async Task<bool> DocumentNotExists(string? document, CancellationToken cancellationToken)
    {
        var normalized = document?.Trim() ?? string.Empty;
        return !await _repository.ExistsWithDocumentAsync(normalized, cancellationToken);
    }
}

public sealed class CreateChurchClientAddressValidator : AbstractValidator<CreateChurchClientAddress>
{
    public CreateChurchClientAddressValidator()
    {
        RuleFor(x => x.Logradouro)
            .NotEmpty().WithMessage("Street (logradouro) is required.")
            .MaximumLength(200);

        RuleFor(x => x.Numero)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.Numero));

        RuleFor(x => x.Complemento)
            .MaximumLength(150)
            .When(x => !string.IsNullOrWhiteSpace(x.Complemento));

        RuleFor(x => x.Bairro)
            .MaximumLength(150)
            .When(x => !string.IsNullOrWhiteSpace(x.Bairro));

        RuleFor(x => x.Cep)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.Cep));

        RuleFor(x => x.CidadeId)
            .GreaterThan(0).WithMessage("CidadeId must be greater than zero.");
    }
}
