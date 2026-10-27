using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsGO.DTOs;
using TicketsGO.Services;

namespace TicketsGO.Controllers;

public class HomeController : Controller
{
    private readonly IEventoService _eventoService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IEventoService eventoService, ILogger<HomeController> logger)
    {
        _eventoService = eventoService;
        _logger = logger;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(int pagina = 1)
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

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Buscar(string? nombre, int pagina = 1)
    {
        var resultado = await _eventoService.BuscarPublicadosAsync(nombre, pagina);

        var items = resultado.Items.Select(e => new
        {
            e.Id,
            e.Nombre,
            e.FechaInicio,
            e.Ciudad,
            e.Provincia,
            e.PrecioTicket,
            e.ImagenUrl
        });

        return Json(new
        {
            items,
            totalCount = resultado.TotalCount,
            pageNumber = resultado.PageNumber,
            pageSize = resultado.PageSize
        });
    }
}