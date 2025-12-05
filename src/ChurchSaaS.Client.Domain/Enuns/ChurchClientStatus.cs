namespace ChurchSaaS.Client.Domain.Enuns;

public enum ChurchClientStatus
{
    Draft = 0,              // cadastrado, mas ainda não provisionado
    PendingProvisioning = 1, // aguardando criação do tenant no produto
    Active = 2,             // provisionado e ativo
    Suspended = 3,          // suspenso (pagamento, decisão administrativa, etc.)
    Cancelled = 4           // contrato encerrado
}
