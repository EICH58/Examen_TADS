using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml; // 👈 Necesario para renderizar HTML (tabla)
using System.Text;

namespace SistemaRegistroAlumnos.Includes
{
    public static class AuxiliarPDF
    {
        /// <summary>
        /// Genera un PDF horizontal con título, descripción, imagen PNG (gráfica) y una tabla HTML.
        /// </summary>
        public static string GenerarPDF(
            string titulo,
            string descripcion,
            byte[] imagenPngBytes,
            string tablaHTML,
            string subcarpetaDestino = "ReportesGenerados",
            string? prefijoNombre = null
        )
        {
            try
            {
                // ========== CONFIGURACIÓN DE DIRECTORIO ==========
                string baseDir = Directory.GetCurrentDirectory();
                string destino = Path.Combine(baseDir, subcarpetaDestino);
                if (!Directory.Exists(destino))
                    Directory.CreateDirectory(destino);

                // ========== NOMBRE DEL ARCHIVO ==========
                string stamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string baseName = string.IsNullOrWhiteSpace(prefijoNombre) ? "Reporte" : prefijoNombre;
                string fileName = $"{baseName}_{stamp}.pdf";
                string fullPath = Path.Combine(destino, fileName);

                // ========== CREACIÓN DEL DOCUMENTO ==========
                // Landscape = A4 horizontal
                using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                using (Document doc = new Document(PageSize.A4.Rotate(), 42f, 42f, 56f, 56f))
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // ====== TÍTULO ======
                    var pTitulo = new Paragraph(string.IsNullOrWhiteSpace(titulo) ? "Reporte" : titulo)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 18f
                    };
                    doc.Add(pTitulo);

                    // ====== DESCRIPCIÓN ======
                    if (!string.IsNullOrWhiteSpace(descripcion))
                    {
                        var pDesc = new Paragraph(descripcion)
                        {
                            Alignment = Element.ALIGN_JUSTIFIED,
                            SpacingAfter = 16f
                        };
                        doc.Add(pDesc);
                    }

                    // ====== IMAGEN DE LA GRÁFICA ======
                    if (imagenPngBytes != null && imagenPngBytes.Length > 0)
                    {
                        var img = Image.GetInstance(imagenPngBytes);
                        img.Alignment = Element.ALIGN_CENTER;
                        img.ScaleToFit(700f, 400f);
                        img.SpacingAfter = 20f;
                        doc.Add(img);
                    }

                    // ====== TABLA HTML (si existe) ======
                    if (!string.IsNullOrWhiteSpace(tablaHTML))
                    {
                        using (var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(tablaHTML)))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, ms, null, Encoding.UTF8);
                        }
                    }


                    // ====== PIE DE PÁGINA ======
                    var pPie = new Paragraph($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm:ss}")
                    {
                        Alignment = Element.ALIGN_RIGHT,
                        SpacingBefore = 12f
                    };
                    doc.Add(pPie);

                    doc.Close();
                }

                return fullPath;
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.Message;
            }
        }
    }
}
