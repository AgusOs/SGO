using SGO.Domain.Procedimientos;

namespace SGO.Domain.Procedimientos;
public sealed class Limpieza : Procedimiento
{
    public bool ConProfilaxis { get; private set; }

    private Limpieza() { }

    public Limpieza(DateTime fecha, string profesional, bool conProfilaxis,
                    EstadoProcedimiento estado = EstadoProcedimiento.Realizado,
                    string? observaciones = null)
        : base(fecha, 0, profesional, estado, null, observaciones)
    {
        ConProfilaxis = conProfilaxis;
    }

    public override string Tipo => "Limpieza";
}
