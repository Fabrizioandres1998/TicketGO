namespace TicketsGO.DTOs
{
    public class EventoListDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int TicketsTotales { get; set; }
        public int TicketsVendidos { get; set; }
        public int TicketsDisponibles => TicketsTotales - TicketsVendidos;
        public decimal PrecioTicket { get; set; }
        public string? ImagenUrl { get; set; }
        public string Productor { get; set; } = string.Empty;
    }
}