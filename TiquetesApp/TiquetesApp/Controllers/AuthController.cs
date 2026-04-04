using Microsoft.AspNetCore.Mvc;

namespace TiquetesApp.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        [HttpGet("login")]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Eventos");
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Eventos");
            return View();
        }
    }
}