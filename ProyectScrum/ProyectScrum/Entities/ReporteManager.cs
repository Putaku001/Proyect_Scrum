using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ProyectScrum.Entities
{
    public class ReporteManager
    {
        private readonly string _connectionString;

        public ReporteManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GenerarReporteUsuariosPremium(string rutaDestino)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT NombreUsuario, Email, FechaFinSuscripcion FROM Usuarios WHERE EsPremium = 1", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }

            if (tabla.Rows.Count == 0)
                return "No hay usuarios premium para mostrar.";

            Document doc = new Document(PageSize.A4, 50, 50, 80, 50);
            string nombreArchivo = $"ReporteUsuariosPremium_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string rutaArchivo = Path.Combine(rutaDestino, nombreArchivo);

            try
            {
                PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.Create));
                doc.Open();

                //Estilo de título
                iTextSharp.text.Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.BLACK);
                Paragraph titulo = new Paragraph("REPORTE DE USUARIOS PREMIUM", tituloFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                doc.Add(titulo);

                //Fecha
                iTextSharp.text.Font fechaFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                doc.Add(new Paragraph(new Phrase("Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy"), fechaFont)));
                doc.Add(new Paragraph("\n"));

                // Tabla con estilo
                PdfPTable pdfTable = new PdfPTable(3)
                {
                    WidthPercentage = 100
                };
                pdfTable.SetWidths(new float[] { 2, 3, 2 });

                //Encabezado
                string[] headers = { "Usuario", "Email", "Fin de suscripción" };
                iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
                BaseColor headerBgColor = new BaseColor(0, 102, 204); // Azul

                foreach (string header in headers)
                {
                    PdfPCell celda = new PdfPCell(new Phrase(header, headerFont))
                    {
                        BackgroundColor = headerBgColor,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                    pdfTable.AddCell(celda);
                }

                //Datos
                iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                foreach (DataRow row in tabla.Rows)
                {
                    pdfTable.AddCell(new PdfPCell(new Phrase(row["NombreUsuario"].ToString(), cellFont)) { Padding = 5 });
                    pdfTable.AddCell(new PdfPCell(new Phrase(row["Email"].ToString(), cellFont)) { Padding = 5 });
                    pdfTable.AddCell(new PdfPCell(new Phrase(Convert.ToDateTime(row["FechaFinSuscripcion"]).ToString("dd/MM/yyyy"), cellFont)) { Padding = 5 });
                }

                doc.Add(pdfTable);
                return $"Reporte guardado en: {rutaArchivo}";
            }
            catch (Exception ex)
            {
                return $"Error al generar el PDF: {ex.Message}";
            }
            finally
            {
                doc.Close();
            }
        }

    }
}
