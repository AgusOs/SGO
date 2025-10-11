using SGO.Domain.Common;

namespace SGO.Domain.Procedimientos;

/// <summary>
/// Representa un tratamiento o práctica realizada dentro de una ficha clínica.
/// </summary>
public sealed class Procedimiento : Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Tipo { get; private set; } = default!;
    public string? Descripcion { get; private set; }
    public int? PiezaFdi { get; private set; }
    public DateTime Fecha { get; private set; } = DateTime.UtcNow;
    public int ProfesionalMatricula { get; private set; }

    private Procedimiento() { } // EF

    private Procedimiento(string tipo, string? descripcion, int? piezaFdi, int profesionalMatricula)
    {
        if (string.IsNullOrWhiteSpace(tipo))
            throw new ArgumentException("El tipo de procedimiento es obligatorio.", nameof(tipo));

        Tipo = tipo.Trim();
        Descripcion = descripcion?.Trim();
        PiezaFdi = piezaFdi;
        ProfesionalMatricula = profesionalMatricula;
        Fecha = DateTime.UtcNow;
    }

    public static Procedimiento Crear(string tipo, string? descripcion, int? piezaFdi, int profesionalMatricula)
        => new(tipo, descripcion, piezaFdi, profesionalMatricula);

    public override string ToString()
        => $"{Tipo} ({PiezaFdi?.ToString() ?? "sin pieza"}) - {Descripcion}";
}
