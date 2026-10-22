using System.ComponentModel.DataAnnotations;

namespace TicketsGO.Models
{
    public class Transferencia
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ticket")]
        public int IdTicket { get; set; }
        public Ticket? Ticket { get; set; }

        [Required]
        [Display(Name = "Usuario origen")]
        public int IdUsuarioOrigen { get; set; }
        public Usuario? UsuarioOrigen { get; set; }

        [Required]
        [Display(Name = "Usuario destino")]
        public int IdUsuarioDestino { get; set; }
        public Usuario? UsuarioDestino { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }
    }
}