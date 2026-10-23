namespace TicketsGO.DTOs
{
    public class UsuarioDetailsDTO
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string Rol { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public string EstadoTexto => Estado ? "Activo" : "Suspendido"; // Propiedad calculada
    }
}