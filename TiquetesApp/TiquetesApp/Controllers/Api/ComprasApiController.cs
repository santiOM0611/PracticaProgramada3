using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TiquetesApp.Exceptions;
using TiquetesApp.Models;
using TiquetesApp.Models.ViewModels;
using TiquetesApp.Services;

namespace TiquetesApp.Controllers.Api
{
    [Route("api/compras")]
    [ApiController]
    [Authorize]
    public class ComprasApiController : ControllerBase
    {
        private readonly ICompraService _compraService;
        private readonly UserManager<Usuario> _userManager;

        public ComprasApiController(ICompraService compraService, UserManager<Usuario> userManager)
        {
            _compraService = compraService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Comprar([FromBody] CompraViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Datos inválidos.", errors });
            }

            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
                return Unauthorized(new { message = "Usuario no autenticado." });

            try
            {
                var compra = _compraService.Comprar(model, usuario.Id, usuario.NombreCompleto);
                return Ok(new
                {
                    message = "Compra realizada exitosamente.",
                    compra.Id,
                    compra.NombreCliente,
                    compra.Cantidad,
                    compra.Total,
                    FechaCompra = compra.FechaCompra.ToString("yyyy-MM-dd HH:mm")
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("historial")]
        public async Task<IActionResult> Historial()
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
                return Unauthorized(new { message = "Usuario no autenticado." });

            var compras = _compraService.ObtenerHistorialUsuario(usuario.Id)
                .Select(c => new
                {
                    c.Id,
                    EventoNombre = c.Evento!.Nombre,
                    EventoFecha = c.Evento.Fecha.ToString("yyyy-MM-dd HH:mm"),
                    EventoLugar = c.Evento.Lugar,
                    c.Cantidad,
                    c.Total,
                    FechaCompra = c.FechaCompra.ToString("yyyy-MM-dd HH:mm")
                });

            return Ok(compras);
        }
    }
}