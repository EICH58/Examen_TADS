using Microsoft.AspNetCore.Mvc;
using SistemaRegistroAlumnos.Data;

namespace SistemaRegistroAlumnos.Controllers.Graficas
{
    [Route("Graficas")]
    public class G_PastelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public G_PastelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===========================
        // 📊 VISTA PRINCIPAL DE PASTEL
        // ===========================
        [HttpGet("G_Pastel")]
        public IActionResult G_Pastel()
        {
            // Renderiza directamente la vista G_Pastel.cshtml
            return View("~/Views/Graficas/G_Pastel.cshtml");
        }
    }
}
