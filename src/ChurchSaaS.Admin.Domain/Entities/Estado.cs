namespace ChurchSaaS.Admin.Domain.Entities;

public class Estado
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
}
