using ChurchSaaS.Admin.Domain.Abstractions;

namespace ChurchSaaS.Admin.Domain.Entities;

public class Endereco : Entity<int>
{
    public string Logradouro { get; set; } = string.Empty;
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cep { get; set; }
    public int CidadeId { get; set; }
}
