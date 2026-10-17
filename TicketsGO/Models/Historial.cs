using System.ComponentModel.DataAnnotations;

namespace TicketsGO.Models
{
    public class Historial
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
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
    }
}