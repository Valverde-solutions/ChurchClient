namespace ChurchSaaS.Admin.Api.Endpoints;

public sealed record CreateChurchClientRequest(
    string LegalName,
    string? TradeName,
    string MainContactName,
    string MainContactEmail,
    string? MainContactPhone,
    string? Document,
    string PlanCode,
    DateTimeOffset? TrialEndsAt,
    CreateChurchClientAddressRequest? Address)
{
    public string RequestedBy => "public-api";
}

public sealed record CreateChurchClientAddressRequest(
    string Logradouro,
    string? Numero,
    string? Complemento,
    string? Bairro,
    string? Cep,
    int CidadeId);
