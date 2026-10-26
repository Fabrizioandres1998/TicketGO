using System.ComponentModel.DataAnnotations;

namespace TicketsGO.DTOs
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = string.Empty;
    }
}