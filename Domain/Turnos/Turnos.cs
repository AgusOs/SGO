using SGO.Domain.Common;
using SGO.Domain.Pacientes;

namespace SGO.Domain.Turnos;

public sealed class Turno : Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public int PacienteDocumento { get; private set; }
    public int ProfesionalMatricula { get; private set; }
    public DateTime FechaHora { get; private set; }
    public EstadoTurno Estado { get; private set; } = EstadoTurno.Programado;
    public string? Motivo { get; private set; }

    public Paciente Paciente { get; private set; } = default!;

    private Turno() { }

    public static Turno CrearNuevo(int pacienteDocumento, int profesionalMatricula, DateTime fechaHora, string? motivo)
        => new(pacienteDocumento, profesionalMatricula, fechaHora, motivo);

    private Turno(int pacienteDocumento, int profesionalMatricula, DateTime fechaHora, string? motivo)
    {
        if (pacienteDocumento <= 0)
            throw new ArgumentException("Debe indicar el documento del paciente.", nameof(pacienteDocumento));
        if (fechaHora < DateTime.UtcNow.AddMinutes(-5))
            throw new ArgumentException("La fecha del turno debe ser futura o reciente.", nameof(fechaHora));

        PacienteDocumento = pacienteDocumento;
        ProfesionalMatricula = profesionalMatricula;
        FechaHora = fechaHora;
        Motivo = motivo?.Trim();
        Estado = EstadoTurno.Programado;
    }

    public void MarcarComoNoAsistido(string? observacion = null)
    {
        Estado = EstadoTurno.NoAsistio;
    }

    public void MarcarComoAtendido() => Estado = EstadoTurno.Atendido;

    public void Cancelar(string? motivo = null)
    {
        Estado = EstadoTurno.Cancelado;
    }

    public void Reprogramar(DateTime nuevaFecha)
    {
        if (nuevaFecha <= DateTime.UtcNow)
            throw new ArgumentException("La nueva fecha debe ser futura.", nameof(nuevaFecha));
        FechaHora = nuevaFecha;
        Estado = EstadoTurno.Reprogramado;
    }
}


public enum EstadoTurno
{
    Programado = 0,
    Atendido = 1,
    NoAsistio = 2,
    Cancelado = 3,
    Reprogramado = 4
}
