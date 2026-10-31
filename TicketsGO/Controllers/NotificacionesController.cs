using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsGO.Services;

namespace TicketsGO.Controllers;

[Authorize]
public class NotificacionesController : Controller
{
    private readonly INotificacionService _notificacionService;

    public NotificacionesController(INotificacionService notificacionService)
    {
        _notificacionService = notificacionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int pagina = 1)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var resultado = await _notificacionService.ListByUsuarioAsync(userId, pagina);
        ViewBag.NoLeidas = await _notificacionService.ContarNoLeidasAsync(userId);
        return View(resultado);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Abrir(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        try
        {
            var destino = await _notificacionService.MarcarLeidaAsync(id, userId);
            if (!string.IsNullOrWhiteSpace(destino) && Url.IsLocalUrl(destino))
                return LocalRedirect(destino);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcarTodasLeidas()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _notificacionService.MarcarTodasLeidasAsync(userId);
        return RedirectToAction(nameof(Index));
    }
}
