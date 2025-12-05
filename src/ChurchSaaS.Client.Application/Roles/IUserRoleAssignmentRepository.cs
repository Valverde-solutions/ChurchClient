using ChurchSaaS.Client.Domain.Abstractions;
using ChurchSaaS.Client.Domain.Roles;

namespace ChurchSaaS.Client.Application.Roles;

public interface IUserRoleAssignmentRepository
{
    Task<IReadOnlyList<UserRoleAssignment>> GetByUserAsync(
        Guid userId,
        TenantId tenantId,
        CancellationToken cancellationToken = default);
}
