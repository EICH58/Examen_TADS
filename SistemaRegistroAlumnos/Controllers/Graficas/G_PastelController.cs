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

        // ===========================
        // 📊 Datos para Gráfica de Pastel (Aprobados vs Reprobados)
        // Umbral: 70 (>=70 Aprobado, <70 Reprobado)
        // ===========================
        [HttpGet("/api/pastel/calificaciones")]
        public IActionResult ObtenerCalificacionesPastel(int? idCarrera, int? idMateria, int? idUnidad)
        {
            try
            {
                // === CASO 1: Filtro por UNIDAD específica ===
                if (idUnidad.HasValue)
                {
                    var promediosPorUnidad = _context.Calificaciones
                        .Where(c => c.Id_Unidad_Calif == idUnidad.Value)
                        .GroupBy(c => c.Id_Alumno_Calif)
                        .Select(g => Math.Round(g.Average(c => c.Calif_Indiv), 2))
                        .ToList();

                    return Json(promediosPorUnidad.Select(v => (double)v));
                }

                // === CASO 2: Filtro por MATERIA ===
                if (idMateria.HasValue)
                {
                    var idsUnidades = _context.Unidades
                        .Where(u => u.Id_Materia_Unidad == idMateria.Value)
                        .Select(u => u.Id_Unidades)
                        .ToList();

                    if (!idsUnidades.Any())
                    {
                        return Json(new double[] { });
                    }

                    var promediosPorMateria = _context.Calificaciones
                        .Where(c => idsUnidades.Contains(c.Id_Unidad_Calif))
                        .GroupBy(c => c.Id_Alumno_Calif)
                        .Select(g => Math.Round(g.Average(c => c.Calif_Indiv), 2))
                        .ToList();

                    return Json(promediosPorMateria.Select(v => (double)v));
                }

                // === CASO 3: Filtro por CARRERA ===
                if (idCarrera.HasValue)
                {
                    var idsAlumnos = _context.Alumno
                        .Where(a => a.Id_Carrera_Alum == idCarrera.Value)
                        .Select(a => a.Id_Alumno)
                        .ToList();

                    if (!idsAlumnos.Any())
                    {
                        return Json(new double[] { });
                    }

                    var promediosPorCarrera = _context.Calificaciones
                        .Where(c => idsAlumnos.Contains(c.Id_Alumno_Calif))
                        .GroupBy(c => c.Id_Alumno_Calif)
                        .Select(g => Math.Round(g.Average(c => c.Calif_Indiv), 2))
                        .ToList();

                    return Json(promediosPorCarrera.Select(v => (double)v));
                }

                // === CASO 4: SIN FILTROS - Todos los alumnos ===
                var promediosGenerales = _context.Calificaciones
                    .GroupBy(c => c.Id_Alumno_Calif)
                    .Select(g => Math.Round(g.Average(c => c.Calif_Indiv), 2))
                    .ToList();

                return Json(promediosGenerales.Select(v => (double)v));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en pastel: {ex.Message}");
                return Json(new { error = ex.Message });
            }
        }
    }
}