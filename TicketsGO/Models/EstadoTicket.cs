using System.ComponentModel.DataAnnotations;

namespace TicketsGO.Models
{
    public enum EstadoTicket
    {
        [Display(Name = "Usado")]
        Usado,

        [Display(Name = "No usado")]
        NoUsado
    }
}
