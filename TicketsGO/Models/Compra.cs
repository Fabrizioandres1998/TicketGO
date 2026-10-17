using System.ComponentModel.DataAnnotations;

namespace TicketsGO.Models
{
    public class Compra
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Evento")]
        public int IdEvento { get; set; }
        public Evento? Evento { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public int IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }

        [Required]
        [Display(Name = "Total de tickets")]
        public int TotalTickets { get; set; }

        [Required]
        [Display(Name = "Monto total")]
        public decimal MontoTotal { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public EstadoCompra Estado { get; set; }
    }
}