using TicketsGO.Models;

namespace TicketsGO.Services;

public interface IUsuarioService
{
    // Registro y autenticación
    Task RegisterAsync(Usuario usuario);
    Task<Usuario?> LoginAsync(string email, string password);

    // Consultas
    Task<IEnumerable<Usuario>> ListAsync();
    Task<Usuario?> GetUsuarioByIdAsync(int id);
    Task<Usuario?> GetUsuarioByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);

    // Gestión del perfil
    Task ModificarUsuarioAsync(Usuario usuario);
    Task CambiarPasswordAsync(
        int idUsuario,
        string passwordActual,
        string passwordNueva);

    // Recuperación y verificación
    Task SolicitarResetPasswordAsync(string email);
    Task ResetPasswordAsync(string token, string passwordNueva);
    // Task VerificarEmailAsync(string token);

    // Administración
    Task EliminarUsuarioAsync(int id);
    Task SuspenderUsuarioAsync(int id, int idAdminQueEjecuta);
    Task ReactivarUsuarioAsync(int id);
    Task CambiarRolAsync(int id, RolUsuario nuevoRol, int idAdminQueEjecuta);
}