using SGO.Domain.Common;
using SGO.Domain.Odontogramas;
using SGO.Domain.Pacientes;

namespace SGO.Domain.Fichas;

/// <summary>
/// Representa una ficha clínica odontológica,
/// correspondiente a una atención concreta del paciente.
/// </summary>
public sealed class FichaClinica : Entity
{
    // --- Identificación ---
    public Guid Id { get; private set; } = Guid.NewGuid();

    // --- Relaciones ---
    public int PacienteDocumento { get; private set; }
    public Guid TurnoId { get; private set; }
    public int ProfesionalMatricula { get; private set; }

    // --- Datos de la ficha ---
    public DateTime FechaCreacionUtc { get; private set; }
    public string MotivoConsulta { get; private set; } = default!;
    public string? Diagnostico { get; private set; }
    public string? TratamientosRealizados { get; private set; }
    public string? Prescripciones { get; private set; }
    public string? Observaciones { get; private set; }
    public Paciente Paciente { get; private set; } = default!;

    // --- Odontograma Snapshot ---
    public Odontograma Odontograma { get; private set; } = default!;

    private FichaClinica() { } // EF Core

    private FichaClinica(
        int pacienteDocumento,
        Guid turnoId,
        int profesionalMatricula,
        string motivoConsulta,
        Odontograma odontograma)
    {
        if (pacienteDocumento <= 0)
            throw new ArgumentException("Debe indicar el documento del paciente.", nameof(pacienteDocumento));
        if (string.IsNullOrWhiteSpace(motivoConsulta))
            throw new ArgumentException("El motivo de consulta es obligatorio.", nameof(motivoConsulta));

        PacienteDocumento = pacienteDocumento;
        TurnoId = turnoId;
        ProfesionalMatricula = profesionalMatricula;
        FechaCreacionUtc = DateTime.UtcNow;
        MotivoConsulta = motivoConsulta.Trim();
        Odontograma = odontograma ?? throw new ArgumentNullException(nameof(odontograma));
    }

    public static FichaClinica CrearNueva(
        int pacienteDocumento,
        Guid turnoId,
        int profesionalMatricula,
        string motivoConsulta,
        Odontograma odontograma)
        => new(pacienteDocumento, turnoId, profesionalMatricula, motivoConsulta, odontograma);

    // --- Métodos de dominio ---
    public void ActualizarDiagnostico(string? diagnostico)
        => Diagnostico = diagnostico?.Trim();

    public void RegistrarTratamientos(string? tratamientos)
        => TratamientosRealizados = tratamientos?.Trim();

    public void RegistrarPrescripciones(string? prescripciones)
        => Prescripciones = prescripciones?.Trim();

    public void AgregarObservaciones(string? observaciones)
        => Observaciones = observaciones?.Trim();
}
