using Microsoft.AspNetCore.Mvc;
using SistemaRegistroAlumnos.Data;
using SistemaRegistroAlumnos.Models;
using System.Diagnostics;
using ClosedXML.Excel;
using System.Globalization;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;

namespace SistemaRegistroAlumnos.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

       }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult info()
        {
            return View();
        }

        public IActionResult ExportarLista()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ExportarLista(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.Error = "Por favor, selecciona un archivo Excel.";
                return View();
            }

            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.Error = "Solo se permiten archivos con extensión .xlsx";
                return View();
            }

            var asistencias = new List<Asistencia>();

            using (var stream = new MemoryStream())
            {
                await excelFile.CopyToAsync(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheets.First();
                    var rows = worksheet.RowsUsed().Skip(1); // Saltar encabezado

                    foreach (var row in rows)
                    {
                        try
                        {
                            // === FECHA (Columna 1) ===
                            var fechaCell = row.Cell(1).Value;
                            DateTime fecha;

                            if (row.Cell(1).TryGetValue<DateTime>(out var dt))
                            {
                                fecha = dt.Date;
                            }
                            else
                            {
                                var fechaStr = fechaCell.ToString().Trim();
                                if (!DateTime.TryParseExact(fechaStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
                                {
                                    ViewBag.Error = $"Formato de fecha inválido en fila {row.RowNumber()}: '{fechaStr}'. Usa dd/MM/yyyy";
                                    return View();
                                }
                                fecha = fecha.Date;
                            }

                            // === RESTO DE COLUMNAS ===
                            var alumno = row.Cell(2).GetValue<int>();
                            var unidad = row.Cell(3).GetValue<int>();
                            var estado = row.Cell(4).GetValue<int>();

                            // Verificar duplicados
                            bool existe = _context.Asistencia.Any(a =>
                                a.Id_Alumno_Asis == alumno &&
                                a.Id_Unidad_Asis == unidad &&
                                a.Fecha_Asis.Date == fecha);

                            if (!existe)
                            {
                                asistencias.Add(new Asistencia
                                {
                                    Id_EstadoAsis_Asis = estado,
                                    Id_Alumno_Asis = alumno,
                                    Id_Unidad_Asis = unidad,
                                    Fecha_Asis = fecha
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Error = $"Error en fila {row.RowNumber()}: {ex.Message}";
                            return View();
                        }
                    }
                }
            }

            if (asistencias.Any())
            {
                _context.Asistencia.AddRange(asistencias);
                await _context.SaveChangesAsync();
                ViewBag.Exito = $"Se insertaron {asistencias.Count} registros de asistencia correctamente.";
            }
            else
            {
                ViewBag.Info = "No se insertaron nuevos registros (posiblemente ya existen o no hay datos).";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
