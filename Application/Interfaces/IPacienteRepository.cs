using SGO.Domain.Pacientes;

namespace SGO.Application.Interfaces;

public interface IPacienteRepository
{
    Task<Paciente?> GetByDocumentoAsync(int documento, bool includeTurnos = false);
    Task<List<Paciente>> GetAllAsync();
    Task AddAsync(Paciente paciente);
    Task UpdateAsync(Paciente paciente);
    Task<bool> ExistsAsync(int documento);
    Task SaveChangesAsync();
}
