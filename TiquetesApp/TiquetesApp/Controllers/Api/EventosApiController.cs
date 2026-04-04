using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiquetesApp.Constants;
using TiquetesApp.Exceptions;
using TiquetesApp.Models;
using TiquetesApp.Services;

namespace TiquetesApp.Controllers.Api
{
    [Route("api/eventos")]
    [ApiController]
    [Authorize]
    public class EventosApiController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public EventosApiController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var eventos = _eventoService.ObtenerTodos().Select(e => new
            {
                e.Id,
                e.Nombre,
                Fecha = e.Fecha.ToString("yyyy-MM-dd HH:mm"),
                e.Lugar,
                e.Precio,
                e.CantidadDisponible
            });

            return Ok(eventos);
        }

        [HttpGet("{id:int}")]
        public IActionResult ObtenerPorId(int id)
        {
            try
            {
                var evento = _eventoService.ObtenerDetalle(id);
                return Ok(new
                {
                    evento.Id,
                    evento.Nombre,
                    Fecha = evento.Fecha.ToString("yyyy-MM-dd HH:mm"),
                    evento.Lugar,
                    evento.Precio,
                    evento.CantidadDisponible
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Crear([FromBody] Evento evento)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Datos inválidos.", errors });
            }

            _eventoService.Crear(evento);

            return CreatedAtAction(nameof(ObtenerPorId),
                new { id = evento.Id },
                new { evento.Id, evento.Nombre, evento.Precio, evento.CantidadDisponible });
        }
    }
}