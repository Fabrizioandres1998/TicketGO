using System.ComponentModel.DataAnnotations;

namespace TicketsGO.Models;

public class Notificacion
{
    [Key]
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Usuario")]
    public int IdUsuario { get; set; }
    public Usuario? Usuario { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    [Display(Name = "Mensaje")]
    public string Mensaje { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Enlace")]
    public string? Url { get; set; }

    [Required]
    [Display(Name = "Fecha")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required]
    [Display(Name = "Leída")]
    public bool Leida { get; set; }
}
