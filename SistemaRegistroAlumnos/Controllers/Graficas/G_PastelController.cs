using Microsoft.AspNetCore.Mvc;
using SistemaRegistroAlumnos.Data;
using System;
using System.Linq;

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

        [HttpGet("G_Pastel")]
        public IActionResult G_Pastel()
        {
            return View("~/Views/Graficas/G_Pastel.cshtml");
        }

        [HttpGet("GetDatosPastel")]
        public IActionResult GetDatosPastel(int? idCarrera, int? idMateria, int? idUnidad)
        {
            try
            {
                // === CASO 1: Filtrar por UNIDAD específica ===
                if (idUnidad.HasValue)
                {
                    var promsUnidad = _context.Calificaciones
                        .Where(c => c.Id_Unidad_Calif == idUnidad.Value)
                        .GroupBy(c => c.Id_Alumno_Calif)
                        .Select(g => Math.Round(g.Average(c => c.Calif_Indiv), 2))
                        .ToList();

                    int aprob = promsUnidad.Count(p => p >= 70);
                    int reprob = promsUnidad.Count(p => p < 70);

                    return Json(new { labels = new[] { "Aprobado (≥70)", "Reprobado (<70)" }, data = new[] { aprob, reprob } });
                }

                // === CASO 2: Filtrar por MATERIA (promedio considerando TODAS sus unidades) ===
                if (idMateria.HasValue)
                {
                    var idsUnidades = _context.Unidades
                        .Where(u => u.Id_Materia_Unidad == idMateria.Value)
                        .Select(u => u.Id_Unidades)
                        .ToList();

                    if (!idsUnidades.Any())
                        return Json(new { labels = new[] { "Aprobado (≥70)", "Reprobado (<70)" }, data = new[] { 0, 0 } });

                    var promsMateria = _context.Calificaciones
                        .Where(c => idsUnidades.Contains(c.Id_Unidad_Calif))
                        .GroupBy(c => c.Id_Alumno_Calif)
                        .Select(g => Math.Round(g.Average(c => c.Calif_Indiv), 2))
                        .ToList();

                    int aprob = promsMateria.Count(p => p >= 70);
                    int reprob = promsMateria.Count(p => p < 70);

                    return Json(new { labels = new[] { "Aprobado (≥70)", "Reprobado (<70)" }, data = new[] { aprob, reprob } });
                }

                // === CASO 3: Filtrar por CARRERA ===
                if (idCarrera.HasValue)
                {
                    var idsAlumnos = _context.Alumno
                        .Where(a => a.Id_Carrera_Alum == idCarrera.Value)
                        .Select(a => a.Id_Alumno)
                        .ToList();

                    if (!idsAlumnos.Any())
                        return Json(new { labels = new[] { "Aprobado (≥70)", "Reprobado (<70)" }, data = new[] { 0, 0 } });

                    var promsCarrera = _context.Calificaciones
                        .Where(c => idsAlumnos.Contains(c.Id_Alumno_Calif))
                        .GroupBy(c => c.Id_Alumno_Calif)
                        .Select(g => Math.Round(g.Average(c => c.Calif_Indiv), 2))
                        .ToList();

                    int aprob = promsCarrera.Count(p => p >= 70);
                    int reprob = promsCarrera.Count(p => p < 70);

                    return Json(new { labels = new[] { "Aprobado (≥70)", "Reprobado (<70)" }, data = new[] { aprob, reprob } });
                }

                // === CASO 4: SIN FILTROS ===
                var promsGenerales = _context.Calificaciones
                    .GroupBy(c => c.Id_Alumno_Calif)
                    .Select(g => Math.Round(g.Average(c => c.Calif_Indiv), 2))
                    .ToList();

                int aprobados = promsGenerales.Count(p => p >= 70);
                int reprobados = promsGenerales.Count(p => p < 70);

                return Json(new { labels = new[] { "Aprobado (≥70)", "Reprobado (<70)" }, data = new[] { aprobados, reprobados } });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en pastel: {ex.Message}");
                return Json(new { error = ex.Message, labels = new[] { "Aprobado (≥70)", "Reprobado (<70)" }, data = new[] { 0, 0 } });
            }
        }
    }
}
