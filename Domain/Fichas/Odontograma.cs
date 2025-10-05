using SGO.Domain.Common;

namespace SGO.Domain.Odontogramas;

/// <summary>
/// Representa el estado odontológico del paciente en una fecha específica.
/// Forma parte de una FichaClínica (snapshot histórico).
/// </summary>
public sealed class Odontograma : Entity
{
    public Guid FichaClinicaId { get; private set; }

    private readonly List<PiezaDental> _piezas = new();
    public IReadOnlyCollection<PiezaDental> Piezas => _piezas.AsReadOnly();

    private Odontograma() { }

    private Odontograma(Guid fichaClinicaId)
    {
        FichaClinicaId = fichaClinicaId;
        InicializarPiezas();
    }

    public static Odontograma CrearNuevo(Guid fichaClinicaId)
    {
        return new Odontograma(fichaClinicaId);
    }

    private void InicializarPiezas()
    {
        var piezasFdi = new[]
        {
            11,12,13,14,15,16,17,18,21,22,23,24,25,26,27,28,
            31,32,33,34,35,36,37,38,41,42,43,44,45,46,47,48
        };

        foreach (var fdi in piezasFdi)
            _piezas.Add(PiezaDental.Nueva(fdi));
    }

    public void MarcarSuperficie(int fdi, string superficie, SuperficieEstado estado)
    {
        var pieza = _piezas.SingleOrDefault(p => p.Fdi == fdi)
            ?? throw new InvalidOperationException($"No se encontró la pieza {fdi} en el odontograma.");
        pieza.CambiarSuperficie(superficie, estado);
    }
}

public sealed class PiezaDental
{
    public int Fdi { get; private set; }
    public DienteEstado Estado { get; private set; } = DienteEstado.Presente;

    public SuperficieEstado M { get; private set; } = SuperficieEstado.Sana;
    public SuperficieEstado D { get; private set; } = SuperficieEstado.Sana;
    public SuperficieEstado V { get; private set; } = SuperficieEstado.Sana;
    public SuperficieEstado L { get; private set; } = SuperficieEstado.Sana;
    public SuperficieEstado O { get; private set; } = SuperficieEstado.Sana;

    private PiezaDental() { }

    public static PiezaDental Nueva(int fdi) => new() { Fdi = fdi };

    public void CambiarSuperficie(string superficie, SuperficieEstado nuevoEstado)
    {
        switch (superficie.ToUpperInvariant())
        {
            case "M": M = nuevoEstado; break;
            case "D": D = nuevoEstado; break;
            case "V": V = nuevoEstado; break;
            case "L": L = nuevoEstado; break;
            case "O":
            case "I": O = nuevoEstado; break;
            default:
                throw new ArgumentOutOfRangeException(nameof(superficie), "Superficie inválida.");
        }
    }
}

public enum DienteEstado { Presente, Ausente, Implante, Corona, Endodoncia }
public enum SuperficieEstado { Sana, Caries, Obturada, Sellada, Fracturada, Desgastada }
