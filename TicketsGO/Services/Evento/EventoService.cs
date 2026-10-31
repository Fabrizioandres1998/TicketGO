using Microsoft.EntityFrameworkCore;
using TicketsGO.Data;
using TicketsGO.Models;

namespace TicketsGO.Services;

public class EventoService : IEventoService
{
    private readonly AppDbContext _context;
    private readonly ILogger<EventoService> _logger;
    private readonly IHistorialService _historial;
    private readonly INotificacionService _notificaciones;

    public EventoService(AppDbContext context, ILogger<EventoService> logger, IHistorialService historial, INotificacionService notificaciones)
    {
        _context = context;
        _logger = logger;
        _historial = historial;
        _notificaciones = notificaciones;
    }

    // --- LISTADOS ---

    public async Task<PaginatedResult<Evento>> ListAsync(int pagina = 1, int cantidadPorPagina = 10, string? busqueda = null)
    {
        await ActualizarEstadosVencidosAsync();

        var query = _context.Eventos
            .Include(e => e.Usuario)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var termino = busqueda.Trim();
            query = query.Where(e =>
                (e.Nombre != null && e.Nombre.Contains(termino)) ||
                (e.Usuario != null &&
                    ((e.Usuario.Nombre != null && e.Usuario.Nombre.Contains(termino)) ||
                     (e.Usuario.Apellido != null && e.Usuario.Apellido.Contains(termino)) ||
                     ((e.Usuario.Nombre ?? "") + " " + (e.Usuario.Apellido ?? "")).Contains(termino) ||
                     (e.Usuario.Email != null && e.Usuario.Email.Contains(termino)))));
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(e => e.FechaInicio)
            .Skip((pagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToListAsync();

        return new PaginatedResult<Evento>
        {
            Items = items,
            TotalCount = total,
            PageNumber = pagina,
            PageSize = cantidadPorPagina
        };
    }

    public async Task<PaginatedResult<Evento>> BuscarPublicadosAsync(string? nombre, int pagina = 1, int cantidadPorPagina = 10)
    {
        await ActualizarEstadosVencidosAsync();

        var query = _context.Eventos
            .Include(e => e.Usuario)
            .Where(e => e.Estado == EstadoEvento.Publicado);

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(e => e.Nombre!.Contains(nombre));

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(e => e.FechaInicio)
            .Skip((pagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToListAsync();

        return new PaginatedResult<Evento>
        {
            Items = items,
            TotalCount = total,
            PageNumber = pagina,
            PageSize = cantidadPorPagina
        };
    }

    public async Task<PaginatedResult<Evento>> ListPublicadosAsync(int pagina = 1, int cantidadPorPagina = 10)
    {
        await ActualizarEstadosVencidosAsync();

        var query = _context.Eventos
            .Include(e => e.Usuario)
            .Where(e => e.Estado == EstadoEvento.Publicado);

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(e => e.FechaInicio)
            .Skip((pagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToListAsync();

        return new PaginatedResult<Evento>
        {
            Items = items,
            TotalCount = total,
            PageNumber = pagina,
            PageSize = cantidadPorPagina
        };
    }

    public async Task<PaginatedResult<Evento>> ListByUsuarioAsync(int idUsuario, int pagina = 1, int cantidadPorPagina = 10)
    {
        await ActualizarEstadosVencidosAsync();

        var query = _context.Eventos
            .Include(e => e.Usuario)
            .Where(e => e.IdUsuario == idUsuario);

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(e => e.FechaInicio)
            .Skip((pagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToListAsync();

        return new PaginatedResult<Evento>
        {
            Items = items,
            TotalCount = total,
            PageNumber = pagina,
            PageSize = cantidadPorPagina
        };
    }

    // --- OBTENER ---

    public async Task<Evento?> GetByIdAsync(int id)
    {
        return await _context.Eventos
            .Include(e => e.Usuario)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    // --- CREAR ---

    public async Task<Evento> CreateAsync(Evento evento, int idUsuario)
    {
        if (evento == null) throw new ArgumentNullException(nameof(evento));

        if (evento.FechaInicio < DateTime.Now)
            throw new InvalidOperationException("La fecha de inicio no puede ser anterior a la actual.");

        if (evento.FechaFin <= evento.FechaInicio)
            throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio.");

        if (evento.TicketsTotales <= 0)
            throw new InvalidOperationException("La cantidad de tickets debe ser mayor a 0.");

        evento.IdUsuario = idUsuario;
        evento.TicketsVendidos = 0;
        evento.Estado = EstadoEvento.Publicado;

        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();

        await _historial.RegistrarAsync(evento.Id, idUsuario, $"Creación del evento '{evento.Nombre}'.");
        await _notificaciones.CrearAsync(idUsuario, "Evento creado",
            $"El evento '{evento.Nombre}' fue creado correctamente.", $"/Evento/Details/{evento.Id}");

        _logger.LogInformation("Evento {Id} creado por usuario {UsuarioId}", evento.Id, idUsuario);
        return evento;
    }

    // --- MODIFICAR ---

    public async Task UpdateAsync(Evento evento, int idUsuario, bool esAdmin)
    {
        var eventoDb = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == evento.Id);
        if (eventoDb == null) throw new KeyNotFoundException("El evento no existe.");

        if (!esAdmin && eventoDb.IdUsuario != idUsuario)
            throw new UnauthorizedAccessException("No podés modificar un evento que no te pertenece.");

        if (eventoDb.FechaInicio <= DateTime.Now.AddHours(24))
            throw new InvalidOperationException("Solo se puede modificar el evento hasta 24 horas antes de su inicio.");

        if (eventoDb.Estado == EstadoEvento.Finalizado || eventoDb.Estado == EstadoEvento.Cancelado)
            throw new InvalidOperationException("No se puede modificar un evento finalizado o cancelado.");

        if (evento.FechaInicio < DateTime.Now)
            throw new InvalidOperationException("La fecha de inicio no puede ser anterior a la actual.");

        if (evento.FechaFin <= evento.FechaInicio)
            throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio.");

        if (evento.TicketsTotales < eventoDb.TicketsVendidos)
            throw new InvalidOperationException("No podés reducir los tickets por debajo de los ya vendidos.");

        eventoDb.Nombre = evento.Nombre;
        eventoDb.Descripcion = evento.Descripcion;
        eventoDb.FechaInicio = evento.FechaInicio;
        eventoDb.FechaFin = evento.FechaFin;
        eventoDb.Provincia = evento.Provincia;
        eventoDb.Ciudad = evento.Ciudad;
        eventoDb.Establecimiento = evento.Establecimiento;
        eventoDb.Privado = evento.Privado;
        eventoDb.TicketsTotales = evento.TicketsTotales;
        eventoDb.PrecioTicket = evento.PrecioTicket;

        await _context.SaveChangesAsync();
        await _historial.RegistrarAsync(eventoDb.Id, idUsuario, $"Modificación del evento '{eventoDb.Nombre}'.");
        _logger.LogInformation("Evento {Id} modificado por usuario {UsuarioId}", evento.Id, idUsuario);
    }

    // --- ELIMINAR (cancelar) ---

    public async Task DeleteAsync(int id, int idUsuario, bool esAdmin)
    {
        var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
        if (evento == null) throw new KeyNotFoundException("El evento no existe.");

        if (!esAdmin && evento.IdUsuario != idUsuario)
            throw new UnauthorizedAccessException("No podés cancelar un evento que no te pertenece.");

        if (evento.Estado == EstadoEvento.Finalizado)
            throw new InvalidOperationException("No se puede cancelar un evento finalizado.");

        var usuariosAfectados = await _context.Tickets
            .Where(t => t.IdEvento == id && t.IdUsuario.HasValue)
            .Select(t => t.IdUsuario!.Value)
            .Distinct()
            .ToListAsync();

        evento.Estado = EstadoEvento.Cancelado;
        await _context.SaveChangesAsync();

        await _historial.RegistrarAsync(evento.Id, idUsuario, $"Cancelación del evento '{evento.Nombre}'.");
        foreach (var usuarioAfectado in usuariosAfectados.Where(uid => uid != idUsuario))
        {
            await _notificaciones.CrearAsync(usuarioAfectado, "Evento cancelado",
                $"El evento '{evento.Nombre}' fue cancelado.", $"/Evento/Details/{evento.Id}");
        }

        _logger.LogWarning("Evento {Id} cancelado por usuario {UsuarioId}", id, idUsuario);
    }

    // --- APROBAR / RECHAZAR (admin) ---

    public async Task AprobarAsync(int id, int idUsuario)
    {
        var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
        if (evento == null) throw new KeyNotFoundException("El evento no existe.");

        if (evento.Estado != EstadoEvento.EnRevision)
            throw new InvalidOperationException("Solo se pueden aprobar eventos en revisión.");

        evento.Estado = EstadoEvento.Publicado;
        await _context.SaveChangesAsync();
        await _historial.RegistrarAsync(evento.Id, idUsuario, $"Aprobación y publicación del evento '{evento.Nombre}'.");
        await _notificaciones.CrearAsync(evento.IdUsuario, "Evento publicado",
            $"Tu evento '{evento.Nombre}' fue aprobado y publicado.", $"/Evento/Details/{evento.Id}");
        _logger.LogInformation("Evento {Id} aprobado", id);
    }

    public async Task RechazarAsync(int id, int idUsuario)
    {
        var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
        if (evento == null) throw new KeyNotFoundException("El evento no existe.");

        if (evento.Estado != EstadoEvento.EnRevision)
            throw new InvalidOperationException("Solo se pueden rechazar eventos en revisión.");

        evento.Estado = EstadoEvento.Cancelado;
        await _context.SaveChangesAsync();
        await _historial.RegistrarAsync(evento.Id, idUsuario, $"Rechazo del evento '{evento.Nombre}'.");
        await _notificaciones.CrearAsync(evento.IdUsuario, "Evento rechazado",
            $"Tu evento '{evento.Nombre}' no fue aprobado.", $"/Evento/Details/{evento.Id}");
        _logger.LogInformation("Evento {Id} rechazado", id);
    }

    public async Task CancelarAsync(int id, int idUsuario, bool esAdmin)
    {
        await DeleteAsync(id, idUsuario, esAdmin);
    }

    // --- IMAGEN ---

    public async Task ActualizarImagenAsync(int id, string imagenUrl)
    {
        var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
        if (evento == null) throw new KeyNotFoundException("El evento no existe.");

        evento.ImagenUrl = imagenUrl;
        await _context.SaveChangesAsync();
    }

    // --- ESTADOS VENCIDOS ---

    private async Task ActualizarEstadosVencidosAsync()
    {
        var ahora = DateTime.Now;

        var vencidos = await _context.Eventos
            .Where(e => e.Estado == EstadoEvento.Publicado && e.FechaFin < ahora)
            .ToListAsync();

        if (vencidos.Any())
        {
            foreach (var e in vencidos)
                e.Estado = EstadoEvento.Finalizado;

            await _context.SaveChangesAsync();

            var idsVencidos = vencidos.Select(e => e.Id).ToList();
            var compradoresPorEvento = await _context.Tickets
                .Where(t => idsVencidos.Contains(t.IdEvento) && t.IdUsuario.HasValue)
                .Select(t => new { t.IdEvento, IdUsuario = t.IdUsuario!.Value })
                .Distinct()
                .ToListAsync();

            foreach (var evento in vencidos)
            {
                await _notificaciones.CrearAsync(evento.IdUsuario, "Evento finalizado",
                    $"El evento '{evento.Nombre}' ya finalizó.", $"/Evento/Details/{evento.Id}");

                foreach (var usuarioId in compradoresPorEvento
                             .Where(c => c.IdEvento == evento.Id && c.IdUsuario != evento.IdUsuario)
                             .Select(c => c.IdUsuario))
                {
                    await _notificaciones.CrearAsync(usuarioId, "Evento finalizado",
                        $"El evento '{evento.Nombre}' ya finalizó.", $"/Evento/Details/{evento.Id}");
                }
            }

            _logger.LogInformation("Se finalizaron {Count} eventos vencidos", vencidos.Count);
        }
    }
}
