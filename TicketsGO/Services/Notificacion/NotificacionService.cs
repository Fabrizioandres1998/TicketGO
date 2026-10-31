using Microsoft.EntityFrameworkCore;
using TicketsGO.Data;
using TicketsGO.Models;

namespace TicketsGO.Services;

public class NotificacionService : INotificacionService
{
    private readonly AppDbContext _context;

    public NotificacionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CrearAsync(int idUsuario, string titulo, string mensaje, string? url = null)
    {
        _context.Notificaciones.Add(new Notificacion
        {
            IdUsuario = idUsuario,
            Titulo = titulo,
            Mensaje = mensaje,
            Url = url,
            Fecha = DateTime.Now
        });
        await _context.SaveChangesAsync();
    }

    public async Task<PaginatedResult<Notificacion>> ListByUsuarioAsync(int idUsuario, int pagina = 1, int cantidadPorPagina = 10)
    {
        pagina = Math.Max(1, pagina);
        var query = _context.Notificaciones.Where(n => n.IdUsuario == idUsuario);
        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(n => n.Fecha)
            .ThenByDescending(n => n.Id)
            .Skip((pagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToListAsync();

        return new PaginatedResult<Notificacion>
        {
            Items = items,
            TotalCount = total,
            PageNumber = pagina,
            PageSize = cantidadPorPagina
        };
    }

    public Task<int> ContarNoLeidasAsync(int idUsuario) =>
        _context.Notificaciones.CountAsync(n => n.IdUsuario == idUsuario && !n.Leida);

    public async Task<string?> MarcarLeidaAsync(int idNotificacion, int idUsuario)
    {
        var notificacion = await _context.Notificaciones
            .FirstOrDefaultAsync(n => n.Id == idNotificacion && n.IdUsuario == idUsuario);
        if (notificacion == null)
            throw new KeyNotFoundException("La notificación no existe.");

        if (!notificacion.Leida)
        {
            notificacion.Leida = true;
            await _context.SaveChangesAsync();
        }

        return notificacion.Url;
    }

    public async Task MarcarTodasLeidasAsync(int idUsuario)
    {
        await _context.Notificaciones
            .Where(n => n.IdUsuario == idUsuario && !n.Leida)
            .ExecuteUpdateAsync(setters => setters.SetProperty(n => n.Leida, true));
    }
}
