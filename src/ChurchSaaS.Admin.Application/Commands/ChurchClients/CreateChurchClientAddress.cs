namespace ChurchSaaS.Admin.Application.Commands.ChurchClients;

public sealed record CreateChurchClientAddress(
    string Logradouro,
    string? Numero,
    string? Complemento,
    string? Bairro,
    string? Cep,
    int CidadeId);

