using ChurchSaaS.Admin.Domain.Abstractions;

namespace ChurchSaaS.Client.Domain.ValueObjects;

public sealed class Address : ValueObject
{
    public string Logradouro { get; }
    public string? Numero { get; }
    public string? Complemento { get; }
    public string? Bairro { get; }
    public string? Cep { get; }
    public int CidadeId { get; }

    private Address(string logradouro, string? numero, string? complemento, string? bairro, string? cep, int cidadeId)
    {
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cep = cep;
        CidadeId = cidadeId;
    }

    public static Address Create(string logradouro, string? numero, string? complemento, string? bairro, string? cep, int cidadeId)
    {
        if (string.IsNullOrWhiteSpace(logradouro))
            throw new ArgumentException("Logradouro is required.", nameof(logradouro));

        return new Address(logradouro.Trim(), string.IsNullOrWhiteSpace(numero) ? null : numero?.Trim(),
            string.IsNullOrWhiteSpace(complemento) ? null : complemento?.Trim(),
            string.IsNullOrWhiteSpace(bairro) ? null : bairro?.Trim(),
            string.IsNullOrWhiteSpace(cep) ? null : cep?.Trim(),
            cidadeId);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Logradouro;
        yield return Numero;
        yield return Complemento;
        yield return Bairro;
        yield return Cep;
        yield return CidadeId;
    }

    public override string ToString() => $"{Logradouro}{(Numero is null ? "" : ", " + Numero)}{(Bairro is null ? "" : ", " + Bairro)}{(Cep is null ? "" : " - " + Cep)}";
}
