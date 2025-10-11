using SGO.Domain.Common;
using SGO.Domain.Fichas;
using SGO.Domain.Turnos;

namespace SGO.Domain.Pacientes;

/// <summary>
/// Entidad raíz del agregado Paciente.
/// Representa a una persona atendida en el consultorio odontológico.
/// Contiene sus datos personales, características clínicas y su historial de fichas.
/// </summary>
public sealed class Paciente : Entity
{
    // --- Identificación ---
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
    public bool Cardiaco { get; private set; }
    public bool Hipertenso { get; private set; }
    public bool Diabetico { get; private set; }
    public bool Hepatitis { get; private set; }
    public bool Mononucleosis { get; private set; }
    public bool EnMedicacion { get; private set; }
    public string? Medicacion { get; private set; }
    public string? Observaciones { get; private set; }

    // --- Historial de fichas clínicas ---
    private readonly List<FichaClinica> _fichasClinicas = new();
    public IReadOnlyCollection<FichaClinica> FichasClinicas => _fichasClinicas.AsReadOnly();

    // --- Historial de turnos (solo lectura) ---
    private readonly List<Turno> _turnos = new();
    public IReadOnlyCollection<Turno> Turnos => _turnos.AsReadOnly();

    // --- Constructor privado para EF Core ---
    private Paciente() { }

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
        bool cardiaco,
        bool hipertenso,
        bool diabetico,
        bool hepatitis,
        bool mononucleosis,
        bool enMedicacion,
        string? medicacion,
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
        Cardiaco = cardiaco;
        Hipertenso = hipertenso;
        Diabetico = diabetico;
        Hepatitis = hepatitis;
        Mononucleosis = mononucleosis;
        EnMedicacion = enMedicacion;
        Medicacion = medicacion?.Trim();
        Observaciones = observaciones?.Trim();
    }

    public static Paciente CrearNuevo(
        int documento,
        string nombre,
        string apellido,
        DateOnly? fechaNacimiento,
        string? obraSocial,
        string? numeroAfiliado,
        string? email,
        string? telefono,
        bool alergico = false,
        string? detalleAlergias = null,
        bool enfermedadSistemica = false,
        string? detalleEnfermedad = null,
        bool cardiaco = false,
        bool hipertenso = false,
        bool diabetico = false,
        bool hepatitis = false,
        bool mononucleosis = false,
        bool enMedicacion = false,
        string? medicacion = null,
        string? observaciones = null)
        => new(documento, nombre, apellido, fechaNacimiento, obraSocial, numeroAfiliado, email, telefono,
               alergico, detalleAlergias, enfermedadSistemica, detalleEnfermedad, cardiaco, hipertenso,
               diabetico, hepatitis, mononucleosis, enMedicacion, medicacion, observaciones);

    // --- Métodos de dominio ---
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
        bool cardiaco,
        bool hipertenso,
        bool diabetico,
        bool hepatitis,
        bool mononucleosis,
        bool enMedicacion,
        string? medicacion,
        string? observaciones)
    {
        Alergico = alergico;
        DetalleAlergias = detalleAlergias?.Trim();
        EnfermedadSistemica = enfermedadSistemica;
        DetalleEnfermedad = detalleEnfermedad?.Trim();
        Cardiaco = cardiaco;
        Hipertenso = hipertenso;
        Diabetico = diabetico;
        Hepatitis = hepatitis;
        Mononucleosis = mononucleosis;
        EnMedicacion = enMedicacion;
        Medicacion = medicacion?.Trim();
        Observaciones = observaciones?.Trim();
    }

    public void AgregarFichaClinica(FichaClinica ficha)
    {
        if (ficha is null)
            throw new ArgumentNullException(nameof(ficha));
        _fichasClinicas.Add(ficha);
    }

    /// Verifica si el paciente tiene inasistencias en el último mes
    public bool TieneInasistenciasRecientes(int maxPermitidas = 1)
        => _turnos.Count(t => t.Estado == EstadoTurno.NoAsistio && t.FechaHora > DateTime.UtcNow.AddMonths(-1) && t.FechaHora < DateTime.UtcNow) >= maxPermitidas;

    public override string ToString() => $"{Apellido}, {Nombre} ({Documento})";
}
