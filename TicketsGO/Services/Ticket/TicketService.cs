using Microsoft.EntityFrameworkCore;
using TicketsGO.Data;
using TicketsGO.Models;

namespace TicketsGO.Services;

public class TicketService : ITicketService
{
    private readonly AppDbContext _context;
    private readonly IHistorialService _historial;
    private readonly INotificacionService _notificaciones;
    private readonly ILogger<TicketService> _logger;

    public TicketService(AppDbContext context, IHistorialService historial, INotificacionService notificaciones, ILogger<TicketService> logger)
    {
        _context = context;
        _historial = historial;
        _notificaciones = notificaciones;
        _logger = logger;
    }

    public async Task<PaginatedResult<Ticket>> ListByUsuarioAsync(int idUsuario, int pagina = 1, int cantidadPorPagina = 10)
    {
        var query = _context.Tickets
            .Include(t => t.Evento)
            .Where(t => t.IdUsuario == idUsuario);

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(t => t.Id)
            .Skip((pagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToListAsync();

        return new PaginatedResult<Ticket>
        {
            Items = items,
            TotalCount = total,
            PageNumber = pagina,
            PageSize = cantidadPorPagina
        };
    }

    public async Task<Ticket?> GetByIdAsync(int id)
    {
        return await _context.Tickets
            .Include(t => t.Evento)
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task TransferirAsync(int idTicket, int idUsuarioOrigen, string emailDestino)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Evento)
            .FirstOrDefaultAsync(t => t.Id == idTicket);

        if (ticket == null)
            throw new KeyNotFoundException("El ticket no existe.");

        if (ticket.IdUsuario != idUsuarioOrigen)
            throw new UnauthorizedAccessException("El ticket no te pertenece.");

        if (ticket.Estado == EstadoTicket.Usado)
            throw new InvalidOperationException("No se puede transferir un ticket ya usado.");

        if (ticket.Evento != null && ticket.Evento.FechaInicio <= DateTime.Now.AddHours(24))
            throw new InvalidOperationException("Solo se puede transferir hasta 24 horas antes del evento.");

        var destino = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == emailDestino);
        if (destino == null)
            throw new InvalidOperationException("El usuario destino no existe.");

        if (destino.Id == idUsuarioOrigen)
            throw new InvalidOperationException("No podés transferirte un ticket a vos mismo.");

        var yaTiene = await _context.Tickets
            .CountAsync(t => t.IdEvento == ticket.IdEvento && t.IdUsuario == destino.Id);

        if (yaTiene >= 5)
            throw new InvalidOperationException("El usuario destino ya tiene el máximo de tickets para este evento.");

        ticket.IdUsuario = destino.Id;
        await _context.SaveChangesAsync();

        _context.Transferencias.Add(new Transferencia
        {
            IdTicket = ticket.Id,
            IdUsuarioOrigen = idUsuarioOrigen,
            IdUsuarioDestino = destino.Id,
            Fecha = DateTime.Now
        });
        await _context.SaveChangesAsync();

        await _historial.RegistrarAsync(ticket.IdEvento, idUsuarioOrigen,
            $"Transferencia del ticket {ticket.Codigo} a {destino.Email}.");
        await _historial.RegistrarAsync(ticket.IdEvento, destino.Id,
            $"Recepción del ticket {ticket.Codigo} desde {ticket.Evento?.Nombre ?? "un evento"}.");
        await _notificaciones.CrearAsync(idUsuarioOrigen, "Ticket transferido",
            $"Transferiste un ticket de '{ticket.Evento?.Nombre ?? "un evento"}' a {destino.Email}.", "/Ticket/MisTickets");
        await _notificaciones.CrearAsync(destino.Id, "Recibiste un ticket",
            $"Recibiste un ticket para '{ticket.Evento?.Nombre ?? "un evento"}'.", "/Ticket/MisTickets");
    }

    public async Task MarcarUsadoAsync(int idTicket, int idUsuario)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Evento)
            .FirstOrDefaultAsync(t => t.Id == idTicket);

        if (ticket == null)
            throw new KeyNotFoundException("El ticket no existe.");

        if (ticket.Evento == null)
            throw new InvalidOperationException("El ticket no tiene evento asociado.");

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == idUsuario);
        if (usuario == null)
            throw new KeyNotFoundException("El usuario no existe.");

        var esAdmin = usuario.Rol == RolUsuario.Administrador;
        var esProductorDelEvento = ticket.Evento.IdUsuario == idUsuario;
        var esPropietario = ticket.IdUsuario == idUsuario;

        if (!esAdmin && !esProductorDelEvento && !esPropietario)
            throw new UnauthorizedAccessException("Solo el titular del ticket, el productor del evento o un administrador pueden marcarlo como usado.");

        if (ticket.Evento.FechaFin <= DateTime.Now)
            throw new InvalidOperationException("El evento ya está finalizado.");

        if (ticket.Estado == EstadoTicket.Usado)
            throw new InvalidOperationException("El ticket ya fue usado.");

        ticket.Estado = EstadoTicket.Usado;
        await _context.SaveChangesAsync();

        await _historial.RegistrarAsync(ticket.IdEvento, idUsuario,
            $"Ticket {ticket.Codigo} marcado como usado.");
        if (ticket.IdUsuario.HasValue && ticket.IdUsuario.Value != idUsuario)
        {
            await _notificaciones.CrearAsync(ticket.IdUsuario.Value, "Ticket marcado como usado",
                $"Tu ticket para '{ticket.Evento.Nombre}' fue marcado como usado.", $"/Ticket/Details/{ticket.Id}");
        }
    }
}
