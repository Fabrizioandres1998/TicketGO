namespace TicketsGO.DTOs
{
    public class EventoDetailsDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Provincia { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Establecimiento { get; set; } = string.Empty;
        public bool Privado { get; set; }
        public int TicketsTotales { get; set; }
        public int TicketsVendidos { get; set; }
        public int TicketsDisponibles => TicketsTotales - TicketsVendidos;
        public decimal PrecioTicket { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }
        public string Productor { get; set; } = string.Empty;
        public int IdUsuario { get; set; }
    }
}