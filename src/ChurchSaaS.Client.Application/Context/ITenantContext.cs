using ChurchSaaS.Client.Domain.Abstractions;

namespace ChurchSaaS.Client.Application.Context;

public interface ITenantContext
{
    TenantId TenantId { get; }
    Guid UserId { get; }
    bool IsPlatformAdmin { get; }
}
