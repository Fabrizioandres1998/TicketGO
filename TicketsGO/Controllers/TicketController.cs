using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsGO.DTOs;
using TicketsGO.Extensions;
using TicketsGO.Models;
using TicketsGO.Services;

namespace TicketsGO.Controllers;

[Authorize]
public class TicketController : Controller
{
    private readonly ITicketService _ticketService;

    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<IActionResult> MisTickets(int pagina = 1)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var resultado = await _ticketService.ListByUsuarioAsync(userId, pagina);

        var items = resultado.Items.Select(t => new TicketListDto
        {
            Id = t.Id,
            Codigo = t.Codigo ?? "—",
            Evento = t.Evento?.Nombre ?? "—",
            FechaEvento = t.Evento?.FechaInicio ?? DateTime.MinValue,
            Estado = t.Estado.ToString(),
            EstadoDisplayName = t.Estado.GetDisplayName(),
            PuedeTransferir = t.Estado == EstadoTicket.NoUsado
                              && t.Evento != null
                              && t.Evento.FechaInicio > DateTime.Now.AddHours(24),
            PuedeMarcarUsado = t.Estado == EstadoTicket.NoUsado
        }).ToList();

        var dto = new PaginatedResult<TicketListDto>
        {
            Items = items,
            TotalCount = resultado.TotalCount,
            PageNumber = resultado.PageNumber,
            PageSize = resultado.PageSize
        };

        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket == null) return NotFound();

        var puedeGestionarTicket = User.IsInRole("Administrador") ||
                                   (User.IsInRole("Productor") && ticket.Evento?.IdUsuario == userId);

        if (ticket.IdUsuario != userId && !puedeGestionarTicket)
            return Forbid();

        var dto = new TicketDetailsDto
        {
            Id = ticket.Id,
            Codigo = ticket.Codigo ?? "—",
            Evento = ticket.Evento?.Nombre ?? "—",
            FechaEvento = ticket.Evento?.FechaInicio ?? DateTime.MinValue,
            Ubicacion = ticket.Evento != null
                ? $"{ticket.Evento.Establecimiento} — {ticket.Evento.Ciudad}, {ticket.Evento.Provincia}"
                : "—",
            Estado = ticket.Estado.ToString(),
            EstadoDisplayName = ticket.Estado.GetDisplayName(),
            Propietario = ticket.Usuario != null
                ? $"{ticket.Usuario.Nombre} {ticket.Usuario.Apellido}"
                : "—",
            PuedeTransferir = ticket.IdUsuario == userId
                              && ticket.Estado == EstadoTicket.NoUsado
                              && ticket.Evento != null
                              && ticket.Evento.FechaInicio > DateTime.Now.AddHours(24),
            PuedeMarcarUsado = (puedeGestionarTicket || ticket.IdUsuario == userId)
                               && ticket.Estado == EstadoTicket.NoUsado
        };

        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Transferir(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var ticket = await _ticketService.GetByIdAsync(id);
        if (ticket == null) return NotFound();

        if (ticket.IdUsuario != userId) return Forbid();

        if (ticket.Estado == EstadoTicket.Usado)
        {
            TempData["Error"] = "No se puede transferir un ticket ya usado.";
            return RedirectToAction(nameof(MisTickets));
        }

        if (ticket.Evento != null && ticket.Evento.FechaInicio <= DateTime.Now.AddHours(24))
        {
            TempData["Error"] = "Solo se puede transferir hasta 24 horas antes del evento.";
            return RedirectToAction(nameof(MisTickets));
        }

        ViewBag.Id = id;
        ViewBag.Evento = ticket.Evento?.Nombre;
        ViewBag.Codigo = ticket.Codigo;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Transferir(int id, string emailDestino)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (string.IsNullOrWhiteSpace(emailDestino))
                throw new InvalidOperationException("El email del destinatario es obligatorio.");

            await _ticketService.TransferirAsync(id, userId, emailDestino);
            TempData["Success"] = "Ticket transferido correctamente.";
            return RedirectToAction(nameof(MisTickets));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Transferir), new { id });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcarUsado(int id)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _ticketService.MarcarUsadoAsync(id, userId);
            TempData["Success"] = "Ticket marcado como usado.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Details), new { id });
    }
}
