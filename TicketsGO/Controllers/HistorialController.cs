using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsGO.Services;

namespace TicketsGO.Controllers;

[Authorize]
public class HistorialController : Controller
{
    private readonly IHistorialService _historialService;
    private readonly IEventoService _eventoService;

    public HistorialController(IHistorialService historialService, IEventoService eventoService)
    {
        _historialService = historialService;
        _eventoService = eventoService;
    }

    [HttpGet]
    public async Task<IActionResult> MisMovimientos(int pagina = 1)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var resultado = await _historialService.ListByUsuarioAsync(userId, pagina);
        return View(resultado);
    }

    [HttpGet]
    public async Task<IActionResult> PorEvento(int idEvento, int pagina = 1)
    {
        var evento = await _eventoService.GetByIdAsync(idEvento);
        if (evento == null) return NotFound();

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (evento.IdUsuario != userId && !User.IsInRole("Administrador"))
            return Forbid();

        ViewBag.NombreEvento = evento.Nombre;
        ViewBag.IdEvento = evento.Id;
        var resultado = await _historialService.ListByEventoAsync(idEvento, pagina);
        return View(resultado);
    }
}
