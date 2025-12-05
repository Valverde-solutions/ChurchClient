using ChurchSaaS.Client.Domain.Abstractions;

namespace ChurchSaaS.Client.Domain.Roles;

public sealed class UserRoleAssignment : TenantAuditableEntity<Guid>, ITenantAggregateRoot
{
    public Guid UserId { get; private set; }
    public RoleName Role { get; private set; }
    public RoleScopeType ScopeType { get; private set; }
    public Guid? LocalChurchId { get; private set; }
    public Guid? ChurchUnitId { get; private set; }

    private UserRoleAssignment() : base()
    {
    }

    private UserRoleAssignment(
        TenantId tenantId,
        Guid userId,
        RoleName role,
        RoleScopeType scopeType,
        Guid? localChurchId,
        Guid? churchUnitId) : base(tenantId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId is required.", nameof(userId));

        if (!Enum.IsDefined(typeof(RoleName), role))
            throw new ArgumentOutOfRangeException(nameof(role), "Invalid role.");

        if (!Enum.IsDefined(typeof(RoleScopeType), scopeType))
            throw new ArgumentOutOfRangeException(nameof(scopeType), "Invalid scope type.");

        if (scopeType == RoleScopeType.LocalChurch && localChurchId is null)
            throw new ArgumentException("LocalChurchId is required for LocalChurch scope.", nameof(localChurchId));

        if (scopeType == RoleScopeType.ChurchUnit && churchUnitId is null)
            throw new ArgumentException("ChurchUnitId is required for ChurchUnit scope.", nameof(churchUnitId));

        if (scopeType == RoleScopeType.Tenant && (localChurchId is not null || churchUnitId is not null))
            throw new ArgumentException("Tenant scope cannot have LocalChurchId or ChurchUnitId.");

        TenantId = tenantId;
        UserId = userId;
        Role = role;
        ScopeType = scopeType;
        LocalChurchId = localChurchId;
        ChurchUnitId = churchUnitId;
    }

    public static UserRoleAssignment CreateTenantAdmin(TenantId tenantId, Guid userId, string createdByUserId)
    {
        var assignment = new UserRoleAssignment(
            tenantId,
            userId,
            RoleName.TenantAdmin,
            RoleScopeType.Tenant,
            null,
            null);

        assignment.SetCreated(createdByUserId);
        return assignment;
    }

    public static UserRoleAssignment CreateLocalChurchAdmin(TenantId tenantId, Guid userId, Guid localChurchId, string createdByUserId)
    {
        var assignment = new UserRoleAssignment(
            tenantId,
            userId,
            RoleName.LocalChurchAdmin,
            RoleScopeType.LocalChurch,
            localChurchId,
            null);

        assignment.SetCreated(createdByUserId);
        return assignment;
    }

    public static UserRoleAssignment CreateUnitManager(TenantId tenantId, Guid userId, Guid churchUnitId, string createdByUserId)
    {
        var assignment = new UserRoleAssignment(
            tenantId,
            userId,
            RoleName.UnitManager,
            RoleScopeType.ChurchUnit,
            null,
            churchUnitId);

        assignment.SetCreated(createdByUserId);
        return assignment;
    }
}
