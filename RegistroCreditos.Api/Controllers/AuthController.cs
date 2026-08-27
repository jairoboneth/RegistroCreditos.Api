using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistroCreditos.Api.Data;
using RegistroCreditos.Api.DTOs;
using RegistroCreditos.Api.Services;

namespace RegistroCreditos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _authService.LoginAsync(loginDto.Email, loginDto.Password);

        if (token == null)
            return Unauthorized("Usuario o contraseña incorrectos.");

        return Ok(new { Token = token });
    }
}
