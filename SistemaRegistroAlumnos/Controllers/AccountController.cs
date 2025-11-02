// Controllers/AccountController.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaRegistroAlumnos.Data;
using SistemaRegistroAlumnos.Models;
using System.Security.Claims;

namespace SistemaRegistroAlumnos.Controllers
{
    [AllowAnonymous] // Permite acceso sin login
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string nombreUsuario, string contraUsuario)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.NombreUsuario == nombreUsuario && u.ContraUsuario == contraUsuario);

            if (usuario != null)
            {
                // Crear claims (datos del usuario)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                // Iniciar sesión
                await HttpContext.SignInAsync("CookieAuth", principal);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        // Cerrar sesión
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }

        public IActionResult Denied()
        {
            return View();
        }
    }
}