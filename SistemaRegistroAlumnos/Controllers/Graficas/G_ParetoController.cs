using Microsoft.AspNetCore.Mvc;
using SistemaRegistroAlumnos.Data;
using System;
using System.Linq;

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

        [HttpGet("G_Pareto")]
        public IActionResult G_Pareto()
        {
            return View("~/Views/Graficas/G_Pareto.cshtml");
        }

        // ===========================
        // 📊 Datos para Gráfica de Pareto (Factores de Riesgo)
        // Parámetro adicional: modo (ocurrencias o severidad)
        // ===========================
        [HttpGet("/api/pareto/factores")]
        public IActionResult ObtenerFactoresPareto(int? idCarrera, int? idMateria, int? idUnidad, string modo = "ocurrencias")
        {
            try
            {
                // Paso 1: Obtener IDs de alumnos según filtros (MISMA LÓGICA QUE HISTOGRAMA)
                var idsAlumnos = ObtenerAlumnosSegunFiltros(idCarrera, idMateria, idUnidad);

                if (!idsAlumnos.Any())
                {
                    return Json(new { labels = new string[] { }, data = new double[] { } });
                }

                // Paso 2: Obtener factores de esos alumnos
                var factores = _context.FactoresPorAlumno
                    .Where(f => idsAlumnos.Contains(f.Id_Alumno_factor))
                    .ToList();

                if (!factores.Any())
                {
                    return Json(new { labels = new string[] { }, data = new double[] { } });
                }

                // Paso 3: Agrupar según el modo
                var resultado = modo.ToLower() == "severidad"
                    ? factores
                        .GroupBy(f => f.Factor)
                        .Select(g => new
                        {
                            Factor = g.Key,
                            Valor = Math.Round(g.Average(x => x.Severidad), 2)
                        })
                        .OrderByDescending(x => x.Valor)
                        .ToList()
                    : factores
                        .GroupBy(f => f.Factor)
                        .Select(g => new
                        {
                            Factor = g.Key,
                            Valor = (double)g.Count()
                        })
                        .OrderByDescending(x => x.Valor)
                        .ToList();

                var labels = resultado.Select(x => x.Factor).ToArray();
                var data = resultado.Select(x => x.Valor).ToArray();

                return Json(new { labels, data });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Pareto: {ex.Message}");
                return Json(new { error = ex.Message, labels = new string[] { }, data = new double[] { } });
            }
        }

        // ===========================
        // 🔍 Método auxiliar con LÓGICA DEL HISTOGRAMA (que sí funciona)
        // ===========================
        private List<int> ObtenerAlumnosSegunFiltros(int? idCarrera, int? idMateria, int? idUnidad)
        {
            // === CASO 1: Filtro por UNIDAD específica ===
            if (idUnidad.HasValue)
            {
                return _context.Calificaciones
                    .Where(c => c.Id_Unidad_Calif == idUnidad.Value)
                    .Select(c => c.Id_Alumno_Calif)
                    .Distinct()
                    .ToList();
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
                    return new List<int>();
                }

                return _context.Calificaciones
                    .Where(c => idsUnidades.Contains(c.Id_Unidad_Calif))
                    .Select(c => c.Id_Alumno_Calif)
                    .Distinct()
                    .ToList();
            }

            // === CASO 3: Filtro por CARRERA ===
            if (idCarrera.HasValue)
            {
                return _context.Alumno
                    .Where(a => a.Id_Carrera_Alum == idCarrera.Value)
                    .Select(a => a.Id_Alumno)
                    .ToList();
            }

            // === CASO 4: SIN FILTROS - Todos los alumnos ===
            return _context.Alumno
                .Select(a => a.Id_Alumno)
                .ToList();
        }

        // ===========================
        // 📘 Los catálogos ya están en G_HistogramaController
        // ===========================
    }
}