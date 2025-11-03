using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaRegistroAlumnos.Data;
using SistemaRegistroAlumnos.Includes;
using SistemaRegistroAlumnos.Models;
using System.Security.Claims;

namespace SistemaRegistroAlumnos.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BitacoraService _bitacoraService;

        public AccountController(ApplicationDbContext context, BitacoraService bitacoraService)
        {
            _context = context;
            _bitacoraService = bitacoraService;
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
                // ✅ Crear claims CON EL ROL
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Rol) // ← AGREGAR ESTO
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                // Iniciar sesión
                await HttpContext.SignInAsync("CookieAuth", principal);

                // ✅ Registrar en bitácora (después de iniciar sesión)
                _bitacoraService.RegistrarAccion(
                    "Inicio de Sesión",
                    $"El usuario '{usuario.NombreCompleto ?? usuario.NombreUsuario}' inició sesión correctamente."
                );

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        // Cerrar sesión
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // ✅ Registrar ANTES de cerrar sesión
            _bitacoraService.RegistrarAccion("Cierre de Sesión", "El usuario cerró sesión correctamente.");

            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }

        public IActionResult Denied()
        {
            return View();
        }
    }
}