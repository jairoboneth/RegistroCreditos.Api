using System.Data;
using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroCreditos.Api.Data;
using RegistroCreditos.Api.DTOs;
using RegistroCreditos.Api.DTOs.Credito;
using RegistroCreditos.Api.Models;
using RegistroCreditos.Api.Services;
namespace RegistroCreditos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CreditosController : ControllerBase
{
    private readonly ICreditoService _creditoService;
    private readonly ICreditoQueryService _creditoQueryService;

    public CreditosController(ICreditoService creditoService, ICreditoQueryService creditoQueryService)
    {
        _creditoService = creditoService;
        _creditoQueryService = creditoQueryService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CrearCreditoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out int usuarioId))
        {
            return Unauthorized("Usuario no vÃ¡lido.");
        }

        var nombreUsuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "";

        var resultDto = await _creditoService.CreateCreditoAsync(dto, usuarioId, nombreUsuario);

        return CreatedAtAction(nameof(Get), new { id = resultDto.Id }, resultDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _creditoQueryService.GetCreditoByIdAsync(id);
        
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? filter, [FromQuery] string? sortBy)
    {
        var results = await _creditoQueryService.GetAllCreditosAsync(filter, sortBy);
        return Ok(results);
    }
}
