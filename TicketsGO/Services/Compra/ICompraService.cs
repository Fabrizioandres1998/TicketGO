using TicketsGO.Models;

namespace TicketsGO.Services;

public interface ICompraService
{
    Task<PaginatedResult<Compra>> ListByUsuarioAsync(int idUsuario, int pagina = 1, int cantidadPorPagina = 10);
    Task<Compra?> GetByIdAsync(int id);
    Task<Compra> CrearAsync(int idEvento, int idUsuario, int cantidad);
    Task CancelarAsync(int id, int idUsuario);
}