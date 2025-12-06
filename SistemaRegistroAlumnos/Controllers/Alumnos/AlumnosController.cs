
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaRegistroAlumnos.Data;
using SistemaRegistroAlumnos.Models;
using System.Linq;
using System.Threading.Tasks;


namespace SistemaRegistroAlumnos.Controllers.Alumnos
{
    public class AlumnosController : Controller
    {

        public ActionResult BuscarAlumnos()
        {
            return View();
        }

        public ActionResult RegistroAlumno()
        {
            return View();
        }

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

            // 🔥 CORRECCIÓN: Usar SelectList en lugar de List<SelectListItem>
            ViewBag.StatusAlumnos = new SelectList(
                new[]
                {
            new { Id = "1", Nombre = "Activo" },
            new { Id = "2", Nombre = "Egresado" },
            new { Id = "3", Nombre = "Baja temporal" },
            new { Id = "4", Nombre = "Baja definitiva" }
                },
                "Id",   // Value
                "Nombre" // Text
            );

            return View();
        }

        // ==============================================
        // 🧾 AGREGAR ALUMNO (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarAlumno(Alumno alumno)
        {
            try
            {
                // Validación adicional personalizada
                if (!string.IsNullOrEmpty(alumno.Num_Control) && _context.Alumno.Any(a => a.Num_Control == alumno.Num_Control))
                {
                    ModelState.AddModelError("Num_Control", "❌ Este número de control ya está registrado");
                }

                if (ModelState.IsValid)
                {
                    // Validar que el Id_Status_Alum esté entre los válidos
                    var statusValido = _context.StatusAlumno
                        .Where(s => s.Estado_Status == "Activo"
                                 || s.Estado_Status == "Egresado"
                                 || s.Estado_Status == "Baja temporal"
                                 || s.Estado_Status == "Baja definitiva")
                        .Any(s => s.Id_Status == alumno.Id_Status_Alum);

                    if (!statusValido)
                    {
                        TempData["MensajeError"] = "❌ El estado seleccionado no es válido.";
                    }
                    else
                    {
                        _context.Alumno.Add(alumno);
                        _context.SaveChanges();
                        TempData["MensajeExito"] = $"✅ Alumno {alumno.Nom_Alumno} registrado correctamente.";
                        return RedirectToAction("AgregarAlumno");
                    }
                }
                else
                {
                    TempData["MensajeError"] = "❌ Verifique los campos del formulario.";
                }
            }
            catch (Exception ex)
            {
                var mensajeError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["MensajeError"] = "⚠️ Error al registrar el alumno: " + mensajeError;
            }

            // 🔥 CORRECCIÓN: Recargar combos usando SelectList en lugar de List<SelectListItem>
            ViewBag.Carreras = new SelectList(_context.Carrera.ToList(), "Id_Carrera", "Nombre_Carrera");
            ViewBag.Semestres = new SelectList(_context.Semestre.ToList(), "Id_Semestre", "Num_Semestre");

            // 🔥 CORRECCIÓN: Usar SelectList para StatusAlumnos también
            ViewBag.StatusAlumnos = new SelectList(
                new[]
                {
            new { Id = "1", Nombre = "Activo" },
            new { Id = "2", Nombre = "Egresado" },
            new { Id = "3", Nombre = "Baja temporal" },
            new { Id = "4", Nombre = "Baja definitiva" }
                },
                "Id",   // Value
                "Nombre" // Text
            );

            return View(alumno);
        }

        // 1. Editar información completa
        [HttpGet]
        public IActionResult EditarAlumno(string numControl)
        {
            if (string.IsNullOrEmpty(numControl))
                return NotFound();

            var alumno = _context.Alumno
                .Include(a => a.Carrera)
                .Include(a => a.Semestre)
                .FirstOrDefault(a => a.Num_Control == numControl);

            if (alumno == null)
                return NotFound();

            ViewBag.Carreras = new SelectList(_context.Carrera, "Id_Carrera", "Nombre_Carrera", alumno.Id_Carrera_Alum);
            ViewBag.Semestres = new SelectList(_context.Semestre, "Id_Semestre", "Num_Semestre", alumno.Id_Semestre_Alum);
            ViewBag.StatusAlumnos = new SelectList(new[]
            {
        new { Id = 1, Nombre = "Activo" },
        new { Id = 2, Nombre = "Egresado" },
        new { Id = 3, Nombre = "Baja temporal" },
        new { Id = 4, Nombre = "Baja definitiva" }
    }, "Id", "Nombre", alumno.Id_Status_Alum);

            return View(alumno);
        }
        public IActionResult EditarAlumno(Alumno alumno)
        {
            if (!ModelState.IsValid)
            {
                CargarCombos(alumno);
                return View(alumno);
            }

            try
            {
                
                _context.Attach(alumno);                              
                _context.Entry(alumno).State = EntityState.Modified;   
                _context.SaveChanges();                               

                TempData["BusquedaOk"] = $"¡Alumno {alumno.Nom_Alumno} actualizado correctamente!";
                return RedirectToAction("BuscarAlumnos");
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al guardar: " + ex.Message;
                CargarCombos(alumno);
                return View(alumno);
            }
        }

        // Método auxiliar para no repetir código
        private void CargarCombos(Alumno alumno)
        {
            ViewBag.Carreras = new SelectList(_context.Carrera, "Id_Carrera", "Nombre_Carrera", alumno.Id_Carrera_Alum);
            ViewBag.Semestres = new SelectList(_context.Semestre, "Id_Semestre", "Num_Semestre", alumno.Id_Semestre_Alum);
            ViewBag.StatusAlumnos = new SelectList(new[]
            {
        new { Id = 1, Nombre = "Activo" },
        new { Id = 2, Nombre = "Egresado" },
        new { Id = 3, Nombre = "Baja temporal" },
        new { Id = 4, Nombre = "Baja definitiva" }
    }, "Id", "Nombre", alumno.Id_Status_Alum);
        }

        // 2. Cambiar solo estatus
        [HttpPost]
        public IActionResult CambiarEstatus(string numControl, int nuevoEstatus)
        {
            var alumno = _context.Alumno.FirstOrDefault(a => a.Num_Control == numControl);
            if (alumno == null)
            {
                TempData["Error"] = "Alumno no encontrado";
            }
            else
            {
                alumno.Id_Status_Alum = nuevoEstatus;
                _context.SaveChanges();
                TempData["BusquedaOk"] = $"Estatus del alumno {alumno.Nom_Alumno} cambiado correctamente.";
            }
            return RedirectToAction("BuscarAlumnos");
        }


        [HttpGet]
        public IActionResult MateriasAlumno(string numControl)
        {
            if (string.IsNullOrEmpty(numControl))
                return NotFound();

            var alumno = _context.Alumno
                .Include(a => a.Carrera)
                .Include(a => a.Semestre)
                .FirstOrDefault(a => a.Num_Control == numControl);

            if (alumno == null)
                return NotFound();

            return View(alumno);  
        }

    }
}
