using ChurchSaaS.Client.Application.Interfaces.Repositories;
using ChurchSaaS.Admin.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchSaaS.Admin.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IEstadoRepository, EstadoRepository>();
        services.AddScoped<ICidadeRepository, CidadeRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();
     
        return services;
    }
}
