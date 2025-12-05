using ChurchSaaS.Client.Application.Interfaces.Repositories;
using ChurchSaaS.Client.Application.Interfaces.Services;
using ChurchSaaS.Admin.Infrastructure.Repositories;
using ChurchSaaS.Admin.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchSaaS.Admin.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IEstadoRepository, EstadoRepository>();
        services.AddScoped<ICidadeRepository, CidadeRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        services.AddScoped<IChurchUnitRepository, ChurchUnitRepository>();
        services.AddScoped<IUserProvisioningService, IdentityUserProvisioningService>();
     
        return services;
    }
}
