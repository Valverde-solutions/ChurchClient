using ChurchSaaS.Client.Domain.Abstractions;

namespace ChurchSaaS.Client.Domain.Entities;

public class Endereco : TenantAuditableEntity<int>
{
    public string Logradouro { get; set; } = string.Empty;
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cep { get; set; }
    public int CidadeId { get; set; }
}
