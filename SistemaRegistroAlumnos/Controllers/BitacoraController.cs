using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaRegistroAlumnos.Data;
using System.Linq;
using System.Security.Claims;

namespace SistemaRegistroAlumnos.Controllers
{
    [Authorize]
    public class BitacoraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BitacoraController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Vista principal - SOLO para Coordinadores
        public IActionResult Index()
        {
            // Verificar si es Coordinador
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var usuario = _context.Usuarios.Find(userId);

            if (usuario?.Rol != "Coordinador")
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // API para obtener registros
        [HttpGet]
        public IActionResult ObtenerRegistros(int page = 1, int pageSize = 50, string? filtroAccion = null)
        {
            var query = _context.Bitacora
                .Include(b => b.Usuario)
                .AsQueryable();

            // Filtro opcional por acción
            if (!string.IsNullOrEmpty(filtroAccion))
            {
                query = query.Where(b => b.Accion.Contains(filtroAccion));
            }

            var total = query.Count();

            var registros = query
                .OrderByDescending(b => b.Fecha_Hora)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new
                {
                    b.Id_Bitacora,
                    b.Accion,
                    b.Descripcion,
                    b.Fecha_Hora,
                    b.IP_Address,
                    Usuario = b.Usuario.NombreCompleto ?? b.Usuario.NombreUsuario
                })
                .ToList();

            return Json(new { registros, total, page, pageSize });
        }
    }
}