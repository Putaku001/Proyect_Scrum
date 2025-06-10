using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data;
using System.Data.SqlClient;
using System.IO;

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

            string nombreArchivo = $"ReporteUsuariosPremium_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string rutaArchivo = Path.Combine(rutaDestino, nombreArchivo);

            try
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(50);
                        page.Size(PageSizes.A4);

                        page.Header().Column(col =>
                        {
                            col.Item().Text("REPORTE DE USUARIOS PREMIUM")
                                .FontSize(22).Bold().AlignCenter();

                            col.Item().Element(e => e
                                .AlignCenter()
                                .PaddingBottom(15)
                                .Text($"Fecha de generación: {DateTime.Now:dd/MM/yyyy}")
                                .FontSize(12));
                        });

                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellHeader).Text("Usuario");
                                header.Cell().Element(CellHeader).Text("Email");
                                header.Cell().Element(CellHeader).Text("Fin de suscripción");

                                static IContainer CellHeader(IContainer container)
                                {
                                    return container
                                        .Background("#007ACC")
                                        .Border(1).BorderColor(Colors.Grey.Darken2)
                                        .AlignCenter()
                                        .PaddingVertical(6)
                                        .PaddingHorizontal(4)
                                        .DefaultTextStyle(x => x.FontSize(12).Bold().FontColor(Colors.White));
                                }
                            });

                            foreach (DataRow row in tabla.Rows)
                            {
                                table.Cell().Element(CellStyle).Text(row["NombreUsuario"].ToString());
                                table.Cell().Element(CellStyle).Text(row["Email"].ToString());
                                table.Cell().Element(CellStyle).Text(Convert.ToDateTime(row["FechaFinSuscripcion"]).ToString("dd/MM/yyyy"));

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container
                                        .Border(1).BorderColor(Colors.Grey.Lighten2)
                                        .PaddingVertical(5).PaddingHorizontal(4)
                                        .AlignLeft();
                                }
                            }
                        });

                        page.Footer().AlignCenter().Text(text =>
                        {
                            text.Span("Página ");
                            text.CurrentPageNumber();
                            text.Span(" de ");
                            text.TotalPages();
                        });
                    });
                }).GeneratePdf(rutaArchivo);

                return $"Reporte guardado en: {rutaArchivo}";
            }
            catch (Exception ex)
            {
                return $"Error al generar el PDF: {ex.Message}";
            }
        }
    }
}
