using System;
using System.IO;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace SistemaRegistroAlumnos.Includes
{
    public static class AuxiliarPDF
    {
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

                // ========== SELECCIÓN DE TIPO DE GRÁFICA ==========
                using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    switch (prefijoNombre)
                    {
                        case "G_Dispersion":
                            GenerarPDF_Dispersion(fs, titulo, descripcion, imagenPngBytes, tablaHTML);
                            break;

                        case "G_Histograma":
                            GenerarPDF_Histograma(fs, titulo, descripcion, imagenPngBytes, tablaHTML);
                            break;

                        case "G_Pastel":
                            GenerarPDF_Pastel(fs, titulo, descripcion, imagenPngBytes, tablaHTML);
                            break;

                        default:
                            throw new Exception("Tipo de gráfico no reconocido o no soportado.");
                    }
                }

                return fullPath;
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.Message;
            }
        }

        // ==========================================================
        // =============== GRÁFICA DE DISPERSIÓN ====================
        // ==========================================================
        private static void GenerarPDF_Dispersion(FileStream fs, string titulo, string descripcion, byte[] imagenPngBytes, string tablaHTML)
        {
            using (Document doc = new Document(PageSize.A4.Rotate(), 42f, 42f, 56f, 56f))
            {
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Título
                var pTitulo = new Paragraph("Gráfica de Dispersión (Asistencia vs Calificación Final)",
                    new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 6f
                };
                doc.Add(pTitulo);

                // Subtítulo / Descripción
                string[] partes = descripcion.Split('|');
                string materia = partes.Length > 0 ? partes[0] : "";
                string unidad = partes.Length > 1 ? partes[1] : "";

                var pDetalles = new Paragraph(
                    $"Carrera: {titulo}\nMateria: {materia}\nUnidad: {unidad}",
                    new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL))
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 18f
                };
                doc.Add(pDetalles);

                // Imagen
                if (imagenPngBytes != null && imagenPngBytes.Length > 0)
                {
                    var img = Image.GetInstance(imagenPngBytes);
                    img.Alignment = Element.ALIGN_CENTER;
                    img.ScaleToFit(700f, 400f);
                    img.SpacingAfter = 18f;
                    doc.Add(img);
                }

                // Tabla
                AgregarTituloTabla(doc, "Datos de la gráfica (Asistencia vs Calificación)");
                AgregarTablaDesdeHTML(doc, tablaHTML, 6);

                doc.Close();
            }
        }

        // ==========================================================
        // ================= GRÁFICA DE HISTOGRAMA ==================
        // ==========================================================
        private static void GenerarPDF_Histograma(FileStream fs, string titulo, string descripcion, byte[] imagenPngBytes, string tablaHTML)
        {
            using (Document doc = new Document(PageSize.A4, 42f, 42f, 56f, 56f))
            {
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Título principal
                var pTitulo = new Paragraph("Histograma (Distribución de Calificaciones)",
                    new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 6f
                };
                doc.Add(pTitulo);

                // Subtítulo
                var pSubtitulo = new Paragraph(descripcion,
                    new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL))
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 16f
                };
                doc.Add(pSubtitulo);

                // Imagen
                if (imagenPngBytes != null && imagenPngBytes.Length > 0)
                {
                    var img = Image.GetInstance(imagenPngBytes);
                    img.Alignment = Element.ALIGN_CENTER;
                    img.ScaleToFit(460f, 320f);
                    img.SpacingAfter = 18f;
                    doc.Add(img);
                }

                // Tabla resumen
                AgregarTituloTabla(doc, "Resumen de la gráfica");
                AgregarTablaDesdeHTML(doc, tablaHTML, 4);

                // Descripción
                var pDesc = new Paragraph(
                    "El histograma muestra la frecuencia de alumnos por rangos de calificación (0–59, 60–69, 70–79, 80–100). " +
                    "Puede filtrarse por Carrera, Materia y Unidad. Si no se elige Unidad, se usa el promedio general del alumno.",
                    new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL))
                {
                    SpacingBefore = 16f,
                    Alignment = Element.ALIGN_JUSTIFIED
                };
                doc.Add(pDesc);

                doc.Close();
            }
        }

        // ==========================================================
        // ============== MÉTODOS AUXILIARES GENERALES ==============
        // ==========================================================
        private static void AgregarTituloTabla(Document doc, string texto)
        {
            var p = new Paragraph(texto,
                new Font(Font.FontFamily.HELVETICA, 13, Font.BOLD))
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingBefore = 8f,
                SpacingAfter = 10f
            };
            doc.Add(p);
        }

        private static void AgregarTablaDesdeHTML(Document doc, string tablaHTML, int columnas)
        {
            if (string.IsNullOrWhiteSpace(tablaHTML))
                return;

            var rowRegex = new Regex(@"<tr>(.*?)</tr>", RegexOptions.Singleline);
            var rows = rowRegex.Matches(tablaHTML);

            var table = new PdfPTable(columnas);
            table.WidthPercentage = 100;
            table.SpacingBefore = 10f;
            table.SpacingAfter = 12f;

            bool esPrimeraFila = true;
            foreach (Match row in rows)
            {
                var cols = Regex.Matches(row.Value, @"<t[dh][^>]*>(.*?)</t[dh]>", RegexOptions.Singleline);
                if (cols.Count == 0) continue;

                foreach (Match col in cols)
                {
                    string textoCelda = Regex.Replace(col.Groups[1].Value, "<.*?>", "").Trim();

                    bool esHeader = esPrimeraFila;
                    var font = esHeader
                        ? new Font(Font.FontFamily.HELVETICA, 11f, Font.BOLD, BaseColor.WHITE)
                        : new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL, BaseColor.BLACK);

                    var cell = new PdfPCell(new Phrase(textoCelda, font))
                    {
                        PaddingTop = 7f,
                        PaddingBottom = 7f,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        BackgroundColor = esHeader ? new BaseColor(67, 74, 64) : BaseColor.WHITE,
                        BorderColor = new BaseColor(150, 150, 150)
                    };
                    table.AddCell(cell);
                }
                esPrimeraFila = false;
            }

            doc.Add(table);
        }

        // ==========================================================
        // ================= GRÁFICA DE PASTEL ======================
        // ==========================================================
        private static void GenerarPDF_Pastel(FileStream fs, string titulo, string descripcion, byte[] imagenPngBytes, string tablaHTML)
        {
            using (Document doc = new Document(PageSize.A4, 42f, 42f, 56f, 56f))
            {
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Título principal
                var pTitulo = new Paragraph("Gráfica de Pastel (Aprobados vs Reprobados)",
                    new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 6f
                };
                doc.Add(pTitulo);

                // Subtítulo
                var pSubtitulo = new Paragraph(descripcion,
                    new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL))
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 16f
                };
                doc.Add(pSubtitulo);

                // Imagen
                if (imagenPngBytes != null && imagenPngBytes.Length > 0)
                {
                    var img = Image.GetInstance(imagenPngBytes);
                    img.Alignment = Element.ALIGN_CENTER;
                    img.ScaleToFit(400f, 400f);
                    img.SpacingAfter = 18f;
                    doc.Add(img);
                }

                // Tabla resumen
                AgregarTituloTabla(doc, "Resumen de la gráfica");
                AgregarTablaDesdeHTML(doc, tablaHTML, 5);

                // Descripción
                var pDesc = new Paragraph(
                    "Este diagrama de pastel muestra el porcentaje de aprobación y reprobación de los alumnos. " +
                    "Se considera aprobado con calificación ≥ 70 y reprobado con < 70.",
                    new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL))
                {
                    SpacingBefore = 16f,
                    Alignment = Element.ALIGN_JUSTIFIED
                };
                doc.Add(pDesc);

                doc.Close();
            }
        }
    }
}
