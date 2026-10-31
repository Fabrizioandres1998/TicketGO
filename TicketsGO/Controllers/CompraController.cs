using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsGO.DTOs;
using TicketsGO.Services;

namespace TicketsGO.Controllers;

[Authorize(Roles = "Usuario,Administrador")]
public class CompraController : Controller
{
    private readonly ICompraService _compraService;
    private readonly IEventoService _eventoService;

    public CompraController(ICompraService compraService, IEventoService eventoService)
    {
        _compraService = compraService;
        _eventoService = eventoService;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? idEvento)
    {
        if (idEvento.HasValue && idEvento.Value > 0)
        {
            var evento = await _eventoService.GetByIdAsync(idEvento.Value);
            if (evento != null && evento.Estado == Models.EstadoEvento.Publicado)
            {
                var dto = new CompraCreateDto { IdEvento = evento.Id };
                ViewBag.EventoPreseleccionado = new
                {
                    id = evento.Id,
                    nombre = evento.Nombre,
                    fechaInicio = evento.FechaInicio,
                    ciudad = evento.Ciudad,
                    provincia = evento.Provincia,
                    precioTicket = evento.PrecioTicket,
                    disponibles = evento.TicketsTotales - evento.TicketsVendidos
                };
                return View(dto);
            }
        }

        return View(new CompraCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CompraCreateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _compraService.CrearAsync(dto.IdEvento, userId, dto.TotalTickets);
            TempData["Success"] = "Compra realizada correctamente. Estos son tus tickets.";
            return RedirectToAction("MisTickets", "Ticket");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);

            if (dto.IdEvento > 0)
            {
                var evento = await _eventoService.GetByIdAsync(dto.IdEvento);
                if (evento != null)
                {
                    ViewBag.EventoPreseleccionado = new
                    {
                        id = evento.Id,
                        nombre = evento.Nombre,
                        fechaInicio = evento.FechaInicio,
                        ciudad = evento.Ciudad,
                        provincia = evento.Provincia,
                        precioTicket = evento.PrecioTicket,
                        disponibles = evento.TicketsTotales - evento.TicketsVendidos
                    };
                }
            }

            return View(dto);
        }
    }

    [HttpGet]
    public async Task<IActionResult> MisCompras(int pagina = 1)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var resultado = await _compraService.ListByUsuarioAsync(userId, pagina);

        var items = resultado.Items.Select(c => new CompraListDto
        {
            Id = c.Id,
            Evento = c.Evento?.Nombre ?? "—",
            FechaEvento = c.Evento?.FechaInicio ?? DateTime.MinValue,
            TotalTickets = c.TotalTickets,
            MontoTotal = c.MontoTotal,
            Estado = c.Estado.ToString()
        }).ToList();

        var dto = new PaginatedResult<CompraListDto>
        {
            Items = items,
            TotalCount = resultado.TotalCount,
            PageNumber = resultado.PageNumber,
            PageSize = resultado.PageSize
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _compraService.CancelarAsync(id, userId);
            TempData["Success"] = "Compra cancelada. Los tickets volvieron a estar disponibles.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(MisCompras));
    }
}