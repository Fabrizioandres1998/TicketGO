using TicketsGO.Models;

namespace TicketsGO.Services;

public interface ITicketService
{
    Task<PaginatedResult<Ticket>> ListByUsuarioAsync(int idUsuario, int pagina = 1, int cantidadPorPagina = 10);
    Task<Ticket?> GetByIdAsync(int id);
    Task TransferirAsync(int idTicket, int idUsuarioOrigen, string emailDestino);
    Task MarcarUsadoAsync(int idTicket, int idUsuario);
}