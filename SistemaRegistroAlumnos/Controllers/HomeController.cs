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
                        if (row.Cell(1).IsEmpty() && row.Cell(2).IsEmpty() && row.Cell(3).IsEmpty())
                            continue;

                        try
                        {
                            // === FECHA (Columna 1) ===
                            DateTime fecha;
                            if (row.Cell(1).TryGetValue<DateTime>(out var dt))
                            {
                                fecha = dt.Date;
                            }
                            else
                            {
                                var fechaStr = row.Cell(1).GetString().Trim();
                                if (!DateTime.TryParseExact(fechaStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
                                {
                                    ViewBag.Error = $"Formato de fecha inválido en fila {row.RowNumber()}: '{fechaStr}'. Usa dd/MM/yyyy.";
                                    return View();
                                }
                            }

                            // === DEMÁS COLUMNAS ===
                            string alumnoStr = row.Cell(2).GetString().Trim();
                            string unidadStr = row.Cell(3).GetString().Trim();
                            string estadoStr = row.Cell(4).GetString().Trim();

                            if (!int.TryParse(alumnoStr, out int alumnoId))
                            {
                                ViewBag.Error = $"Fila {row.RowNumber()}: el valor '{alumnoStr}' no es un ID de alumno válido.";
                                return View();
                            }

                            if (!int.TryParse(unidadStr, out int unidadId))
                            {
                                ViewBag.Error = $"Fila {row.RowNumber()}: el valor '{unidadStr}' no es un ID de unidad válido.";
                                return View();
                            }

                            if (!int.TryParse(estadoStr, out int estadoId))
                            {
                                ViewBag.Error = $"Fila {row.RowNumber()}: el valor '{estadoStr}' no es un ID de estado válido.";
                                return View();
                            }

                            // === VALIDACIÓN DE EXISTENCIA EN LA BD ===
                            bool alumnoExiste = _context.Alumno.Any(a => a.Id_Alumno == alumnoId);
                            if (!alumnoExiste)
                            {
                                ViewBag.Error = $"Fila {row.RowNumber()}: el alumno con ID {alumnoId} no existe en la base de datos.";
                                return View();
                            }

                            bool unidadExiste = _context.Unidades.Any(u => u.Id_Unidades == unidadId);
                            if (!unidadExiste)
                            {
                                ViewBag.Error = $"Fila {row.RowNumber()}: la unidad con ID {unidadId} no existe en la base de datos.";
                                return View();
                            }

                            bool estadoExiste = _context.EstadoAsistencia.Any(e => e.Id_EstadoAsistencia == estadoId);
                            if (!estadoExiste)
                            {
                                ViewBag.Error = $"Fila {row.RowNumber()}: el estado de asistencia con ID {estadoId} no existe en la base de datos.";
                                return View();
                            }

                            // === EVITAR DUPLICADOS ===
                            bool existe = _context.Asistencia.Any(a =>
                                a.Id_Alumno_Asis == alumnoId &&
                                a.Id_Unidad_Asis == unidadId &&
                                a.Fecha_Asis == fecha);

                            if (!existe)
                            {
                                asistencias.Add(new Asistencia
                                {
                                    Id_Alumno_Asis = alumnoId,
                                    Id_Unidad_Asis = unidadId,
                                    Id_EstadoAsis_Asis = estadoId,
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

            // === GUARDAR EN LA BD ===
            if (asistencias.Any())
            {
                try
                {
                    _context.Asistencia.AddRange(asistencias);
                    await _context.SaveChangesAsync();
                    ViewBag.Exito = $"Se insertaron {asistencias.Count} registros correctamente.";
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Ocurrió un error al guardar los datos: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Info = "No se insertaron nuevos registros (posiblemente ya existen o no hay datos válidos).";
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
