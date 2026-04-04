using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TiquetesApp.Models;

namespace TiquetesApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("Login", "Auth");

            return RedirectToAction("Index", "Eventos");
        }

        [Route("Home/Error/{statusCode?}")]
        public IActionResult Error(int? statusCode = null)
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode,
                Message = statusCode switch
                {
                    400 => "La solicitud no es válida.",
                    401 => "No tienes autorización.",
                    403 => "No tienes permiso para acceder a este recurso.",
                    404 => "La página que buscas no fue encontrada.",
                    500 => "Ocurrió un error interno en el servidor.",
                    _ => "Ocurrió un error inesperado."
                }
            };

            return View(model);
        }
    }
}