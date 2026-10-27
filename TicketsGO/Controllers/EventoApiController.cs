using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsGO.DTOs;
using TicketsGO.Models;
using TicketsGO.Services;

namespace TicketsGO.Controllers;

[ApiController]
[Route("api/eventos")]
[Authorize(Roles = "Productor,Administrador")]
public class EventosApiController : ControllerBase
{
    private readonly IEventoService _eventoService;
    private readonly IFileService _fileService;

    public EventosApiController(IEventoService eventoService, IFileService fileService)
    {
        _eventoService = eventoService;
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(int pagina = 1)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        var esAdmin = User.IsInRole("Administrador");

        var resultado = esAdmin
            ? await _eventoService.ListAsync(pagina)
            : await _eventoService.ListByUsuarioAsync(userId, pagina);

        var items = resultado.Items.Select(e => new
        {
            e.Id,
            e.Nombre,
            e.FechaInicio,
            e.Ciudad,
            e.Provincia,
            Estado = e.Estado.ToString(),
            e.TicketsTotales,
            e.TicketsVendidos,
            e.PrecioTicket,
            e.ImagenUrl
        });

        return Ok(new
        {
            items,
            totalCount = resultado.TotalCount,
            pageNumber = resultado.PageNumber,
            pageSize = resultado.PageSize
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Obtener(int id)
    {
        var evento = await _eventoService.GetByIdAsync(id);
        if (evento == null) return NotFound();

        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        if (!User.IsInRole("Administrador") && evento.IdUsuario != userId)
            return Forbid();

        return Ok(new
        {
            evento.Id,
            evento.Nombre,
            evento.Descripcion,
            evento.FechaInicio,
            evento.FechaFin,
            evento.Provincia,
            evento.Ciudad,
            evento.Establecimiento,
            evento.Privado,
            evento.TicketsTotales,
            evento.PrecioTicket,
            evento.ImagenUrl
        });
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromForm] EventoCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

            var evento = new Evento
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Provincia = dto.Provincia,
                Ciudad = dto.Ciudad,
                Establecimiento = dto.Establecimiento,
                Privado = dto.Privado,
                TicketsTotales = dto.TicketsTotales,
                PrecioTicket = dto.PrecioTicket
            };

            if (dto.Imagen != null && dto.Imagen.Length > 0)
            {
                evento.ImagenUrl = await _fileService.SaveEventoImagenAsync(dto.Imagen);
            }

            await _eventoService.CreateAsync(evento, userId);
            return Ok(new { id = evento.Id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Editar(int id, [FromForm] EventoEditDto dto)
    {
        if (id != dto.Id) return BadRequest();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var esAdmin = User.IsInRole("Administrador");

            var eventoDb = await _eventoService.GetByIdAsync(id);
            if (eventoDb == null) return NotFound();

            if (dto.Imagen != null && dto.Imagen.Length > 0)
            {
                var nueva = await _fileService.SaveEventoImagenAsync(dto.Imagen);
                _fileService.DeleteFile(eventoDb.ImagenUrl);
                await _eventoService.ActualizarImagenAsync(id, nueva);
            }

            var evento = new Evento
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Provincia = dto.Provincia,
                Ciudad = dto.Ciudad,
                Establecimiento = dto.Establecimiento,
                Privado = dto.Privado,
                TicketsTotales = dto.TicketsTotales,
                PrecioTicket = dto.PrecioTicket
            };

            await _eventoService.UpdateAsync(evento, userId, esAdmin);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var esAdmin = User.IsInRole("Administrador");
            await _eventoService.DeleteAsync(id, userId, esAdmin);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}