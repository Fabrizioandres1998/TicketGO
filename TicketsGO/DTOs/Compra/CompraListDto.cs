namespace TicketsGO.DTOs
{
    public class CompraListDto
    {
        public int Id { get; set; }
        public string Evento { get; set; } = string.Empty;
        public DateTime FechaEvento { get; set; }
        public int TotalTickets { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}