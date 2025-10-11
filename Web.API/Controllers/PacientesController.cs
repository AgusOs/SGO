using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGO.Application.Interfaces;
using SGO.Application.UseCases.Pacientes.Commands;

namespace SGO.Web.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PacientesController(
    RegistrarPacienteCommand registrarPaciente,
    IPacienteRepository pacienteRepository,
    ActualizarPacienteCommand actualizarPaciente) : ControllerBase
{
    [HttpGet("{documento:int}")]
    public async Task<IActionResult> ObtenerPorDocumento(int documento)
    {
        var paciente = await pacienteRepository.GetByDocumentoAsync(documento, includeTurnos: true);
        if (paciente == null)
            return NotFound("Paciente no encontrado.");
        return Ok(paciente);
    }

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] PacienteDto dto)
    {
        var paciente = await registrarPaciente.EjecutarAsync(
            dto.Documento, dto.Nombre, dto.Apellido, dto.FechaNacimiento,
            dto.ObraSocial, dto.NumeroAfiliado, dto.Email, dto.Telefono
        );

        return CreatedAtAction(nameof(ObtenerPorDocumento), new { documento = paciente.Documento }, paciente);
    }

    [HttpPut("{documento:int}")]
    public async Task<IActionResult> Actualizar(int documento, [FromBody] ActualizarPacienteDto dto)
    {
        if (dto.Documento != documento)
            return BadRequest("El documento del cuerpo y el de la URL no coinciden.");

        var pacienteActualizado = await actualizarPaciente.EjecutarAsync(dto);
        return Ok(pacienteActualizado);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
        => Ok(await pacienteRepository.GetAllAsync());
}

public record PacienteDto(
    int Documento,
    string Nombre,
    string Apellido,
    DateOnly? FechaNacimiento,
    string? ObraSocial,
    string? NumeroAfiliado,
    string? Email,
    string? Telefono
);
