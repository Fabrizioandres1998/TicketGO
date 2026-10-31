using TicketsGO.DTOs;
using TicketsGO.Models;

namespace TicketsGO.Services;

public interface IEventoService
{
    Task<PaginatedResult<Evento>> ListAsync(int pagina = 1, int cantidadPorPagina = 10, string? busqueda = null);
    Task<PaginatedResult<Evento>> ListPublicadosAsync(int pagina = 1, int cantidadPorPagina = 10);
    Task<PaginatedResult<Evento>> ListByUsuarioAsync(int idUsuario, int pagina = 1, int cantidadPorPagina = 10);

    Task<Evento?> GetByIdAsync(int id);

    Task<Evento> CreateAsync(Evento evento, int idUsuario);
    Task UpdateAsync(Evento evento, int idUsuario, bool esAdmin);
    Task DeleteAsync(int id, int idUsuario, bool esAdmin);

    Task AprobarAsync(int id, int idUsuario);
    Task RechazarAsync(int id, int idUsuario);
    Task CancelarAsync(int id, int idUsuario, bool esAdmin);

    Task ActualizarImagenAsync(int id, string imagenUrl);
    Task<PaginatedResult<Evento>> BuscarPublicadosAsync(string? nombre, int pagina = 1, int cantidadPorPagina = 10);
}
