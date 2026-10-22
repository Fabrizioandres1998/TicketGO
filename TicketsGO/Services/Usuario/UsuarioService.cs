using TicketsGO.Models;
using TicketsGO.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TicketsGO.Services;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _context;
    private readonly ILogger<UsuarioService> _logger;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UsuarioService(AppDbContext context, ILogger<UsuarioService> logger, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<Usuario>> ListAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario?> GetUsuarioByIdAsync(int id)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacio", nameof(email));

        return await _context.Usuarios.AnyAsync(u => u.Email == email);
    }

    public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task RegisterAsync(Usuario usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario));

        if (await ExistsByEmailAsync(usuario.Email!))
        {
            _logger.LogWarning("Intento de registro fallido: El email {Email} ya está en uso", usuario.Email);
            throw new InvalidOperationException("El mail ya está registrado");
        }

        usuario.Password = _passwordHasher.HashPassword(usuario, usuario.Password!);

        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Usuario registrado exitosamente con el email {Email}", usuario.Email);
    }

    public async Task<Usuario?> LoginAsync(string email, string password)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        if (usuario == null)
        {
            _logger.LogWarning("Intento de login fallido: El email {Email} no existe", email);
            throw new InvalidOperationException("Credenciales inválidas");
        }

        var passwordHasheada = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password!, password);

        if (passwordHasheada != PasswordVerificationResult.Success)
        {
            _logger.LogWarning("Intento de login fallido: Contraseña incorrecta para el email {Email}", email);
            throw new InvalidOperationException("Credenciales inválidas");
        }

        if (!usuario.Estado)
        {
            _logger.LogWarning("Intento de login fallido: La cuenta {Email} está suspendida", email);
            throw new UnauthorizedAccessException("Su cuenta está suspendida. Contacte al administrador.");
        }

        _logger.LogInformation("Login exitoso para el usuario {Email}", email);
        return usuario;
    }

    public async Task ModificarUsuarioAsync(Usuario usuarioModificado)
    {
        var usuarioDb = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioModificado.Id);

        if (usuarioDb == null)
            throw new KeyNotFoundException("El usuario no existe");

        usuarioDb.Nombre = usuarioModificado.Nombre;
        usuarioDb.Apellido = usuarioModificado.Apellido;
        usuarioDb.Telefono = usuarioModificado.Telefono;

        await _context.SaveChangesAsync();
    }

    public async Task CambiarPasswordAsync(int idUsuario, string passwordActual, string passwordNueva)
    {
        if (string.IsNullOrWhiteSpace(passwordNueva))
            throw new ArgumentException("La nueva contraseña no puede estar vacía.", nameof(passwordNueva));

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == idUsuario);

        if (usuario == null)
            throw new KeyNotFoundException("El usuario no existe");

        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password!, passwordActual);

        if (resultado == PasswordVerificationResult.Failed)
            throw new InvalidOperationException("La contraseña actual es incorrecta");

        usuario.Password = _passwordHasher.HashPassword(usuario, passwordNueva);

        await _context.SaveChangesAsync();
        _logger.LogInformation("Contraseña actualizada para el usuario {UsuarioId}", idUsuario);
    }

    public async Task SuspenderUsuarioAsync(int id, int idAdminQueEjecuta)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        if (usuario == null) throw new KeyNotFoundException("El usuario no existe");

        // Admin no puede suspenderse a sí mismo.
        if (id == idAdminQueEjecuta)
            throw new InvalidOperationException("No puedes suspender tu propia cuenta.");

        // Evita dejar el sistema sin admins activos.
        if (usuario.Rol == RolUsuario.Administrador)
        {
            var adminsActivos = await _context.Usuarios
                .CountAsync(u => u.Rol == RolUsuario.Administrador && u.Estado);

            if (adminsActivos <= 1)
                throw new InvalidOperationException("No se puede suspender al único administrador activo del sistema.");
        }

        usuario.Estado = false;
        await _context.SaveChangesAsync();
        _logger.LogWarning("Usuario {UsuarioId} suspendido por el admin {AdminId}", id, idAdminQueEjecuta);
    }

    public async Task ReactivarUsuarioAsync(int id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        if (usuario == null)
            throw new KeyNotFoundException("El usuario no existe");

        if (usuario.Estado)
            throw new InvalidOperationException("El usuario ya se encuentra activo.");

        usuario.Estado = true;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Usuario {UsuarioId} reactivado", id);
    }

    public async Task CambiarRolAsync(int id, RolUsuario nuevoRol, int idAdminQueEjecuta)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        if (usuario == null)
            throw new KeyNotFoundException("El usuario no existe");

        if (!Enum.IsDefined(typeof(RolUsuario), nuevoRol))
            throw new ArgumentException("El rol no es válido");

        // No permite asignar el mismo rol que ya tiene.
        if (usuario.Rol == nuevoRol)
            throw new InvalidOperationException($"El usuario ya tiene el rol {nuevoRol}.");

        // Admin no puede quitarse su propio rol.
        if (id == idAdminQueEjecuta && nuevoRol != RolUsuario.Administrador)
            throw new InvalidOperationException("No puedes quitarte a ti mismo el rol de Administrador.");

        // Evita que el último admin activo cambie de rol.
        if (usuario.Rol == RolUsuario.Administrador && usuario.Estado)
        {
            var adminsActivos = await _context.Usuarios
                .CountAsync(u => u.Rol == RolUsuario.Administrador && u.Estado);

            if (adminsActivos <= 1)
                throw new InvalidOperationException("No se puede cambiar el rol del único administrador activo.");
        }

        usuario.Rol = nuevoRol;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Rol del usuario {UsuarioId} cambiado a {NuevoRol} por el admin {AdminId}", id, nuevoRol, idAdminQueEjecuta);
    }
    public async Task SolicitarResetPasswordAsync(string email)
    {
        await Task.CompletedTask;
    }

    public async Task ResetPasswordAsync(string token, string passwordNueva)
    {
        await Task.CompletedTask;
    }

    public async Task VerificarEmailAsync(string token)
    {
        await Task.CompletedTask;
    }

    public async Task EliminarUsuarioAsync(int id)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id);

        if (usuario == null)
            throw new KeyNotFoundException("El usuario no existe");

        usuario.Estado = false;

        await _context.SaveChangesAsync();
    }
}