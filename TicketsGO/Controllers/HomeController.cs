using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult Index()
    {
        return View();
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