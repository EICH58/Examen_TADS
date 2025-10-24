using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemaRegistroAlumnos.Controllers
{
    public class AlumnosController : Controller
    {
        // GET: AlumnosController1
        public ActionResult BuscarAlumnos()
        {
            return View();
        }

        public ActionResult RegistroAlumno()
        {
            return View();
        }
    }
}
