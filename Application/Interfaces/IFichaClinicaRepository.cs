using SGO.Domain.Fichas;

namespace SGO.Application.Interfaces;

public interface IFichaClinicaRepository
{
    Task<FichaClinica?> GetByIdAsync(Guid id);
    Task AddAsync(FichaClinica ficha);
    Task SaveChangesAsync();
    Task<List<FichaClinica>> GetByPacienteAsync(int documentoPaciente);
}