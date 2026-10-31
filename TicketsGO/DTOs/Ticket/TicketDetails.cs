namespace TicketsGO.DTOs
{
    public class TicketDetailsDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Evento { get; set; } = string.Empty;
        public DateTime FechaEvento { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string EstadoDisplayName { get; set; } = string.Empty;
        public string Propietario { get; set; } = string.Empty;
        public bool PuedeTransferir { get; set; }
        public bool PuedeMarcarUsado { get; set; }
    }
}
