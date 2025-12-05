using System.Collections.Generic;
using ChurchSaaS.Admin.Domain.Abstractions;

namespace ChurchSaaS.Client.Domain.Abstractions;

public abstract class TenantAuditableEntity<TId> : AuditableEntity<TId>, ITenantEntity
{
    public TenantId TenantId { get; protected set; }

    protected TenantAuditableEntity() : base()
    {
    }

    protected TenantAuditableEntity(TenantId tenantId) : base()
    {
        TenantId = tenantId;
    }

    protected TenantAuditableEntity(TenantId tenantId, TId id) : base(id)
    {
        TenantId = tenantId;
    }

    public void SetTenant(TenantId tenantId)
    {
        if (!EqualityComparer<TenantId>.Default.Equals(TenantId, default)
            && !EqualityComparer<TenantId>.Default.Equals(TenantId, tenantId))
        {
            throw new InvalidOperationException("Não é permitido mudar o TenantId de uma entidade existente.");
        }

        TenantId = tenantId;
    }
}
