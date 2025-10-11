using SGO.Application.Interfaces;
using SGO.Domain.Pacientes;

namespace SGO.Application.UseCases.Pacientes.Commands;

public class ActualizarPacienteCommand
{
    private readonly IPacienteRepository _pacientes;

    public ActualizarPacienteCommand(IPacienteRepository pacientes)
    {
        _pacientes = pacientes;
    }

    public async Task<Paciente> EjecutarAsync(ActualizarPacienteDto dto)
    {
        var paciente = await _pacientes.GetByDocumentoAsync(dto.Documento)
            ?? throw new InvalidOperationException("El paciente no existe.");

        paciente.ActualizarContacto(dto.Email, dto.Telefono);

        paciente.ActualizarDatosClinicos(
            dto.Alergico,
            dto.DetalleAlergias,
            dto.EnfermedadSistemica,
            dto.DetalleEnfermedad,
            dto.Cardiaco,
            dto.Hipertenso,
            dto.Diabetico,
            dto.Hepatitis,
            dto.Mononucleosis,
            dto.EnMedicacion,
            dto.Medicacion,
            dto.Observaciones
        );

        await _pacientes.UpdateAsync(paciente);
        await _pacientes.SaveChangesAsync();

        return paciente;
    }
}

public record ActualizarPacienteDto(
    int Documento,
    string? Email,
    string? Telefono,
    bool Alergico,
    string? DetalleAlergias,
    bool EnfermedadSistemica,
    string? DetalleEnfermedad,
    bool Cardiaco,
    bool Hipertenso,
    bool Diabetico,
    bool Hepatitis,
    bool Mononucleosis,
    bool EnMedicacion,
    string? Medicacion,
    string? Observaciones
);
