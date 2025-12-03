namespace ChurchSaaS.Admin.Domain.Entities;

public class Cidade
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int EstadoId { get; set; }
    public bool Ativo { get; set; } = true;
}
