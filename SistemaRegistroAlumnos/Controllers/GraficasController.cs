using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SistemaRegistroAlumnos.Controllers
{
    public class GraficasController : Controller
    {
        // GET: GraficasController1
        public ActionResult G_Pastel()
        {
            return View();
        }

        public ActionResult G_Histograma()
        {
            return View();
        }

        public ActionResult G_Dispersion()
        {
            return View();
        }

        public ActionResult G_Pareto()
        {
            return View();
        }

    }
}
