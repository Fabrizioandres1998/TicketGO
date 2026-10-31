using System.ComponentModel.DataAnnotations;

namespace TicketsGO.DTOs
{
    public class CompraCreateDto
    {
        [Required(ErrorMessage = "El evento es obligatorio.")]
        [Display(Name = "Evento")]
        public int IdEvento { get; set; }

        [Required(ErrorMessage = "La cantidad de tickets es obligatoria.")]
        [Range(1, 5, ErrorMessage = "Podés comprar entre 1 y 5 tickets.")]
        [Display(Name = "Cantidad de tickets")]
        public int TotalTickets { get; set; } = 1;
    }
}