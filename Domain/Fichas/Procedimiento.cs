namespace SGO.Domain.Procedimientos;

/// <summary>
/// Clase base abstracta para cualquier procedimiento odontológico.
/// Heredada por procedimientos específicos (Obturación, Extracción, Limpieza, etc.)
/// </summary>
public abstract class Procedimiento
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime Fecha { get; private set; }
    public int PiezaFdi { get; private set; } // Pieza afectada (FDI)
    public string? Superficie { get; private set; } // "M", "O", etc.
    public string Profesional { get; private set; } = default!;
    public string? Observaciones { get; private set; }

    public EstadoProcedimiento Estado { get; private set; } = EstadoProcedimiento.Realizado;

    protected Procedimiento() { }

    protected Procedimiento(DateTime fecha, int piezaFdi, string profesional,
                            EstadoProcedimiento estado = EstadoProcedimiento.Realizado,
                            string? superficie = null, string? observaciones = null)
    {
        Fecha = fecha;
        PiezaFdi = piezaFdi;
        Profesional = profesional.Trim();
        Estado = estado;
        Superficie = superficie?.ToUpperInvariant();
        Observaciones = observaciones?.Trim();
    }

    public abstract string Tipo { get; }

    public void CambiarEstado(EstadoProcedimiento nuevoEstado)
    {
        Estado = nuevoEstado;
    }
}

public enum EstadoProcedimiento
{
    Pendiente,   // Representa color rojo en el odontograma
    Realizado    // Representa color azul en el odontograma
}
