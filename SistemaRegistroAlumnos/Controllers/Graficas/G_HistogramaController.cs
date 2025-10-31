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

        // ===========================
        // 📊 Vista principal (NECESARIA para /Graficas/G_Histograma)
        // ===========================
        [HttpGet("G_Histograma")]
        public IActionResult G_Histograma()
        {
            return View("~/Views/Graficas/G_Histograma.cshtml");
        }

        // ===========================
        // 📈 API: Calificaciones para histograma
        // ===========================
        [HttpGet("/api/histograma/calificaciones")]
        public IActionResult ObtenerCalificacionesHistograma(int? idCarrera, int? idMateria, int? idUnidad)
        {
            var q =
                from c in _context.Calificaciones
                join a in _context.Alumno on c.Id_Alumno_Calif equals a.Id_Alumno
                join u in _context.Unidades on c.Id_Unidad_Calif equals u.Id_Unidades
                join m in _context.Materias on u.Id_Materia_Unidad equals m.Id_Materia
                select new
                {
                    Alumno = a.Id_Alumno,
                    CarreraAlumno = a.Id_Carrera_Alum,   // carrera real del alumno
                    Materia = m.Id_Materia,
                    Unidad = u.Id_Unidades,
                    Calif = (decimal?)c.Calif_Indiv
                };

            if (idCarrera.HasValue)
                q = q.Where(x => x.CarreraAlumno == idCarrera.Value);

            if (idMateria.HasValue)
                q = q.Where(x => x.Materia == idMateria.Value);

            if (idUnidad.HasValue)
                q = q.Where(x => x.Unidad == idUnidad.Value);

            // Trae datos a memoria y promedia por alumno (sin inflar)
            var lista = q.Where(x => x.Calif != null).ToList();

            var promediosPorAlumno = lista
                .GroupBy(x => x.Alumno)
                .Select(g => Math.Round(g.Average(y => y.Calif!.Value), 2))
                .Select(v => (double)v)
                .ToList();

            return Json(promediosPorAlumno);
        }

        // ===========================
        // 📘 Catálogos
        // ===========================
        [HttpGet("/api/carreras")]
        public IActionResult ObtenerCarreras()
        {
            var carreras = _context.Carrera
                .Select(c => new { id_Carrera = c.Id_Carrera, nombre_Carrera = c.Nombre_Carrera })
                .ToList();
            return Json(carreras);
        }

        [HttpGet("/api/materias/{idCarrera}")]
        public IActionResult ObtenerMateriasPorCarrera(int idCarrera)
        {
            var materias = _context.Materias
                .Where(m => m.Id_Carrera_Materia == idCarrera)
                .Select(m => new { id_Materia = m.Id_Materia, nombre_Materia = m.Nombre_Materia })
                .ToList();
            return Json(materias);
        }

        [HttpGet("/api/unidades/{idMateria}")]
        public IActionResult ObtenerUnidadesPorMateria(int idMateria)
        {
            var unidades = _context.Unidades
                .Where(u => u.Id_Materia_Unidad == idMateria)
                .Select(u => new { id_Unidades = u.Id_Unidades, nombre_Unidad = u.Nombre_Unidad })
                .ToList();
            return Json(unidades);
        }
    }
}
