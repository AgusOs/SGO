using SGO.Domain.Procedimientos;

namespace SGO.Domain.Procedimientos;

public sealed class Extraccion : Procedimiento
{
    //public string Motivo { get; private set; } = default!;
    public bool Quirurgica { get; private set; }

    private Extraccion() { }

    public Extraccion(DateTime fecha, int piezaFdi, string profesional, string motivo, bool quirurgica,
                      EstadoProcedimiento estado = EstadoProcedimiento.Realizado,
                      string? observaciones = null)
        : base(fecha, piezaFdi, profesional, estado, null, observaciones)
    {
        //Motivo = motivo;
        Quirurgica = quirurgica;
    }

    public override string Tipo => "Extracción";
}