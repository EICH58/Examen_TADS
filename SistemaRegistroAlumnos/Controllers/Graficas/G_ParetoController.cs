using Microsoft.AspNetCore.Mvc;
using SistemaRegistroAlumnos.Data;

namespace SistemaRegistroAlumnos.Controllers.Graficas
{
    [Route("Graficas")]
    public class G_ParetoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public G_ParetoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===========================
        // 📊 VISTA PRINCIPAL DE PARETO
        // ===========================
        [HttpGet("G_Pareto")]
        public IActionResult G_Pareto()
        {
            // Renderiza la vista directamente desde la carpeta Graficas
            return View("~/Views/Graficas/G_Pareto.cshtml");
        }
    }
}
