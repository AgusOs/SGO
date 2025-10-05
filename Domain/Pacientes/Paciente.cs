using SGO.Domain.Common;
using SGO.Domain.Fichas;

namespace SGO.Domain.Pacientes;

/// <summary>
/// Entidad raíz del agregado Paciente.
/// Se identifica por su número de documento.
/// </summary>
public sealed class Paciente : Entity
{
    // --- Identificador natural ---
    public int Documento { get; private set; }

    // --- Datos personales ---
    public string Nombre { get; private set; } = default!;
    public string Apellido { get; private set; } = default!;
    public DateOnly? FechaNacimiento { get; private set; }
    public string? ObraSocial { get; private set; }
    public string? NumeroAfiliado { get; private set; }
    public string? Email { get; private set; }
    public string? Telefono { get; private set; }

    // --- Características clínicas ---
    public bool Alergico { get; private set; }
    public string? DetalleAlergias { get; private set; }
    public bool EnfermedadSistemica { get; private set; }
    public string? DetalleEnfermedad { get; private set; }
    public string? Observaciones { get; private set; }

    // --- Historial de fichas ---
    private readonly List<FichaClinica> _fichasClinicas = new();
    public IReadOnlyCollection<FichaClinica> FichasClinicas => _fichasClinicas.AsReadOnly();

    private Paciente() { } // Para EF Core

    private Paciente(
        int documento,
        string nombre,
        string apellido,
        DateOnly? fechaNacimiento,
        string? obraSocial,
        string? numeroAfiliado,
        string? email,
        string? telefono,
        bool alergico,
        string? detalleAlergias,
        bool enfermedadSistemica,
        string? detalleEnfermedad,
        string? observaciones)
    {
        if (documento <= 0)
            throw new ArgumentException("El documento del paciente es obligatorio.", nameof(documento));
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del paciente es obligatorio.", nameof(nombre));
        if (string.IsNullOrWhiteSpace(apellido))
            throw new ArgumentException("El apellido del paciente es obligatorio.", nameof(apellido));

        Documento = documento;
        Nombre = nombre.Trim();
        Apellido = apellido.Trim();
        FechaNacimiento = fechaNacimiento;
        ObraSocial = obraSocial?.Trim();
        NumeroAfiliado = numeroAfiliado?.Trim();
        Email = email?.Trim();
        Telefono = telefono?.Trim();
        Alergico = alergico;
        DetalleAlergias = detalleAlergias?.Trim();
        EnfermedadSistemica = enfermedadSistemica;
        DetalleEnfermedad = detalleEnfermedad?.Trim();
        Observaciones = observaciones?.Trim();
    }

    public static Paciente CrearNuevo(
        int documento,
        string nombre,
        string apellido,
        DateOnly? fechaNacimiento = null,
        string? obraSocial = null,
        string? numeroAfiliado = null,
        string? email = null,
        string? telefono = null,
        bool alergico = false,
        string? detalleAlergias = null,
        bool enfermedadSistemica = false,
        string? detalleEnfermedad = null,
        string? observaciones = null)
        => new(documento, nombre, apellido, fechaNacimiento, obraSocial, numeroAfiliado,
               email, telefono, alergico, detalleAlergias, enfermedadSistemica,
               detalleEnfermedad, observaciones);

    public void ActualizarContacto(string? email, string? telefono)
    {
        Email = email?.Trim();
        Telefono = telefono?.Trim();
    }

    public void ActualizarDatosClinicos(
        bool alergico,
        string? detalleAlergias,
        bool enfermedadSistemica,
        string? detalleEnfermedad,
        string? observaciones)
    {
        Alergico = alergico;
        DetalleAlergias = detalleAlergias?.Trim();
        EnfermedadSistemica = enfermedadSistemica;
        DetalleEnfermedad = detalleEnfermedad?.Trim();
        Observaciones = observaciones?.Trim();
    }

    public void AgregarFichaClinica(FichaClinica ficha)
    {
        if (ficha is null)
            throw new ArgumentNullException(nameof(ficha));

        _fichasClinicas.Add(ficha);
    }

    public override string ToString() => $"{Apellido}, {Nombre} ({Documento})";
}
