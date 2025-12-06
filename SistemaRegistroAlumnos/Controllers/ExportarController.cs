using Microsoft.AspNetCore.Mvc;
using SistemaRegistroAlumnos.Includes;
using System;
using System.IO;

namespace SistemaRegistroAlumnos.Controllers
{
    [Route("api/exportar")]
    public class ExportarController : Controller
    {
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

                // ✅ RESPUESTA CORRECTA
                Response.Headers.Append("Content-Disposition", $"attachment; filename={nombreArchivo}");
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {
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
