using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TiquetesApp.Controllers
{
    [Route("compras")]
    [Authorize]
    public class ComprasController : Controller
    {
        [HttpGet("comprar/{eventoId:int}")]
        public IActionResult Comprar(int eventoId)
        {
            ViewBag.EventoId = eventoId;
            return View();
        }

        [HttpGet("historial")]
        public IActionResult Historial() => View();
    }
}