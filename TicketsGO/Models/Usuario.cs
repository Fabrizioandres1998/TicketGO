
using System.ComponentModel.DataAnnotations;

namespace TicketsGO.Models
{
    public class Usuario
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Apellido")]
        public string? Apellido { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [StringLength(150)]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [StringLength(30)]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(255, MinimumLength = 6)]
        [Display(Name = "Contraseña")]
        public string? Password { get; set; }

        [Display(Name = "Rol")]
        public RolUsuario Rol { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; }

        [Display(Name = "Email verificado")]
        public bool EmailVerificado { get; set; }

        [StringLength(255)]
        public string? VerificationToken { get; set; }

        public DateTime? ResetPasswordExpires { get; set; }
    }
}
