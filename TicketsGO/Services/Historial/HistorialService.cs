using Microsoft.EntityFrameworkCore;
using TicketsGO.Data;
using TicketsGO.Models;

namespace TicketsGO.Services;

public class HistorialService : IHistorialService
{
    private readonly AppDbContext _context;

    public HistorialService(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarAsync(int idEvento, int idUsuario, string descripcion)
    {
        _context.Historiales.Add(new Historial
        {
            IdEvento = idEvento,
            IdUsuario = idUsuario,
            Descripcion = descripcion,
            Fecha = DateTime.Now
        });
        await _context.SaveChangesAsync();
    }

    public async Task<PaginatedResult<Historial>> ListByUsuarioAsync(int idUsuario, int pagina = 1, int cantidadPorPagina = 10)
    {
        var query = _context.Historiales
            .Include(h => h.Evento)
            .Where(h => h.IdUsuario == idUsuario);

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(h => h.Fecha)
            .ThenByDescending(h => h.Id)
            .Skip((pagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToListAsync();

        return new PaginatedResult<Historial>
        {
            Items = items,
            TotalCount = total,
            PageNumber = pagina,
            PageSize = cantidadPorPagina
        };
    }

    public async Task<PaginatedResult<Historial>> ListByEventoAsync(int idEvento, int pagina = 1, int cantidadPorPagina = 10)
    {
        var query = _context.Historiales
            .Include(h => h.Usuario)
            .Where(h => h.IdEvento == idEvento);

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(h => h.Fecha)
            .ThenByDescending(h => h.Id)
            .Skip((pagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToListAsync();

        return new PaginatedResult<Historial>
        {
            Items = items,
            TotalCount = total,
            PageNumber = pagina,
            PageSize = cantidadPorPagina
        };
    }
}
