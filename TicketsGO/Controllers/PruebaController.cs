
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketsGO.Data;

namespace TicketsGO.Controllers;

//[Route("Prueba")]
public class PruebaController : Controller
{
    private readonly AppDbContext _context;

    public PruebaController(AppDbContext context)
    {
        _context = context;
    }
    //[HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var usuarios = await _context.Usuarios.CountAsync();

        return Content($"Conexión correcta. Usuarios registrados: {usuarios}");
    }
}
