using ChurchSaaS.Admin.Application.Abstractions;
using ChurchSaaS.Admin.Application.Interfaces.Repositories;
using ChurchSaaS.Admin.Domain.Entities;
using FluentValidation;
using MediatR;

namespace ChurchSaaS.Admin.Application.Commands.ChurchClients;

public sealed class CreateChurchClientCommandHandler : IRequestHandler<CreateChurchClientCommand, Result<Guid>>
{
    private readonly IChurchClientRepository _repository;
    private readonly IValidator<CreateChurchClientCommand> _validator;

    public CreateChurchClientCommandHandler(
        IChurchClientRepository repository,
        IValidator<CreateChurchClientCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid>> Handle(CreateChurchClientCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToArray();
            return Result<Guid>.Fail("Validation failed.", errors);
        }

        try
        {
            var address = MapAddress(request.Address);

            var client = ChurchClient.Create(
                request.LegalName.Trim(),
                request.TradeName?.Trim(),
                request.MainContactName.Trim(),
                request.MainContactEmail.Trim(),
                NormalizeOrNull(request.MainContactPhone),
                NormalizeOrNull(request.Document),
                request.PlanCode.Trim(),
                request.TrialEndsAt,
                request.RequestedBy,
                address);

            await _repository.AddAsync(client, cancellationToken);

            return Result<Guid>.Ok(client.Id, "Church client created.");
        }
        catch (Exception ex)
        {
            return Result<Guid>.Fail("Could not create church client.", new[] { ex.Message });
        }
    }

    private static Endereco? MapAddress(CreateChurchClientAddress? addressDto)
    {
        if (addressDto is null) return null;

        return new Endereco
        {
            Logradouro = addressDto.Logradouro.Trim(),
            Numero = addressDto.Numero?.Trim(),
            Complemento = addressDto.Complemento?.Trim(),
            Bairro = addressDto.Bairro?.Trim(),
            Cep = addressDto.Cep?.Trim(),
            CidadeId = addressDto.CidadeId
        };
    }

    private static string? NormalizeOrNull(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

