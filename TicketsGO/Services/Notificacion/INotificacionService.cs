using TicketsGO.Models;

namespace TicketsGO.Services;

public interface INotificacionService
{
    Task CrearAsync(int idUsuario, string titulo, string mensaje, string? url = null);
    Task<PaginatedResult<Notificacion>> ListByUsuarioAsync(int idUsuario, int pagina = 1, int cantidadPorPagina = 10);
    Task<int> ContarNoLeidasAsync(int idUsuario);
    Task<string?> MarcarLeidaAsync(int idNotificacion, int idUsuario);
    Task MarcarTodasLeidasAsync(int idUsuario);
}
