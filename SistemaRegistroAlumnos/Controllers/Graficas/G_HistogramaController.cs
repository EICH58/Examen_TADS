using Microsoft.AspNetCore.Mvc;
using SistemaRegistroAlumnos.Data;
using System;
using System.Linq;

namespace SistemaRegistroAlumnos.Controllers.Graficas
{
    [Route("Graficas")]
    public class G_HistogramaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public G_HistogramaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("G_Histograma")]
        public IActionResult G_Histograma()
        {
            return View("~/Views/Graficas/G_Histograma.cshtml");
        }

        // ===========================
        // 📈 Calificaciones CORREGIDO V3 - FUNCIONAL
        // ===========================
        [HttpGet("/api/histograma/calificaciones")]
        public IActionResult ObtenerCalificacionesHistograma(int? idCarrera, int? idMateria, int? idUnidad)
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

                // === CASO 2: Filtro por MATERIA (promedio de todas sus unidades) ===
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

                // === CASO 3: Filtro por CARRERA (TODAS las calificaciones de alumnos de esa carrera) ===
                if (idCarrera.HasValue)
                {
                    // 🔑 CLAVE: Solo filtramos por alumnos de la carrera
                    // NO filtramos por materias de la carrera
                    var idsAlumnos = _context.Alumno
                        .Where(a => a.Id_Carrera_Alum == idCarrera.Value)
                        .Select(a => a.Id_Alumno)
                        .ToList();

                    if (!idsAlumnos.Any())
                    {
                        return Json(new double[] { });
                    }

                    // Obtener TODAS las calificaciones de esos alumnos
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
                Console.WriteLine($"❌ Error en histograma: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return Json(new { error = ex.Message });
            }
        }

        // ===========================
        // 📘 Catálogos
        // ===========================
        [HttpGet("/api/carreras")]
        public IActionResult ObtenerCarreras()
        {
            var carreras = _context.Carrera
                .Select(c => new
                {
                    id_Carrera = c.Id_Carrera,
                    nombre_Carrera = c.Nombre_Carrera
                })
                .ToList();

            return Json(carreras);
        }

        [HttpGet("/api/materias/{idCarrera}")]
        public IActionResult ObtenerMateriasPorCarrera(int idCarrera)
        {
            var materias = _context.Materias
                .Where(m => m.Id_Carrera_Materia == idCarrera)
                .Select(m => new
                {
                    id_Materia = m.Id_Materia,
                    nombre_Materia = m.Nombre_Materia
                })
                .ToList();

            return Json(materias);
        }

        [HttpGet("/api/unidades/{idMateria}")]
        public IActionResult ObtenerUnidadesPorMateria(int idMateria)
        {
            var unidades = _context.Unidades
                .Where(u => u.Id_Materia_Unidad == idMateria)
                .Select(u => new
                {
                    id_Unidades = u.Id_Unidades,
                    nombre_Unidad = u.Nombre_Unidad
                })
                .ToList();

            return Json(unidades);
        }
    }
}