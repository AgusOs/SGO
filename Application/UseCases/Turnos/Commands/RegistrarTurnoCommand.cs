using SGO.Application.Interfaces;
using SGO.Domain.Turnos;

namespace SGO.Application.UseCases.Turnos.Commands;

public sealed class RegistrarTurnoCommand
{
    private readonly ITurnoRepository _repository;
    private readonly IPacienteRepository _pacienteRepository;

    public RegistrarTurnoCommand(ITurnoRepository repository, IPacienteRepository pacienteRepository)
    {
        _repository = repository;
        _pacienteRepository = pacienteRepository;
    }

    public async Task<Turno> EjecutarAsync(
        int documentoPaciente,
        int profesionalMatricula,
        DateTime fechaHora,
        string? motivo)
    {
        var paciente = await _pacienteRepository.GetByDocumentoAsync(documentoPaciente);
        if (paciente is null)
            throw new InvalidOperationException("No existe un paciente con ese documento.");

        if (await _repository.ExisteTurnoEnHorarioAsync(profesionalMatricula, fechaHora))
            throw new InvalidOperationException("Ya existe un turno asignado en ese horario.");

        var turno = Turno.CrearNuevo(documentoPaciente, profesionalMatricula, fechaHora, motivo);
        await _repository.AddAsync(turno);
        await _repository.SaveChangesAsync();

        return turno;
    }
}
