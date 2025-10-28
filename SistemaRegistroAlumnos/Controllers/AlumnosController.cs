<<<<<<< Updated upstream
﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
=======
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaRegistroAlumnos.Data;
using SistemaRegistroAlumnos.Models;
using System.Linq;
using System.Threading.Tasks;
>>>>>>> Stashed changes

namespace SistemaRegistroAlumnos.Controllers
{
    public class AlumnosController : Controller
    {
<<<<<<< Updated upstream
        // GET: AlumnosController1
        public ActionResult BuscarAlumnos()
        {
            return View();
        }

        public ActionResult RegistroAlumno()
        {
            return View();
=======
        private readonly ApplicationDbContext _context;

        public AlumnosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==============================================
        // 🔍 BUSCAR ALUMNOS
        // ==============================================
        [HttpGet]
        public IActionResult BuscarAlumnos(string? numControl, int? carreraId, int? semestreId)
        {
            var alumnos = _context.Alumno
                .Include(a => a.Carrera)
                .Include(a => a.Semestre)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(numControl))
                alumnos = alumnos.Where(a => a.Num_Control.Contains(numControl));

            if (carreraId.HasValue && carreraId.Value > 0)
                alumnos = alumnos.Where(a => a.Id_Carrera_Alum == carreraId.Value);

            if (semestreId.HasValue && semestreId.Value > 0)
                alumnos = alumnos.Where(a => a.Id_Semestre_Alum == semestreId.Value);

            if (!string.IsNullOrEmpty(numControl) || carreraId.HasValue || semestreId.HasValue)
                TempData["BusquedaOk"] = "✅ Búsqueda realizada correctamente.";

            ViewBag.NumControl = numControl;
            ViewBag.CarreraId = carreraId;
            ViewBag.SemestreId = semestreId;
            ViewBag.Carreras = new SelectList(_context.Carrera.OrderBy(c => c.Nombre_Carrera), "Id_Carrera", "Nombre_Carrera");
            ViewBag.Semestres = new SelectList(_context.Semestre.OrderBy(s => s.Num_Semestre), "Id_Semestre", "Num_Semestre");

            return View(alumnos.ToList());
        }

        // ==============================================
        // 🧾 AGREGAR ALUMNO (GET)
        // ==============================================
        [HttpGet]
        public IActionResult AgregarAlumno()
        {
            ViewBag.Carreras = new SelectList(_context.Carrera.ToList(), "Id_Carrera", "Nombre_Carrera");
            ViewBag.Semestres = new SelectList(_context.Semestre.ToList(), "Id_Semestre", "Num_Semestre");

            /*
            ViewBag.StatusAlumnos = new SelectList(
                _context.Status_Alumno
                .Select(s => s.Estado_Status)
                .Distinct()
                .ToList()
            );
            */

            return View();
            
        }
            



        // ==============================================
        // 🧾 AGREGAR ALUMNO (POST)
        // ==============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarAlumno(Alumno alumno)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Alumno.Add(alumno);
                    _context.SaveChanges();

                    TempData["MensajeExito"] = $"✅ Alumno {alumno.Nom_Alumno} registrado correctamente.";
                    return RedirectToAction("AgregarAlumno");
                }
                else
                {
                    TempData["MensajeError"] = "❌ Verifique los campos del formulario.";
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "⚠️ Error al registrar el alumno: " + ex.Message;
            }

            // Si hay error, recargar los combos para que no se pierdan
            ViewBag.Carreras = new SelectList(_context.Carrera.ToList(), "Id_Carrera", "Nombre_Carrera");
            ViewBag.Semestres = new SelectList(_context.Semestre.ToList(), "Id_Semestre", "Num_Semestre");
           // ViewBag.StatusAlumnos = new SelectList(_context.Status_Alumno.ToList(), "Id_Status", "Estado_Status");

            return View(alumno);
>>>>>>> Stashed changes
        }
    }
}
