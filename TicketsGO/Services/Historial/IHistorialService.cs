using TicketsGO.Models;

namespace TicketsGO.Services;

public interface IHistorialService
{
    Task RegistrarAsync(int idEvento, int idUsuario, string descripcion);
    Task<PaginatedResult<Historial>> ListByUsuarioAsync(int idUsuario, int pagina = 1, int cantidadPorPagina = 10);
    Task<PaginatedResult<Historial>> ListByEventoAsync(int idEvento, int pagina = 1, int cantidadPorPagina = 10);
}