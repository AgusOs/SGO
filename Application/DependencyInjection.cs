using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SGO.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<UseCases.Pacientes.Commands.RegistrarPacienteCommand>();
        services.AddScoped<UseCases.Turnos.Commands.RegistrarTurnoCommand>();
        services.AddScoped<UseCases.Fichas.Commands.RegistrarFichaClinicaCommand>();

        return services;
    }
}
