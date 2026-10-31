using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketsGO.Data;

namespace TicketsGO.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize(Policy = "ApiAuthentication")]
public class UsuariosApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(string? email)
    {
        if (string.IsNullOrWhiteSpace(email) || email.Length < 3)
            return Ok(new { items = Array.Empty<object>() });

        var usuarios = await _context.Usuarios
            .Where(u => u.Email!.Contains(email) && u.Estado)
            .Take(10)
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.Nombre,
                u.Apellido
            })
            .ToListAsync();

        return Ok(new { items = usuarios });
    }
}
