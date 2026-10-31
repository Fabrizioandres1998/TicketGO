using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsGO.DTOs;
using TicketsGO.Models;
using TicketsGO.Services;

namespace TicketsGO.Controllers;

public class EventoController : Controller
{
    private readonly IEventoService _eventoService;
    private readonly IFileService _fileService;
    private readonly ILogger<EventoController> _logger;

    public EventoController(IEventoService eventoService, IFileService fileService, ILogger<EventoController> logger)
    {
        _eventoService = eventoService;
        _fileService = fileService;
        _logger = logger;
    }

    // --- LISTADO PÚBLICO (solo publicados) ---

    [AllowAnonymous]
    public async Task<IActionResult> Public(int pagina = 1)
    {
        var resultado = await _eventoService.ListPublicadosAsync(pagina);

        var eventosDto = resultado.Items.Select(e => new EventoListDto
        {
            Id = e.Id,
            Nombre = e.Nombre!,
            FechaInicio = e.FechaInicio,
            Ubicacion = $"{e.Ciudad}, {e.Provincia}",
            Estado = e.Estado.ToString(),
            TicketsTotales = e.TicketsTotales,
            TicketsVendidos = e.TicketsVendidos,
            PrecioTicket = e.PrecioTicket,
            ImagenUrl = e.ImagenUrl,
            Productor = e.Usuario != null ? $"{e.Usuario.Nombre} {e.Usuario.Apellido}" : "—"
        }).ToList();

        var resultadoDto = new PaginatedResult<EventoListDto>
        {
            Items = eventosDto,
            TotalCount = resultado.TotalCount,
            PageNumber = resultado.PageNumber,
            PageSize = resultado.PageSize
        };

        return View(resultadoDto);
    }

    // --- GESTIÓN CON VUE (ABM vía AJAX) ---

    [Authorize(Roles = "Productor,Administrador")]
    [HttpGet]
    public IActionResult GestionVue()
    {
        return View();
    }

    // --- DETALLE ---

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var evento = await _eventoService.GetByIdAsync(id);
        if (evento == null) return NotFound();

        var dto = new EventoDetailsDto
        {
            Id = evento.Id,
            Nombre = evento.Nombre!,
            Descripcion = evento.Descripcion!,
            FechaInicio = evento.FechaInicio,
            FechaFin = evento.FechaFin,
            Provincia = evento.Provincia!,
            Ciudad = evento.Ciudad!,
            Establecimiento = evento.Establecimiento!,
            Privado = evento.Privado,
            TicketsTotales = evento.TicketsTotales,
            TicketsVendidos = evento.TicketsVendidos,
            PrecioTicket = evento.PrecioTicket,
            Estado = evento.Estado.ToString(),
            ImagenUrl = evento.ImagenUrl,
            Productor = evento.Usuario != null ? $"{evento.Usuario.Nombre} {evento.Usuario.Apellido}" : "—",
            IdUsuario = evento.IdUsuario
        };

        return View(dto);
    }

    // --- APROBAR / RECHAZAR (solo admin) ---

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Aprobar(int id)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _eventoService.AprobarAsync(id, userId);
            TempData["Success"] = "Evento aprobado.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(GestionVue));
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rechazar(int id)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _eventoService.RechazarAsync(id, userId);
            TempData["Success"] = "Evento rechazado.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(GestionVue));
    }
}
