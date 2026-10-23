using TicketsGO.Models;
using System.Linq;
using TicketsGO.DTOs;
using TicketsGO.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TicketsGO.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    private readonly UsuarioService _UsuarioService;

    public UsuarioController(ILogger<UsuarioController> logger, UsuarioService usuarioService)
    {
        _logger = logger;
        _UsuarioService = usuarioService;
    }

    // --- LISTADOS Y DETALLES (Requieren estar logueado) ---

    [Authorize]
    public async Task<IActionResult> Index(int pagina = 1)
    {
        var resultadoPaginado = await _UsuarioService.ListAsync(pagina);

        var usuariosDTO = resultadoPaginado.Items.Select(u => new UsuarioListDTO
        {
            Id = u.Id,
            NombreCompleto = $"{u.Nombre} {u.Apellido}",
            Email = u.Email!,
            Rol = u.Rol.ToString(),
            EstadoTexto = u.Estado ? "Activo" : "Suspendido"
        }).ToList();

        var resultadoDTO = new PaginatedResult<UsuarioListDTO>
        {
            Items = usuariosDTO,
            TotalCount = resultadoPaginado.TotalCount,
            PageNumber = resultadoPaginado.PageNumber,
            PageSize = resultadoPaginado.PageSize
        };

        return View(resultadoDTO);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var usuario = await _UsuarioService.GetUsuarioByIdAsync(id);
        if (usuario == null) return NotFound();

        var usuarioDTO = new UsuarioDetailsDTO
        {
            Id = usuario.Id,
            NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
            Email = usuario.Email!,
            Telefono = usuario.Telefono,
            Rol = usuario.Rol.ToString(),
            Estado = usuario.Estado
        };

        return View(usuarioDTO);
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
                Rol = RolUsuario.Usuario, // Por defecto se crean como Usuario
                Estado = true
            };

            await _UsuarioService.RegisterAsync(usuario);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
    }

    // --- EDITAR (Solo Administradores) ---

    [Authorize(Roles = "Administrador")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var usuario = await _UsuarioService.GetUsuarioByIdAsync(id);
        if (usuario == null) return NotFound();

        var usuarioDTO = new UsuarioEditDTO
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre!,
            Apellido = usuario.Apellido!,
            Telefono = usuario.Telefono
        };

        return View(usuarioDTO);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UsuarioEditDTO dto)
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

            await _UsuarioService.ModificarUsuarioAsync(usuario);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
    }

    // --- ELIMINAR (Solo Administradores) ---

    [Authorize(Roles = "Administrador")]
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await _UsuarioService.GetUsuarioByIdAsync(id);
        if (usuario == null) return NotFound();

        var usuarioDTO = new UsuarioDeleteDTO
        {
            Id = usuario.Id,
            NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
            Email = usuario.Email!
        };

        return View(usuarioDTO);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            // Usamos el ID del admin logueado para la validación del servicio
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _UsuarioService.SuspenderUsuarioAsync(id, adminId);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // --- ACCIONES DE ADMINISTRADOR (Suspender, Reactivar, Cambiar Rol) ---

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reactivar(int id)
    {
        try
        {
            await _UsuarioService.ReactivarUsuarioAsync(id);
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
            await _UsuarioService.CambiarRolAsync(id, nuevoRol, adminId);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // --- LOGIN / LOGOUT (Público) ---

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
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
            var usuario = await _UsuarioService.LoginAsync(email, password);

            // Creamos los Claims (datos del usuario para la sesión)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario!.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Email!),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }
        catch (UnauthorizedAccessException ex)
        {
            ModelState.AddModelError("", ex.Message); // Cuenta suspendida
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message); // Credenciales inválidas
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
}