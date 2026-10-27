using System.ComponentModel.DataAnnotations;

namespace TicketsGO.Models
{
    public class Evento
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public int IdUsuario { get; set; }

        public Usuario? Usuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Display(Name = "Fecha de fin")]
        public DateTime FechaFin { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Provincia")]
        public string? Provincia { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Ciudad")]
        public string? Ciudad { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Establecimiento")]
        public string? Establecimiento { get; set; }

        [Required]
        [Display(Name = "Privado")]
        public bool Privado { get; set; }

        [Required]
        [Display(Name = "Tickets totales")]
        public int TicketsTotales { get; set; }

        [Required]
        [Display(Name = "Tickets vendidos")]
        public int TicketsVendidos { get; set; }

        [Required]
        [Display(Name = "Precio del ticket")]
        public decimal PrecioTicket { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public EstadoEvento Estado { get; set; }

        [Display(Name = "Imagen")]
        [StringLength(255)]
        public string? ImagenUrl { get; set; }
        
    }
}