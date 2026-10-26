using System.ComponentModel.DataAnnotations;
using TicketsGO.Models;

namespace TicketsGO.DTOs
{
    public class UsuarioAdminEditDTO
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

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [Display(Name = "Rol")]
        public RolUsuario Rol { get; set; } = RolUsuario.Usuario;

        [Display(Name = "Activo")]
        public bool Estado { get; set; } = true;
    }
}