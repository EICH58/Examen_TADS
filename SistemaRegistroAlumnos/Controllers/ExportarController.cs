using Microsoft.AspNetCore.Mvc;
using SistemaRegistroAlumnos.Includes; // ✅ Para acceder a BitacoraService
using System;
using System.IO;

namespace SistemaRegistroAlumnos.Controllers
{
    [Route("api/exportar")]
    public class ExportarController : Controller
    {
        // ✅ Inyección del servicio de Bitácora
        private readonly BitacoraService _bitacoraService;

        // ✅ Constructor actualizado
        public ExportarController(BitacoraService bitacoraService)
        {
            _bitacoraService = bitacoraService;
        }

        [HttpPost("pdf")]
        public IActionResult ExportarPDF([FromBody] DatosGrafica datos)
        {
            try
            {
                // ===== VALIDACIÓN DE DATOS =====
                if (datos == null)
                    return Json(new { exito = false, error = "No se recibieron datos para generar el PDF." });

                if (string.IsNullOrEmpty(datos.ImagenBase64))
                    return Json(new { exito = false, error = "No se recibió la imagen de la gráfica." });

                // ===== CONVERTIR IMAGEN =====
                byte[] bytes = Convert.FromBase64String(
                    datos.ImagenBase64.Replace("data:image/png;base64,", "").Trim()
                );

                // ===== GENERAR PDF =====
                string ruta = AuxiliarPDF.GenerarPDF(
                    datos.Titulo ?? "Gráfica generada",
                    datos.Descripcion ?? "",
                    bytes,
                    datos.TablaHTML ?? "",
                    "ReportesGenerados",
                    datos.Prefijo ?? "Reporte"
                );

                // ===== VALIDAR RUTA =====
                if (ruta.StartsWith("ERROR:"))
                    return Json(new { exito = false, error = ruta });

                if (!System.IO.File.Exists(ruta))
                    return Json(new { exito = false, error = "El archivo PDF no se generó correctamente." });

                // ===== LEER PDF =====
                byte[] pdfBytes = System.IO.File.ReadAllBytes(ruta);
                string nombreArchivo = Path.GetFileName(ruta);

                // ✅ REGISTRO EN BITÁCORA
                _bitacoraService.RegistrarAccion(
                    "Exportar PDF",
                    $"Se generó correctamente el archivo '{nombreArchivo}' en {DateTime.Now:dd/MM/yyyy HH:mm:ss}"
                );

                // ✅ RESPUESTA CORRECTA
                Response.Headers.Append("Content-Disposition", $"attachment; filename={nombreArchivo}");
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                // ❌ REGISTRA ERRORES TAMBIÉN
                _bitacoraService.RegistrarAccion("Error PDF", $"Error al generar PDF: {ex.Message}");

                return Json(new { exito = false, error = "Error al generar el PDF: " + ex.Message });
            }
        }
    }

    // ===== CLASE AUXILIAR =====
    public class DatosGrafica
    {
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string? ImagenBase64 { get; set; }
        public string? TablaHTML { get; set; }
        public string? Prefijo { get; set; }
    }
}
