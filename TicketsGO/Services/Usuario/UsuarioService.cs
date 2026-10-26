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

    // Trae todos los usuarios.
    public async Task<PaginatedResult<Usuario>> ListAsync(int pagina = 1, int cantidadPorPagina = 10)
    {
        var totalRegistros = await _context.Usuarios.CountAsync();

        var usuarios = await _context.Usuarios
            .OrderBy(u => u.Id)
            .Skip((pagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToListAsync();

        return new PaginatedResult<Usuario>
        {
            Items = usuarios,
            TotalCount = totalRegistros,
            PageNumber = pagina,
            PageSize = cantidadPorPagina
        };
    }

    // Busca un usuario por su ID.
    public async Task<Usuario?> GetUsuarioByIdAsync(int id)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
    }

    // Busca un usuario por su email.
    public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    // Verifica si un email ya está registrado.
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacio", nameof(email));

        return await _context.Usuarios.AnyAsync(u => u.Email == email);
    }

    // Registra un usuario nuevo y hashea su contraseña.
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

    // Modifica los datos personales del usuario.
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

    // Valida credenciales y controla si la cuenta está activa.
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

    // Cambia la contraseña validando la actual.
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

    // Genera un token y lo guarda para recuperar contraseña.
    public async Task SolicitarResetPasswordAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email es obligatorio.", nameof(email));

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        if (usuario == null)
        {
            _logger.LogWarning("Intento de reseteo para email no registrado: {Email}", email);
            return;
        }

        var token = Guid.NewGuid().ToString();

        usuario.VerificationToken = token;
        usuario.ResetPasswordExpires = DateTime.Now.AddHours(1);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Token de reseteo generado para {Email} Enlace: http://localhost:5062/Usuario/ResetPassword?token={Token}", email, token);
    }

    // Valida el token y cambia la contraseña.
    public async Task ResetPasswordAsync(string token, string passwordNueva)
    {
        if (string.IsNullOrWhiteSpace(passwordNueva))
            throw new ArgumentException("La nueva contraseña no puede estar vacía.", nameof(passwordNueva));

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.VerificationToken == token);

        if (usuario == null)
        {
            _logger.LogWarning("Intento de reseteo fallido: Token inválido {Token}", token);
            throw new InvalidOperationException("El token es inválido o ya fue utilizado.");
        }

        if (usuario.ResetPasswordExpires == null || usuario.ResetPasswordExpires < DateTime.Now)
        {
            _logger.LogWarning("Intento de reseteo fallido: Token expirado para {Email}", usuario.Email);
            throw new InvalidOperationException("El token ha expirado. Solicite uno nuevo.");
        }

        usuario.Password = _passwordHasher.HashPassword(usuario, passwordNueva);
        usuario.VerificationToken = null;
        usuario.ResetPasswordExpires = null;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Contraseña reseteada exitosamente para {Email}", usuario.Email);
    }

    // Da de baja lógica a un usuario.
    public async Task EliminarUsuarioAsync(int id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        if (usuario == null)
            throw new KeyNotFoundException("El usuario no existe");

        usuario.Estado = false;

        await _context.SaveChangesAsync();
    }

    // Suspende a un usuario validando no dejar el sistema sin admins.
    public async Task SuspenderUsuarioAsync(int id, int idAdminQueEjecuta)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        if (usuario == null) throw new KeyNotFoundException("El usuario no existe");

        if (id == idAdminQueEjecuta)
            throw new InvalidOperationException("No puedes suspender tu propia cuenta.");

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

    // Reactiva a un usuario suspendido.
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

    // Cambia el rol del usuario validando permisos de admin.
    public async Task CambiarRolAsync(int id, RolUsuario nuevoRol, int idAdminQueEjecuta)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        if (usuario == null)
            throw new KeyNotFoundException("El usuario no existe");

        if (!Enum.IsDefined(typeof(RolUsuario), nuevoRol))
            throw new ArgumentException("El rol no es válido");

        if (usuario.Rol == nuevoRol)
            throw new InvalidOperationException($"El usuario ya tiene el rol {nuevoRol}.");

        if (id == idAdminQueEjecuta && nuevoRol != RolUsuario.Administrador)
            throw new InvalidOperationException("No puedes quitarte a ti mismo el rol de Administrador.");

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

    // VerificarEmailAsync queda pendiente para cuando se integre el envío de mails.
    // public async Task VerificarEmailAsync(string token)
    // {
    //     await Task.CompletedTask;
    // }
}