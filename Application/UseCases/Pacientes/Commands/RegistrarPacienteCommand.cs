using SGO.Application.Interfaces;
using SGO.Domain.Pacientes;

namespace SGO.Application.UseCases.Pacientes.Commands;

public sealed class RegistrarPacienteCommand
{
    private readonly IPacienteRepository _repository;

    public RegistrarPacienteCommand(IPacienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<Paciente> EjecutarAsync(
        int documento,
        string nombre,
        string apellido,
        DateOnly? fechaNacimiento,
        string? obraSocial,
        string? numeroAfiliado,
        string? email,
        string? telefono)
    {
        if (await _repository.ExistsAsync(documento))
            throw new InvalidOperationException("Ya existe un paciente con ese documento.");

        var paciente = Paciente.CrearNuevo(
            documento, nombre, apellido, fechaNacimiento, obraSocial, numeroAfiliado, email, telefono
        );

        await _repository.AddAsync(paciente);
        await _repository.SaveChangesAsync();

        return paciente;
    }
}
