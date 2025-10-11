using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGO.Application.Interfaces;
using SGO.Application.UseCases.Turnos.Commands;
using SGO.Domain.Turnos;

namespace SGO.Web.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TurnosController : ControllerBase
{
    private readonly ITurnoRepository _repository;
    private readonly RegistrarTurnoCommand _registrarTurno;

    public TurnosController(ITurnoRepository repository, RegistrarTurnoCommand registrarTurno)
    {
        _repository = repository;
        _registrarTurno = registrarTurno;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(Guid id)
    {
        var turno = await _repository.GetByIdAsync(id);
        if (turno is null)
            return NotFound("Turno no encontrado.");
        return Ok(turno);
    }

    [HttpGet("paciente/{documento:int}")]
    public async Task<IActionResult> ObtenerPorPaciente(int documento)
        => Ok(await _repository.GetByPacienteAsync(documento));

    [HttpGet("fecha/{fecha:datetime}")]
    public async Task<IActionResult> ObtenerPorFecha(DateTime fecha)
    {
        var turnos = await _repository.GetByFechaAsync(DateOnly.FromDateTime(fecha));
        return Ok(turnos);
    }

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] RegistrarTurnoDto dto)
    {
        var turno = await _registrarTurno.EjecutarAsync(
            dto.PacienteDocumento,
            dto.ProfesionalMatricula,
            dto.FechaHora,
            dto.Motivo
        );

        return CreatedAtAction(nameof(ObtenerPorId), new { id = turno.Id }, turno);
    }

    [HttpPut("{id:guid}/estado")]
    public async Task<IActionResult> CambiarEstado(Guid id, [FromBody] ActualizarEstadoTurnoDto dto)
    {
        var turno = await _repository.GetByIdAsync(id);
        if (turno is null)
            return NotFound();

        switch (dto.Estado)
        {
            case EstadoTurno.Atendido: turno.MarcarComoAtendido(); break;
            case EstadoTurno.Cancelado: turno.Cancelar(dto.Observaciones); break;
            case EstadoTurno.NoAsistio: turno.MarcarComoNoAsistido(dto.Observaciones); break;
            case EstadoTurno.Reprogramado:
                if (dto.NuevaFechaHora is null)
                    return BadRequest("Debe indicar la nueva fecha para reprogramar.");
                turno.Reprogramar(dto.NuevaFechaHora.Value);
                break;
        }

        await _repository.UpdateAsync(turno);
        await _repository.SaveChangesAsync();

        return Ok(turno);
    }
}

public record RegistrarTurnoDto(
    int PacienteDocumento,
    int ProfesionalMatricula,
    DateTime FechaHora,
    string? Motivo
);

public record ActualizarEstadoTurnoDto(
    EstadoTurno Estado,
    string? Observaciones,
    DateTime? NuevaFechaHora
);
