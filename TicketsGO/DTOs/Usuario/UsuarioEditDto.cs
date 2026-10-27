using System.ComponentModel.DataAnnotations;

namespace TicketsGO.DTOs
{
    public class UsuarioEditDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(45)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(45)]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(45)]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Display(Name = "Avatar")]
        public IFormFile? Avatar { get; set; }

        public string? AvatarUrl { get; set; }
    }
}