using ChurchSaaS.Client.Application.Abstractions;
using ChurchSaaS.Client.Application.Interfaces.Services;
using ChurchSaaS.Client.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace ChurchSaaS.Admin.Infrastructure.Services;

public class IdentityUserProvisioningService : IUserProvisioningService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityUserProvisioningService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<Guid>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true,
            DisplayName = request.DisplayName,
            TenantId = new(request.TenantId),
            PersonId = request.PersonId
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
            return Result<Guid>.Fail("Failed to create user", createResult.Errors.Select(e => e.Description));

        var addRoleResult = await _userManager.AddToRoleAsync(user, request.Role);
        if (!addRoleResult.Succeeded)
            return Result<Guid>.Fail($"User created but failed to add role {request.Role}", addRoleResult.Errors.Select(e => e.Description));

        return Result<Guid>.Ok(user.Id);
    }
}
