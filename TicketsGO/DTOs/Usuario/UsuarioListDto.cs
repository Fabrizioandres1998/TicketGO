namespace TicketsGO.DTOs
{
    public class UsuarioListDTO
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string EstadoTexto { get; set; } = string.Empty;
    }
}