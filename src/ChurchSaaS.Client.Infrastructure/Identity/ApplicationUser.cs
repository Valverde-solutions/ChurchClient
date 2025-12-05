using ChurchSaaS.Client.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace ChurchSaaS.Client.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public TenantId? TenantId { get; set; }
    public Guid? PersonId { get; set; }
    public string? DisplayName { get; set; }

    /// <summary>
    /// Usuário da plataforma (BusinessApp) com acesso total.
    /// </summary>
    public bool IsPlatformAdmin { get; set; }
}
