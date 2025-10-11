using SGO.Application.Interfaces;
using SGO.Domain.Fichas;
using SGO.Domain.Odontogramas;
using SGO.Domain.Procedimientos;
using SGO.Domain.Turnos;

namespace SGO.Application.UseCases.Fichas.Commands;

public sealed class RegistrarFichaClinicaCommand
{
    private readonly IFichaClinicaRepository _fichaRepository;
    private readonly ITurnoRepository _turnoRepository;

    public RegistrarFichaClinicaCommand(
        IFichaClinicaRepository fichaRepository,
        ITurnoRepository turnoRepository)
    {
        _fichaRepository = fichaRepository;
        _turnoRepository = turnoRepository;
    }

    public async Task<FichaClinica> EjecutarAsync(
        Guid turnoId,
        string motivoConsulta,
        string? diagnostico,
        List<Procedimiento> procedimientos,
        Odontograma odontograma,
        string? prescripciones,
        string? observaciones)
    {
        var turno = await _turnoRepository.GetByIdAsync(turnoId);
        if (turno is null)
            throw new InvalidOperationException("No se encontró el turno especificado.");

        if (turno.Estado != EstadoTurno.Programado && turno.Estado != EstadoTurno.Reprogramado)
            throw new InvalidOperationException("Solo se puede registrar ficha clínica para un turno vigente.");

        var ficha = FichaClinica.CrearNueva(
            turno.PacienteDocumento,
            turno.Id,
            turno.ProfesionalMatricula,
            motivoConsulta,
            odontograma,
            procedimientos
        );

        ficha.ActualizarDiagnostico(diagnostico);
        ficha.RegistrarPrescripciones(prescripciones);
        ficha.AgregarObservaciones(observaciones);

        turno.MarcarComoAtendido();

        await _fichaRepository.AddAsync(ficha);
        await _fichaRepository.SaveChangesAsync();
        await _turnoRepository.UpdateAsync(turno);
        await _turnoRepository.SaveChangesAsync();

        return ficha;
    }
}
