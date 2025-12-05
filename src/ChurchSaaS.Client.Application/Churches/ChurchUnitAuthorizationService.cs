using ChurchSaaS.Client.Application.Context;
using ChurchSaaS.Client.Application.Roles;
using ChurchSaaS.Client.Domain.Roles;

namespace ChurchSaaS.Client.Application.Churches;

public sealed class ChurchUnitAuthorizationService : IChurchUnitAuthorizationService
{
    private readonly ITenantContext _tenantContext;
    private readonly IUserRoleAssignmentRepository _roleRepository;
    private readonly IChurchUnitRepository _churchUnitRepository;

    public ChurchUnitAuthorizationService(
        ITenantContext tenantContext,
        IUserRoleAssignmentRepository roleRepository,
        IChurchUnitRepository churchUnitRepository)
    {
        _tenantContext = tenantContext;
        _roleRepository = roleRepository;
        _churchUnitRepository = churchUnitRepository;
    }

    public async Task<bool> CanAccessUnitAsync(Guid targetUnitId, CancellationToken cancellationToken = default)
    {
        if (_tenantContext.IsPlatformAdmin)
            return true;

        var tenantId = _tenantContext.TenantId;
        var userId = _tenantContext.UserId;

        var targetUnit = await _churchUnitRepository.GetByIdAsync(targetUnitId, tenantId, cancellationToken);
        if (targetUnit is null)
            return false;

        var assignments = await _roleRepository.GetByUserAsync(userId, tenantId, cancellationToken);

        foreach (var assignment in assignments)
        {
            if (assignment.TenantId != tenantId)
                continue;

            switch (assignment.ScopeType)
            {
                case RoleScopeType.Tenant when assignment.Role == RoleName.TenantAdmin:
                    return true;

                case RoleScopeType.LocalChurch when assignment.Role == RoleName.LocalChurchAdmin:
                    if (assignment.LocalChurchId.HasValue &&
                        targetUnit.LocalChurchId == assignment.LocalChurchId.Value)
                    {
                        return true;
                    }
                    break;

                case RoleScopeType.ChurchUnit when assignment.Role == RoleName.UnitManager:
                    if (assignment.ChurchUnitId.HasValue)
                    {
                        var allowed = await _churchUnitRepository.IsDescendantOrSelfAsync(
                            targetUnitId,
                            assignment.ChurchUnitId.Value,
                            tenantId,
                            cancellationToken);

                        if (allowed)
                            return true;
                    }
                    break;
            }
        }

        return false;
    }
}
