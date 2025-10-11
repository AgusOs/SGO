using Microsoft.Extensions.DependencyInjection;
using SGO.Application.Interfaces;
using SGO.Infrastructure.Persistence.Repositories;

namespace SGO.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPacienteRepository, PacienteRepository>();
        services.AddScoped<ITurnoRepository, TurnoRepository>();
        services.AddScoped<IFichaClinicaRepository, FichaClinicaRepository>();

        return services;
    }
}
