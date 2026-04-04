using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiquetesApp.Constants;

namespace TiquetesApp.Controllers
{
    [Route("eventos")]
    [Authorize]
    public class EventosController : Controller
    {
        [HttpGet("")]
        public IActionResult Index() => View();

        [HttpGet("crear")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Crear() => View();
    }
}