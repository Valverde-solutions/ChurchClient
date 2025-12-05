using ChurchSaaS.Client.Application.Abstractions;
using ChurchSaaS.Client.Domain.Churches;
using MediatR;

namespace ChurchSaaS.Client.Application.Commands.ChurchUnits;

public sealed record CreateChurchUnitCommand(
    Guid TenantId,
    string LegalName,
    string? TradeName,
    string MainContactName,
    string MainContactEmail,
    string? MainContactPhone,
    string? Document,
    ChurchUnitType Type,
    string? Code,
    CreateChurchUnitAddress? Address,
    string CreatedByUserId,
    CreateChurchUnitUser User) : IRequest<Result<CreateChurchUnitResult>>;

public sealed record CreateChurchUnitAddress(
    string Logradouro,
    string? Numero,
    string? Complemento,
    string? Bairro,
    string? Cep,
    int CidadeId);

public sealed record CreateChurchUnitUser(
    string Email,
    string Password,
    string DisplayName,
    string Role,
    Guid? PersonId);

public sealed record CreateChurchUnitResult(Guid UnitId, Guid UserId);
