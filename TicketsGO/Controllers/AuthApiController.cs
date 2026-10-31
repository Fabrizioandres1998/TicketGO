using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsGO.DTOs;
using TicketsGO.Services;

namespace TicketsGO.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthApiController(IUsuarioService usuarioService, IJwtTokenService jwtTokenService)
    {
        _usuarioService = usuarioService;
        _jwtTokenService = jwtTokenService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDto dto)
    {
        try
        {
            var usuario = await _usuarioService.LoginAsync(dto.Email, dto.Password);
            if (usuario == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas." });

            var (token, expiresAtUtc) = _jwtTokenService.CreateToken(usuario);
            return Ok(new
            {
                accessToken = token,
                tokenType = "Bearer",
                expiresAtUtc,
                user = new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Apellido,
                    usuario.Email,
                    role = usuario.Rol.ToString()
                }
            });
        }
        catch (InvalidOperationException)
        {
            return Unauthorized(new { mensaje = "Credenciales inválidas." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { mensaje = ex.Message });
        }
    }

    [Authorize(Policy = "ApiAuthentication")]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            id = User.FindFirstValue(ClaimTypes.NameIdentifier),
            nombre = User.FindFirstValue(ClaimTypes.Name),
            email = User.FindFirstValue(ClaimTypes.Email),
            rol = User.FindFirstValue(ClaimTypes.Role)
        });
    }
}
