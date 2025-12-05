using ChurchSaaS.Client.Domain.Churches;

namespace ChurchSaaS.Client.Api.Endpoints;

public sealed record CreateChurchUnitRequest(
    Guid TenantId,
    string LegalName,
    string? TradeName,
    string MainContactName,
    string MainContactEmail,
    string? MainContactPhone,
    string? Document,
    ChurchUnitType Type,
    string? Code,
    AddressRequest? Address,
    string CreatedByUserId,
    NewUserRequest User);

public sealed record AddressRequest(
    string Logradouro,
    string? Numero,
    string? Complemento,
    string? Bairro,
    string? Cep,
    int CidadeId);

public sealed record NewUserRequest(
    string Email,
    string Password,
    string DisplayName,
    string Role,
    Guid? PersonId);
