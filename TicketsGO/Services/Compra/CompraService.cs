using Microsoft.EntityFrameworkCore;
using TicketsGO.Data;
using TicketsGO.Models;

namespace TicketsGO.Services;

public class CompraService : ICompraService
{
    private readonly AppDbContext _context;
    private readonly IHistorialService _historial;
    private readonly INotificacionService _notificaciones;
    private readonly ILogger<CompraService> _logger;

    public CompraService(AppDbContext context, IHistorialService historial, INotificacionService notificaciones, ILogger<CompraService> logger)
    {
        _context = context;
        _historial = historial;
        _notificaciones = notificaciones;
        _logger = logger;
    }

    public async Task<PaginatedResult<Compra>> ListByUsuarioAsync(int idUsuario, int pagina = 1, int cantidadPorPagina = 10)
    {
        var query = _context.Compras
            .Include(c => c.Evento)
            .Where(c => c.IdUsuario == idUsuario);

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(c => c.Id)
            .Skip((pagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToListAsync();

        return new PaginatedResult<Compra>
        {
            Items = items,
            TotalCount = total,
            PageNumber = pagina,
            PageSize = cantidadPorPagina
        };
    }

    public async Task<Compra?> GetByIdAsync(int id)
    {
        return await _context.Compras
            .Include(c => c.Evento)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Compra> CrearAsync(int idEvento, int idUsuario, int cantidad)
    {
        var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == idEvento);
        if (evento == null)
            throw new KeyNotFoundException("El evento no existe.");

        if (evento.FechaFin <= DateTime.Now)
            throw new InvalidOperationException("El evento ya está finalizado.");

        if (evento.Estado != EstadoEvento.Publicado)
            throw new InvalidOperationException("Solo se pueden comprar tickets de eventos publicados.");

        var disponibles = evento.TicketsTotales - evento.TicketsVendidos;

        if (disponibles <= 0)
            throw new InvalidOperationException("No quedan tickets disponibles para este evento.");

        if (cantidad > disponibles)
            throw new InvalidOperationException($"Solo quedan {disponibles} ticket(s) disponibles. No podés comprar {cantidad}.");

        var yaComprados = await _context.Tickets
            .CountAsync(t => t.IdEvento == idEvento && t.IdUsuario == idUsuario);

        if (yaComprados + cantidad > 5)
            throw new InvalidOperationException("No podés tener más de 5 tickets para este evento.");

        var compra = new Compra
        {
            IdEvento = idEvento,
            IdUsuario = idUsuario,
            TotalTickets = cantidad,
            MontoTotal = cantidad * evento.PrecioTicket,
            Estado = EstadoCompra.Realizada
        };

        _context.Compras.Add(compra);
        await _context.SaveChangesAsync();

        for (int i = 0; i < cantidad; i++)
        {
            _context.Tickets.Add(new Ticket
            {
                IdEvento = idEvento,
                IdCompra = compra.Id,
                IdUsuario = idUsuario,
                Estado = EstadoTicket.NoUsado,
                Codigo = Guid.NewGuid().ToString("N").ToUpper()
            });
        }

        evento.TicketsVendidos += cantidad;
        await _context.SaveChangesAsync();

        await _historial.RegistrarAsync(idEvento, idUsuario,
            $"Compra de {cantidad} ticket(s) para el evento '{evento.Nombre}'.");
        await _notificaciones.CrearAsync(idUsuario, "Compra confirmada",
            $"Compraste {cantidad} ticket(s) para '{evento.Nombre}'.", "/Compra/MisCompras");
        if (evento.IdUsuario != idUsuario)
        {
            await _notificaciones.CrearAsync(evento.IdUsuario, "Nueva compra",
                $"Se compraron {cantidad} ticket(s) para tu evento '{evento.Nombre}'.", $"/Evento/Details/{evento.Id}");
        }

        _logger.LogInformation("Compra {Id} creada por usuario {UsuarioId} para evento {EventoId}", compra.Id, idUsuario, idEvento);
        return compra;
    }

    public async Task CancelarAsync(int id, int idUsuario)
    {
        var compra = await _context.Compras
            .Include(c => c.Evento)
            .FirstOrDefaultAsync(c => c.Id == id && c.IdUsuario == idUsuario);

        if (compra == null)
            throw new KeyNotFoundException("La compra no existe.");

        if (compra.Estado == EstadoCompra.Cancelada)
            throw new InvalidOperationException("La compra ya está cancelada.");

        var tickets = await _context.Tickets
            .Where(t => t.IdCompra == id)
            .ToListAsync();
        var idsTickets = tickets.Select(t => t.Id).ToList();
        var tieneTicketsTransferidos = idsTickets.Count > 0 && await _context.Transferencias
            .AnyAsync(t => idsTickets.Contains(t.IdTicket));

        if (tieneTicketsTransferidos)
            throw new InvalidOperationException("No se puede devolver esta compra porque uno o más tickets ya fueron transferidos.");

        var evento = compra.Evento;
        if (evento != null)
        {
            if (evento.FechaInicio <= DateTime.Now.AddHours(48))
                throw new InvalidOperationException("Solo se puede devolver hasta 48 horas antes del evento.");

            evento.TicketsVendidos -= compra.TotalTickets;
        }

        _context.Tickets.RemoveRange(tickets);

        compra.Estado = EstadoCompra.Cancelada;
        await _context.SaveChangesAsync();

        await _historial.RegistrarAsync(compra.IdEvento, idUsuario,
            $"Devolución de {compra.TotalTickets} ticket(s) de la compra #{compra.Id}.");
        await _notificaciones.CrearAsync(idUsuario, "Devolución confirmada",
            $"Se canceló la compra #{compra.Id} de {compra.TotalTickets} ticket(s) para '{evento?.Nombre ?? "el evento"}'.",
            "/Compra/MisCompras");
        if (evento != null && evento.IdUsuario != idUsuario)
        {
            await _notificaciones.CrearAsync(evento.IdUsuario, "Tickets devueltos",
                $"Se devolvieron {compra.TotalTickets} ticket(s) de tu evento '{evento.Nombre}'.", $"/Evento/Details/{evento.Id}");
        }
    }
}
