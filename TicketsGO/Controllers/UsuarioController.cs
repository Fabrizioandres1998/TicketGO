using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketsGO.DTOs;
using TicketsGO.Models;
using TicketsGO.Services;

namespace TicketsGO.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(ILogger<UsuarioController> logger, IUsuarioService usuarioService)
    {
        _logger = logger;
        _usuarioService = usuarioService;
    }

    // --- LISTADOS Y DETALLES ---

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Index(int pagina = 1)
    {
        var resultadoPaginado = await _usuarioService.ListAsync(pagina);

        var usuariosDto = resultadoPaginado.Items.Select(u => new UsuarioListDTO
        {
            Id = u.Id,
            NombreCompleto = $"{u.Nombre} {u.Apellido}",
            Email = u.Email!,
            Rol = u.Rol.ToString(),
            EstadoTexto = u.Estado ? "Activo" : "Suspendido"
        }).ToList();

        var resultadoDto = new PaginatedResult<UsuarioListDTO>
        {
            Items = usuariosDto,
            TotalCount = resultadoPaginado.TotalCount,
            PageNumber = resultadoPaginado.PageNumber,
            PageSize = resultadoPaginado.PageSize
        };

        return View(resultadoDto);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // Si no es admin y no es su propio id, lo mandamos a su perfil
        if (!User.IsInRole("Administrador") && id != userId)
            return RedirectToAction(nameof(MyDetails));

        var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
        if (usuario == null) return NotFound();

        var usuarioDto = new UsuarioDetailsDTO
        {
            Id = usuario.Id,
            NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
            Email = usuario.Email!,
            Telefono = usuario.Telefono,
            Rol = usuario.Rol.ToString(),
            Estado = usuario.Estado
        };

        return View(usuarioDto);
    }

    // --- CREAR (Solo Administradores) ---

    [Authorize(Roles = "Administrador")]
    [HttpGet]
    public IActionResult Create()
    {
        return View(new UsuarioCreateDTO());
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UsuarioCreateDTO dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Password = dto.Password,
                Rol = dto.Rol,
                Estado = dto.Estado
            };

            await _usuarioService.RegisterAsync(usuario);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
    }

    // --- REGISTRO PÚBLICO ---

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(MyDetails));

        return View(new UsuarioRegisterDto());
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(UsuarioRegisterDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Password = dto.Password,
                Rol = RolUsuario.Usuario,
                Estado = true
            };

            await _usuarioService.RegisterAsync(usuario);
            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
    }

    // --- PERFIL PROPIO ---

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var usuario = await _usuarioService.GetUsuarioByIdAsync(userId);
        if (usuario == null) return NotFound();

        var dto = new UsuarioEditDTO
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre!,
            Apellido = usuario.Apellido!,
            Telefono = usuario.Telefono
        };

        return View(dto);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(UsuarioEditDTO dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            var usuario = new Usuario
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Telefono = dto.Telefono
            };

            await _usuarioService.ModificarUsuarioAsync(usuario);
            return RedirectToAction(nameof(MyDetails));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> MyDetails()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var usuario = await _usuarioService.GetUsuarioByIdAsync(userId);
        if (usuario == null) return NotFound();

        var usuarioDto = new UsuarioDetailsDTO
        {
            Id = usuario.Id,
            NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
            Email = usuario.Email!,
            Telefono = usuario.Telefono,
            Rol = usuario.Rol.ToString(),
            Estado = usuario.Estado
        };

        return View(usuarioDto);
    }

    // --- CAMBIO DE CONTRASEÑA (Usuario logueado) ---

    [Authorize]
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View(new UsuarioChangePasswordDto());
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(UsuarioChangePasswordDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _usuarioService.CambiarPasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
            TempData["Success"] = "Contraseña actualizada correctamente.";
            return RedirectToAction(nameof(MyDetails));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
    }

    // --- EDITAR USUARIO (Solo Administradores) ---

    [Authorize(Roles = "Administrador")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
        if (usuario == null) return NotFound();

        var dto = new UsuarioAdminEditDTO
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre!,
            Apellido = usuario.Apellido!,
            Telefono = usuario.Telefono,
            Rol = usuario.Rol,
            Estado = usuario.Estado
        };

        return View(dto);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UsuarioAdminEditDTO dto)
    {
        if (id != dto.Id) return BadRequest();
        if (!ModelState.IsValid) return View(dto);

        try
        {
            var usuario = new Usuario
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Telefono = dto.Telefono
            };

            await _usuarioService.ModificarUsuarioAsync(usuario);
            await _usuarioService.CambiarRolAsync(dto.Id, dto.Rol,
                int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));

            if (!dto.Estado)
                await _usuarioService.SuspenderUsuarioAsync(dto.Id,
                    int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
    }

    // --- ELIMINAR / SUSPENDER (Solo Administradores) ---

    [Authorize(Roles = "Administrador")]
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
        if (usuario == null) return NotFound();

        var usuarioDto = new UsuarioDeleteDTO
        {
            Id = usuario.Id,
            NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
            Email = usuario.Email!
        };

        return View(usuarioDto);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _usuarioService.SuspenderUsuarioAsync(id, adminId);
            TempData["Success"] = "Usuario suspendido correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // --- ACCIONES DE ADMINISTRADOR ---

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reactivar(int id)
    {
        try
        {
            await _usuarioService.ReactivarUsuarioAsync(id);
            TempData["Success"] = "Usuario reactivado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarRol(int id, RolUsuario nuevoRol)
    {
        try
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _usuarioService.CambiarRolAsync(id, nuevoRol, adminId);
            TempData["Success"] = "Rol actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // --- LOGIN / LOGOUT ---

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(MyDetails));

        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "Email y contraseña son obligatorios.");
            return View();
        }

        try
        {
            var usuario = await _usuarioService.LoginAsync(email, password);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario!.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Email!),
                    new Claim(ClaimTypes.Role, usuario.Rol.ToString()),
                    new Claim("NombreCompleto", $"{usuario.Nombre} {usuario.Apellido}")
                };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            if (usuario.Rol == RolUsuario.Administrador)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(MyDetails));
        }
        catch (UnauthorizedAccessException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    // --- RECUPERACIÓN DE CONTRASEÑA ---

    [AllowAnonymous]
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(MyDetails));

        return View(new ForgotPasswordDto());
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _usuarioService.SolicitarResetPasswordAsync(dto.Email);
            TempData["Success"] = "Si el email está registrado, se generó un token. Revisá la consola del servidor.";
            return RedirectToAction(nameof(ForgotPassword));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult ResetPassword(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return RedirectToAction(nameof(Login));

        return View(new ResetPasswordDto { Token = token });
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _usuarioService.ResetPasswordAsync(dto.Token, dto.NewPassword);
            TempData["Success"] = "Contraseña actualizada. Ya podés iniciar sesión.";
            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
    }
}