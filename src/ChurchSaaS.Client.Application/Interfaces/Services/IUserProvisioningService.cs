using ChurchSaaS.Client.Application.Abstractions;

namespace ChurchSaaS.Client.Application.Interfaces.Services;

public sealed record CreateUserRequest(
    string Email,
    string Password,
    string DisplayName,
    string Role,
    Guid TenantId,
    Guid? PersonId);

public interface IUserProvisioningService
{
    Task<Result<Guid>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
}
