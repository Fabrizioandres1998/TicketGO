using System.ComponentModel.DataAnnotations;

namespace TicketsGO.Models
{
    public class Ticket
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Evento")]
        public int IdEvento { get; set; }
        public Evento? Evento { get; set; }

        [Required]
        [Display(Name = "Compra")]
        public int IdCompra { get; set; }
        public Compra? Compra { get; set; }

        [Display(Name = "Usuario")]
        public int? IdUsuario { get; set; }

        public Usuario? Usuario { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public EstadoTicket Estado { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Código")]
        public string? Codigo { get; set; }
    }
}