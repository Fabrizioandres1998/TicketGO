using Microsoft.EntityFrameworkCore;
using TicketsGO.Data;
using TicketsGO.Models;

namespace TicketsGO.Services;

public class EventoService : IEventoService
{
    private readonly AppDbContext _context;
    private readonly ILogger<EventoService> _logger;

    public EventoService(AppDbContext context, ILogger<EventoService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // --- LISTADOS ---

    public async Task<PaginatedResult<Evento>> ListAsync(int pagina = 1, int cantidadPorPagina = 10)
    {
        var total = await _context.Eventos.CountAsync();

        var items = await _context.Eventos
            .Include(e => e.Usuario)
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

        // RN-05: fecha de inicio no puede ser pasada
        if (evento.FechaInicio < DateTime.Now)
            throw new InvalidOperationException("La fecha de inicio no puede ser anterior a la actual.");

        if (evento.FechaFin <= evento.FechaInicio)
            throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio.");

        // RN-08: capacidad
        if (evento.TicketsTotales <= 0)
            throw new InvalidOperationException("La cantidad de tickets debe ser mayor a 0.");

        evento.IdUsuario = idUsuario;
        evento.TicketsVendidos = 0;

        // RN-07: productor o admin publica directo; usuario no puede crear (se valida en controller)
        evento.Estado = EstadoEvento.Publicado;

        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();

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

        // RN-10: solo hasta 24 h antes del inicio
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

        evento.Estado = EstadoEvento.Cancelado;
        await _context.SaveChangesAsync();

        _logger.LogWarning("Evento {Id} cancelado por usuario {UsuarioId}", id, idUsuario);
    }

    // --- APROBAR / RECHAZAR (admin) ---

    public async Task AprobarAsync(int id)
    {
        var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
        if (evento == null) throw new KeyNotFoundException("El evento no existe.");

        if (evento.Estado != EstadoEvento.EnRevision)
            throw new InvalidOperationException("Solo se pueden aprobar eventos en revisión.");

        evento.Estado = EstadoEvento.Publicado;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Evento {Id} aprobado", id);
    }

    public async Task RechazarAsync(int id)
    {
        var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
        if (evento == null) throw new KeyNotFoundException("El evento no existe.");

        if (evento.Estado != EstadoEvento.EnRevision)
            throw new InvalidOperationException("Solo se pueden rechazar eventos en revisión.");

        evento.Estado = EstadoEvento.Cancelado;
        await _context.SaveChangesAsync();
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
}
