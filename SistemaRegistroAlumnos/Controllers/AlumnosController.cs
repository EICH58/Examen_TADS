using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaRegistroAlumnos.Data;
using SistemaRegistroAlumnos.Models;

namespace SistemaRegistroAlumnos.Controllers
{
    public class AlumnosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AlumnosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AlumnosController1
        public ActionResult BuscarAlumnos()
        {
            return View();
        }

        public ActionResult RegistroAlumno()
        {
            return View();
        }

        public async Task<IActionResult> ListaAlumnos()
        {
            var alumnos = await _context.Alumno
                .Include(a => a.Carrera)
                .Include(a => a.Semestre)
                .Include(a => a.StatusAlumno)
                .ToListAsync();
            return View(alumnos);
        }

        public async Task<IActionResult> Index()
        {
            var alumnos = await _context.Alumno
                .Include(a => a.Carrera)
                .Include(a => a.Semestre)
                .Include(a => a.StatusAlumno)
                .ToListAsync();
            return View(alumnos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var alumno = await _context.Alumno
                .Include(a => a.Carrera)
                .Include(a => a.Semestre)
                .Include(a => a.StatusAlumno)
                .Include(a => a.PerfilSocioeconomico)
                .FirstOrDefaultAsync(a => a.Id_Alumno == id);

            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        // GET: Alumno/Create
      /*  public IActionResult AgregarAlumno()
        {
            // Asegúrate de cargar los ViewBag antes de retornar la vista
            ViewBag.Carrera = new SelectList(_context.Carrera, "Id_Carrera", "Nombre_Carrera");
            ViewBag.Semestre = new SelectList(_context.Semestre, "Id_Semestre", "Num_Semestre");
            ViewBag.StatusAlumno = new SelectList(_context.Status_Alumno, "Id_StatusAlumno", "Estado_Status");

            return View();
        }*/

        // GET: Alumno/AgregarAlumno
        /*public IActionResult AgregarAlumno()
        {
            return View();
        }*/

        // GET: Alumno/AgregarAlumno
        public IActionResult AgregarAlumno()
        {
            CargarViewBags();
            return View();
        }
        private void CargarViewBags(Alumno alumno = null)
        {
            // Obtener solo los valores válidos (excluir opciones por defecto con ID 0)
            var carreras = _context.Carrera.Where(c => c.Id_Carrera != 0).ToList();
            var semestres = _context.Semestre.Where(s => s.Id_Semestre != 0).ToList();
            var statusAlumnos = _context.Status_Alumno.Where(st => st.Id_Status != 0).ToList();

            ViewBag.Carreras = new SelectList(carreras, "Id_Carrera", "Nombre_Carrera", alumno?.Id_Carrera_Alum);
            ViewBag.Semestres = new SelectList(semestres, "Id_Semestre", "Num_Semestre", alumno?.Id_Semestre_Alum);
            ViewBag.StatusAlumnos = new SelectList(statusAlumnos, "Id_Status", "Estado_Status", alumno?.Id_Status_Alum);
        }

        /* private void CargarViewBags(Alumno? alumno = null)
         {
             // Verificar que hay datos antes de crear los SelectList
             var carreras = _context.Carrera.ToList();
             var semestres = _context.Semestre.ToList();
             var statusAlumnos = _context.Status_Alumno.ToList();

             if (!carreras.Any())
             {
                 // Si no hay carreras, crear una opción por defecto
                 carreras = new List<Carrera> { new Carrera { Id_Carrera = 0, Nombre_Carrera = "No hay carreras disponibles" } };
             }

             if (!semestres.Any())
             {
                 semestres = new List<Semestre> { new Semestre { Id_Semestre = 0, Num_Semestre = 0 } };
             }

             if (!statusAlumnos.Any())
             {
                 statusAlumnos = new List<StatusAlumno> { new StatusAlumno { Id_Status = 0, Estado_Status = "No hay status disponibles" } };
             }

             ViewBag.Carrera = new SelectList(carreras, "Id_Carrera", "Nombre_Carrera", alumno?.Id_Carrera_Alum);
             ViewBag.Semestre = new SelectList(semestres, "Id_Semestre", "Num_Semestre", alumno?.Id_Semestre_Alum);
             ViewBag.StatusAlumno = new SelectList(statusAlumnos, "Id_Status", "Estado_Status", alumno?.Id_Status_Alum);
         }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarAlumno(Alumno alumno)
        {
            // LOG para debugging
            Console.WriteLine($"Datos recibidos:");
            Console.WriteLine($"Num_Control: {alumno.Num_Control}");
            Console.WriteLine($"Nombre: {alumno.Nom_Alumno}");
            Console.WriteLine($"Id_Carrera_Alum: {alumno.Id_Carrera_Alum}");
            Console.WriteLine($"Id_Semestre_Alum: {alumno.Id_Semestre_Alum}");
            Console.WriteLine($"Id_Status_Alum: {alumno.Id_Status_Alum}");

            // Remover la validación de propiedades de navegación
            ModelState.Remove("Carrera");
            ModelState.Remove("Semestre");
            ModelState.Remove("StatusAlumno");
            ModelState.Remove("Asistencias");
            ModelState.Remove("Calificaciones");
            ModelState.Remove("PerfilSocioeconomico");

            // Validar que las claves foráneas existan
            bool erroresValidacion = false;

            // Validar Carrera
            var carreraExiste = await _context.Carrera.AnyAsync(c => c.Id_Carrera == alumno.Id_Carrera_Alum);
            Console.WriteLine($"Carrera existe: {carreraExiste}");
            if (!carreraExiste || alumno.Id_Carrera_Alum == 0)
            {
                ModelState.AddModelError("Id_Carrera_Alum", "Debe seleccionar una carrera válida");
                erroresValidacion = true;
            }

            // Validar Semestre
            var semestreExiste = await _context.Semestre.AnyAsync(s => s.Id_Semestre == alumno.Id_Semestre_Alum);
            Console.WriteLine($"Semestre existe: {semestreExiste}");
            if (!semestreExiste || alumno.Id_Semestre_Alum == 0)
            {
                ModelState.AddModelError("Id_Semestre_Alum", "Debe seleccionar un semestre válido");
                erroresValidacion = true;
            }

            // Validar Status
            var statusExiste = await _context.Status_Alumno.AnyAsync(st => st.Id_Status == alumno.Id_Status_Alum);
            Console.WriteLine($"Status existe: {statusExiste}");
            if (!statusExiste || alumno.Id_Status_Alum == 0)
            {
                ModelState.AddModelError("Id_Status_Alum", "Debe seleccionar un status válido");
                erroresValidacion = true;
            }

            if (erroresValidacion)
            {
                Console.WriteLine("Errores de validación encontrados");
                CargarViewBags(alumno);
                return View(alumno);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("ModelState es válido, intentando guardar...");

                    // Verificar si ya existe un alumno con el mismo número de control
                    var alumnoExistente = await _context.Alumno
                        .FirstOrDefaultAsync(a => a.Num_Control == alumno.Num_Control);

                    if (alumnoExistente != null)
                    {
                        ModelState.AddModelError("Num_Control", "Ya existe un alumno con este número de control");
                        CargarViewBags(alumno);
                        return View(alumno);
                    }

                    _context.Add(alumno);
                    await _context.SaveChangesAsync();

                    Console.WriteLine("Alumno guardado exitosamente!");
                    TempData["SuccessMessage"] = "Alumno agregado correctamente";
                    return RedirectToAction(nameof(ListaAlumnos));
                }
                catch (DbUpdateException ex)
                {
                    // Log del error interno para debugging
                    var innerException = ex.InnerException?.Message;
                    Console.WriteLine($"DbUpdateException: {innerException ?? ex.Message}");

                    ModelState.AddModelError("", $"Error de base de datos: {innerException ?? ex.Message}");

                    // Si es error de clave foránea, mostrar mensaje más específico
                    if (innerException?.Contains("547") == true)
                    {
                        ModelState.AddModelError("", "Error: Uno de los valores seleccionados (Carrera, Semestre o Status) no existe en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    ModelState.AddModelError("", "Error al guardar el alumno: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("ModelState no es válido");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Error en {error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            CargarViewBags(alumno);
            return View(alumno);
        }

        /*  [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> AgregarAlumno(Alumno alumno)
          {
              // Remover la validación de propiedades de navegación
              ModelState.Remove("Carrera");
              ModelState.Remove("Semestre");
              ModelState.Remove("StatusAlumno");
              ModelState.Remove("Asistencias");
              ModelState.Remove("Calificaciones");
              ModelState.Remove("PerfilSocioeconomico");

              // Validar que las claves foráneas existan
              bool erroresValidacion = false;

              // Validar Carrera
              var carreraExiste = await _context.Carrera.AnyAsync(c => c.Id_Carrera == alumno.Id_Carrera_Alum);
              if (!carreraExiste)
              {
                  ModelState.AddModelError("Id_Carrera_Alum", "La carrera seleccionada no existe");
                  erroresValidacion = true;
              }

              // Validar Semestre
              var semestreExiste = await _context.Semestre.AnyAsync(s => s.Id_Semestre == alumno.Id_Semestre_Alum);
              if (!semestreExiste)
              {
                  ModelState.AddModelError("Id_Semestre_Alum", "El semestre seleccionado no existe");
                  erroresValidacion = true;
              }

              // Validar Status
              var statusExiste = await _context.Status_Alumno.AnyAsync(st => st.Id_Status == alumno.Id_Status_Alum);
              if (!statusExiste)
              {
                  ModelState.AddModelError("Id_Status_Alum", "El status seleccionado no existe");
                  erroresValidacion = true;
              }

              if (erroresValidacion)
              {
                  CargarViewBags(alumno);
                  return View(alumno);
              }

              if (ModelState.IsValid)
              {
                  try
                  {
                      // Verificar si ya existe un alumno con el mismo número de control
                      var alumnoExistente = await _context.Alumno
                          .FirstOrDefaultAsync(a => a.Num_Control == alumno.Num_Control);

                      if (alumnoExistente != null)
                      {
                          ModelState.AddModelError("Num_Control", "Ya existe un alumno con este número de control");
                          CargarViewBags(alumno);
                          return View(alumno);
                      }

                      _context.Add(alumno);
                      await _context.SaveChangesAsync();

                      TempData["SuccessMessage"] = "Alumno agregado correctamente";
                      return RedirectToAction(nameof(Index));
                  }
                  catch (DbUpdateException ex)
                  {
                      // Log del error interno para debugging
                      var innerException = ex.InnerException?.Message;
                      ModelState.AddModelError("", $"Error de base de datos: {innerException ?? ex.Message}");

                      // Si es error de clave foránea, mostrar mensaje más específico
                      if (innerException?.Contains("547") == true)
                      {
                          ModelState.AddModelError("", "Error: Uno de los valores seleccionados (Carrera, Semestre o Status) no existe en la base de datos.");
                      }
                  }
                  catch (Exception ex)
                  {
                      ModelState.AddModelError("", "Error al guardar el alumno: " + ex.Message);
                  }
              }

              CargarViewBags(alumno);
              return View(alumno);
          }
          // POST: Alumno/AgregarAlumno
          /*   [HttpPost]
             [ValidateAntiForgeryToken]
             public async Task<IActionResult> AgregarAlumno(Alumno alumno)
             {
                 if (ModelState.IsValid)
                 {
                     try
                     {
                         // Verificar si ya existe un alumno con el mismo número de control
                         var alumnoExistente = await _context.Alumno
                             .FirstOrDefaultAsync(a => a.Num_Control == alumno.Num_Control);

                         if (alumnoExistente != null)
                         {
                             ModelState.AddModelError("Num_Control", "Ya existe un alumno con este número de control");
                             return View(alumno);
                         }

                         _context.Add(alumno);
                         await _context.SaveChangesAsync();

                         TempData["SuccessMessage"] = "Alumno agregado correctamente";
                         return RedirectToAction(nameof(Index));
                     }
                     catch (DbUpdateException ex)
                     {
                         ModelState.AddModelError("", "Error de base de datos: " + ex.Message);
                     }
                     catch (Exception ex)
                     {
                         ModelState.AddModelError("", "Error al guardar el alumno: " + ex.Message);
                     }
                 }

                 Console.WriteLine("Datos recibidos:");
                 Console.WriteLine($"Num_Control: {alumno.Num_Control}");
                 Console.WriteLine($"Nombre: {alumno.Nom_Alumno}");
                 Console.WriteLine($"Apellido Paterno: {alumno.App_Alumno}");
                 Console.WriteLine($"Apellido Materno: {alumno.Apm_Alumno}");
                 Console.WriteLine($"Género: {alumno.Gen_Alumno}");
                 Console.WriteLine($"Fecha_Nac: {alumno.Fecha_Nac}");
                 Console.WriteLine($"Colonia: {alumno.Colonia_Alum}");
                 Console.WriteLine($"Calle: {alumno.Calle_Alum}");
                 Console.WriteLine($"Numero Casa: {alumno.Num_Casa}");
                 Console.WriteLine($"Numero Ext: {alumno.Num_Ext}");
               // Agrega más campos para ver qué datos llegan

               if (ModelState.IsValid)
                 {
                     // resto del código...
                 }

                 // Muestra errores de validación
                 foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                 {
                     Console.WriteLine($"Error: {error.ErrorMessage}");
                 }

                 return View(alumno);
             }

             // POST: Alumno/Create

             /* [HttpPost]
              [ValidateAntiForgeryToken]
              public async Task<IActionResult> AgregarAlumno(Alumno alumno)
              {
                  if (ModelState.IsValid)
                  {
                      try
                      {
                          _context.Add(alumno);
                          await _context.SaveChangesAsync();
                          return RedirectToAction(nameof(Index));
                      }
                      catch (Exception ex)
                      {
                          ModelState.AddModelError("", "Error al guardar el alumno: " + ex.Message);
                      }
                  }

                  // Recargar los ViewBag si hay error en la validación
                  ViewBag.Carrera = new SelectList(_context.Carrera, "Id_Carrera", "Nombre_Carrera", alumno.Id_Carrera_Alum);
                  ViewBag.Semestre = new SelectList(_context.Semestre, "Id_Semestre", "Num_Semestre", alumno.Id_Semestre_Alum);
                  ViewBag.StatusAlumno = new SelectList(_context.Status_Alumno, "Id_StatusAlumno", "Estado_Status", alumno.Id_Status_Alum);

                  return View(alumno);
              } */
        // Más acciones: Create, Edit, Delete, etc.
    }
}
