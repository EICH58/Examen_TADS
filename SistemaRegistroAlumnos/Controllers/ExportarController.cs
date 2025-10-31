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
                if (datos == null || string.IsNullOrEmpty(datos.ImagenBase64))
                    return BadRequest(new { exito = false, error = "Datos inválidos o incompletos." });

                // Convertir imagen Base64 → bytes
                byte[] bytes = Convert.FromBase64String(
                    datos.ImagenBase64.Replace("data:image/png;base64,", "").Trim()
                );

                // Generar el PDF (horizontal, con tabla e imagen)
                string ruta = AuxiliarPDF.GenerarPDF(
                    datos.Titulo ?? "Gráfica generada",
                    datos.Descripcion ?? "Gráfica generada desde el sistema.",
                    bytes,
                    datos.TablaHTML ?? "",
                    "ReportesGenerados",
                    datos.Prefijo ?? "Grafica"
                );

                // Leer el PDF generado en memoria para descargarlo
                byte[] pdfBytes = System.IO.File.ReadAllBytes(ruta);

                // Nombre de descarga
                string nombreArchivo = Path.GetFileName(ruta);

                // Enviar el PDF como descarga directa
                return File(pdfBytes, "application/pdf", nombreArchivo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = false, error = "Error al generar el PDF: " + ex.Message });
            }
        }
    }

    // Clase auxiliar para recibir datos del frontend
    public class DatosGrafica
    {
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string? ImagenBase64 { get; set; }
        public string? TablaHTML { get; set; }   // 👈 nuevo campo
        public string? Prefijo { get; set; }
    }
}
