using Microsoft.AspNetCore.Mvc;
using SistemaRegistroAlumnos.Data;
using System;
using System.Linq;

namespace SistemaRegistroAlumnos.Controllers
{
    public class GraficasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GraficasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================== VISTAS =====================
        public ActionResult G_Pastel() => View();
        public ActionResult G_Histograma() => View();
        public ActionResult G_Dispersion() => View();
        public ActionResult G_Pareto() => View();

        // ===================================================
        // MÉTODO NUEVO: ObtenerDatosMateriaUnidadCarrera
        // ===================================================
        [HttpGet]
        public IActionResult ObtenerDatosMateriaUnidadCarrera(int? idCarrera = 1, int? idMateria = 1, int? idUnidad = 1)
        {
            var query =
                from a in _context.Asistencia
                join ea in _context.EstadoAsistencia on a.Id_EstadoAsis_Asis equals ea.Id_EstadoAsistencia
                join u in _context.Unidades on a.Id_Unidad_Asis equals u.Id_Unidades
                join m in _context.Materias on u.Id_Materia_Unidad equals m.Id_Materia
                join crr in _context.Carrera on m.Id_Carrera_Materia equals crr.Id_Carrera
                join al in _context.Alumno on a.Id_Alumno_Asis equals al.Id_Alumno
                join c in _context.Calificaciones
                    on new { Alumno = a.Id_Alumno_Asis, Unidad = a.Id_Unidad_Asis }
                    equals new { Alumno = c.Id_Alumno_Calif, Unidad = c.Id_Unidad_Calif }
                select new { a, ea, u, m, al, c, crr };

            if (idCarrera.HasValue)
                query = query.Where(x => x.crr.Id_Carrera == idCarrera.Value);

            if (idMateria.HasValue)
                query = query.Where(x => x.m.Id_Materia == idMateria.Value);

            if (idUnidad.HasValue)
                query = query.Where(x => x.u.Id_Unidades == idUnidad.Value);

            var datos = query
                .GroupBy(x => new { x.al.Nom_Alumno, x.crr.Nombre_Carrera, x.m.Nombre_Materia, x.u.Nombre_Unidad })
                .Select(g => new
                {
                    Alumno = g.Key.Nom_Alumno,
                    Carrera = g.Key.Nombre_Carrera,
                    Materia = g.Key.Nombre_Materia,
                    Unidad = g.Key.Nombre_Unidad,
                    PorcentajeAsistencia = Math.Round(
                        g.Sum(x =>
                            x.ea.Estado_Asistencia == "Presente" ? 1.0 :
                            (x.ea.Id_EstadoAsistencia == 2 || x.ea.Id_EstadoAsistencia == 3) ? 0.5 : 0.0
                        ) * 100.0 / g.Count(), 2),
                    CalificacionUnidad = Math.Round(g.Average(x => x.c.Calif_Indiv), 2)
                })
                .OrderBy(x => x.Alumno)
                .ThenBy(x => x.Unidad)
                .ToList();

            return Json(datos);
        }

        // ===================================================
        // ENDPOINT PARA LLENAR SELECT DE CARRERAS
        // ===================================================
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

        // ===================================================
        // ENDPOINT PARA LLENAR SELECT DE MATERIAS SEGÚN CARRERA
        // ===================================================
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

        // ===================================================
        // ENDPOINT PARA LLENAR SELECT DE UNIDADES SEGÚN MATERIA
        // ===================================================
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
