using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using Microsoft.Win32;
using ProyectScrum.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ProyectScrum.Forms.Admin
{
    public partial class CrossData : Form
    {
        public CrossData()
        {
            InitializeComponent();
        }
        private void exportButton_Click(object? sender, EventArgs e)
        {
            panelData.Controls.Clear(); 
            int y = 10;

            using var conn = new SqlDataAccess().GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(
                "SELECT MangaID, Titulo FROM Mangas ORDER BY Titulo", conn);
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                int id = rd.GetInt32(0);
                string titulo = rd.GetString(1);

                var btn = new Guna.UI2.WinForms.Guna2Button
                {
                    Size = new Size(panelData.Width - 20, 40),
                    Location = new Point(10, y),
                    Text = titulo,
                    Tag = id,
                    BorderRadius = 6,
                    FillColor = Color.FromArgb(60, 90, 165),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };
                btn.Click += (_, __) => ExportarUnManga(id);
                panelData.Controls.Add(btn);
                y += 50;
            }
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // 3)  HANDLER ExpAllButton  —exporta TODO
        // ─────────────────────────────────────────────────────────────────────────────
        private void ExpAllButton_Click(object? sender, EventArgs e)
        {
            using var conn = new SqlDataAccess().GetConnection();
            conn.Open();

            // obtenemos todos los IDs primero
            List<int> ids = new();
            using (var cmd = new SqlCommand("SELECT MangaID FROM Mangas", conn))
            using (var rd = cmd.ExecuteReader())
                while (rd.Read()) ids.Add(rd.GetInt32(0));

            if (ids.Count == 0)
            {
                MessageBox.Show("No hay mangas para exportar.");
                return;
            }

            StringBuilder sb = new StringBuilder("/* === EXPORTACIÓN MASIVA === */\n");
            sb.AppendLine("BEGIN TRANSACTION;");

            foreach (int id in ids)
                sb.AppendLine(GenerarScriptManga(id, conn));

            sb.AppendLine("COMMIT;");

            GuardarArchivoSQL(sb.ToString(), $"Mangas_{DateTime.Now:yyyyMMdd}.sql");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // 4)  Exportar UN manga (invocado por cada botón del panel)
        // ─────────────────────────────────────────────────────────────────────────────
        private void ExportarUnManga(int mangaID)
        {
            using var conn = new SqlDataAccess().GetConnection();
            conn.Open();

            string script = GenerarScriptManga(mangaID, conn);
            if (string.IsNullOrWhiteSpace(script))
            {
                MessageBox.Show("No se encontró el manga en la base de datos.");
                return;
            }

            GuardarArchivoSQL(script, $"Manga_{mangaID}.sql");
        }

        // ─────────────────────────────────────────────────────────────────────
        // 5)  Genera el script SQL  (con verificación de duplicados por Título)
        // ─────────────────────────────────────────────────────────────────────
        private static string GenerarScriptManga(int mangaID, SqlConnection conn)
        {
            static string Esc(string? s) => s?.Replace("'", "''") ?? "";

            // ── Recuperar datos ──────────────────────────────────────────────
            using var mangaCmd = new SqlCommand(@"
        SELECT Titulo, Autor, Descripcion, Estado,
               CONVERT(date, FechaPublicacion) AS Fec,
               URLMangaDrive, URLPortada, GeneroID
        FROM   Mangas
        WHERE  MangaID = @id", conn);
            mangaCmd.Parameters.AddWithValue("@id", mangaID);

            using var rdM = mangaCmd.ExecuteReader();
            if (!rdM.Read()) return "";

            string titulo = Esc(rdM.GetString(0));
            string autor = Esc(rdM.GetString(1));
            string descripcion = Esc(rdM.GetString(2));
            string estado = Esc(rdM.GetString(3));
            string fecha = rdM.GetDateTime(4).ToString("yyyy-MM-dd");
            string urlDrive = Esc(rdM.GetString(5));
            string urlPortada = Esc(rdM.GetString(6));
            int generoID = rdM.GetInt32(7);
            rdM.Close();

            var sb = new StringBuilder();
            sb.AppendLine($"\n/* ===============  {titulo}  =============== */");

            // ▶️ Nueva protección contra duplicados
            sb.AppendLine($"IF EXISTS (SELECT 1 FROM Mangas WHERE Titulo = N'{titulo}')");
            sb.AppendLine("BEGIN");
            sb.AppendLine($"    PRINT 'Manga \"{titulo}\" ya existe — se omitió.';");
            sb.AppendLine("    RETURN;");
            sb.AppendLine("END");

            sb.AppendLine("DECLARE @NewMangaID INT;");

            sb.AppendLine($@"INSERT INTO Mangas
        (Titulo, Autor, Descripcion, Estado,
         FechaPublicacion, URLMangaDrive, URLPortada, GeneroID)
VALUES (N'{titulo}', N'{autor}', N'{descripcion}', N'{estado}',
        '{fecha}', N'{urlDrive}', N'{urlPortada}', {generoID});
SET @NewMangaID = SCOPE_IDENTITY();");

            // ── Títulos alternativos ─────────────────────────────────────────
            using var altCmd = new SqlCommand(
                "SELECT TituloAlternativo FROM TitulosAlternativos WHERE MangaID=@id", conn);
            altCmd.Parameters.AddWithValue("@id", mangaID);

            using var rdAlt = altCmd.ExecuteReader();
            while (rdAlt.Read())
            {
                string alt = Esc(rdAlt.GetString(0));
                sb.AppendLine($@"INSERT INTO TitulosAlternativos
            (MangaID, TituloAlternativo)
VALUES (@NewMangaID, N'{alt}');");
            }

            return sb.ToString();
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // 6)  Guarda contenido en .sql con SaveFileDialog
        // ─────────────────────────────────────────────────────────────────────────────
        private static void GuardarArchivoSQL(string contenido, string nombreSugerido)
        {
            using var dlg = new SaveFileDialog
            {
                Filter = "Script SQL (*.sql)|*.sql",
                FileName = nombreSugerido,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            File.WriteAllText(dlg.FileName, contenido, Encoding.UTF8);
            MessageBox.Show("Script exportado correctamente.",
                            "Exportación terminada",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        // ─────────────────────────────────────────────────────────────────────
        // 7)  IMPORTAR SCRIPTS  (.sql)  — evita duplicados antes de ejecutar
        // ─────────────────────────────────────────────────────────────────────
        private void importDataButton_Click(object? sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Selecciona el script de manga a importar",
                Filter = "Script SQL (*.sql)|*.sql",
                Multiselect = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            foreach (string path in dlg.FileNames)
            {
                string script = File.ReadAllText(path, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(script))
                {
                    MessageBox.Show($"El archivo {Path.GetFileName(path)} está vacío.");
                    continue;
                }

                // ─── Detectar título dentro del script ───────────────────────
                string? titulo = ExtraerTitulo(script);

                if (!string.IsNullOrEmpty(titulo) && MangaYaExiste(titulo))
                {
                    MessageBox.Show($"⚠️  El manga «{titulo}» ya existe en la base de datos.\n" +
                                    $"Se omitió la importación de {Path.GetFileName(path)}.",
                                    "Duplicado detectado",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;   // salta este archivo
                }

                // ─── Ejecutar (no hay duplicado) ─────────────────────────────
                try
                {
                    EjecutarScriptEnBD(script);
                    MessageBox.Show($"✅  «{Path.GetFileName(path)}» importado correctamente.",
                                    "Importación OK",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"⚠️  Error al importar «{Path.GetFileName(path)}»\n\n{ex.Message}",
                                    "Fallo en la importación",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// Extrae el título del manga del script exportado.
        private static string? ExtraerTitulo(string script)
        {
            var m = Regex.Match(script,
                @"IF\s+EXISTS\s*\([^\)]*Titulo\s*=\s*N'([^']+)'",
                RegexOptions.IgnoreCase);
            return m.Success ? m.Groups[1].Value : null;
        }

        /// Comprueba si ya existe un manga con ese título en la BD.
        private static bool MangaYaExiste(string titulo)
        {
            using var conn = new SqlDataAccess().GetConnection();
            conn.Open();

            using var cmd = new SqlCommand(
                "SELECT 1 FROM Mangas WHERE Titulo = @t", conn);
            cmd.Parameters.AddWithValue("@t", titulo);
            return cmd.ExecuteScalar() != null;
        }

        private static void EjecutarScriptEnBD(string script)
        {
            using var conn = new SqlDataAccess().GetConnection();
            conn.Open();

            using var tx = conn.BeginTransaction();
            using var cmd = new SqlCommand(script, conn, tx)
            {
                CommandTimeout = 0
            };

            try
            {
                cmd.ExecuteNonQuery();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

    }
}
