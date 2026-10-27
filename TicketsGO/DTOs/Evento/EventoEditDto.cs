using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TicketsGO.DTOs
{
    public class EventoEditDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(150)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        [Display(Name = "Fecha de fin")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "La provincia es obligatoria.")]
        [StringLength(100)]
        [Display(Name = "Provincia")]
        public string Provincia { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [StringLength(100)]
        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; } = string.Empty;

        [Required(ErrorMessage = "El establecimiento es obligatorio.")]
        [StringLength(150)]
        [Display(Name = "Establecimiento")]
        public string Establecimiento { get; set; } = string.Empty;

        [Display(Name = "Privado")]
        public bool Privado { get; set; }

        [Required(ErrorMessage = "La cantidad de tickets es obligatoria.")]
        [Range(1, 100000, ErrorMessage = "Debe ser mayor a 0.")]
        [Display(Name = "Tickets totales")]
        public int TicketsTotales { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 99999999.99, ErrorMessage = "El precio debe ser mayor a 0.")]
        [Display(Name = "Precio del ticket")]
        public decimal PrecioTicket { get; set; }

        [Display(Name = "Imagen")]
        public IFormFile? Imagen { get; set; }

        public string? ImagenUrl { get; set; }
    }
}