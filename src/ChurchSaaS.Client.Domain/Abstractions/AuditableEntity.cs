namespace ChurchSaaS.Admin.Domain.Abstractions;

public abstract class AuditableEntity<TId> : Entity<TId>
{
    // Auditoria de criação
    public DateTimeOffset CreatedAt { get; protected set; }
    public string? CreatedBy { get; protected set; }

    // Auditoria de atualização
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public string? UpdatedBy { get; protected set; }

    // Auditoria de exclusão (soft delete)
    public DateTimeOffset? DeletedAt { get; protected set; }
    public string? DeletedBy { get; protected set; }

    // Conveniência para saber se está "deletado"
    public bool IsDeleted => DeletedAt.HasValue;

    protected AuditableEntity() : base()
    {
        CreatedAt = DateTimeOffset.UtcNow;
    }

    protected AuditableEntity(TId id) : base(id)
    {
        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Marca a entidade como criada por um usuário específico.
    /// Normalmente chamado a partir da Application/Infrastructure.
    /// </summary>
    public virtual void SetCreated(string userId)
    {
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = userId;
    }

    /// <summary>
    /// Marca a entidade como atualizada por um usuário específico.
    /// </summary>
    public virtual void SetUpdated(string userId)
    {
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = userId;
    }

    /// <summary>
    /// Marca a entidade como excluída (soft delete) por um usuário específico.
    /// </summary>
    public virtual void SetDeleted(string userId)
    {
        if (IsDeleted) return;

        DeletedAt = DateTimeOffset.UtcNow;
        DeletedBy = userId;
    }

    /// <summary>
    /// Restaura uma entidade que havia sido excluída (soft delete).
    /// </summary>
    public virtual void Restore()
    {
        DeletedAt = null;
        DeletedBy = null;
    }
}
