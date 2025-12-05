using ChurchSaaS.Admin.Domain.Abstractions;

namespace ChurchSaaS.Client.Domain.Abstractions;

public interface ITenantAggregateRoot : IAggregateRoot, ITenantEntity
{
}
