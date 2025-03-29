using ProyectScrum.Entities;
using ProyectScrum.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace ProyectScrum.Forms
{
    public partial class Catalog : Form
    {
        private int paginaActual = 1;
        private int mangasPorPagina = 12;
        public Catalog()
        {
            InitializeComponent();
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            Main mainForm = new Main();
            mainForm.Show();
            this.Close();
        }

        private void Catalog_Load(object sender, EventArgs e)
        {
            MostrarPortadas();

            anteriorButton.Visible = false;
            siguienteButton.Visible = false;


        }

        //obtener mangas
        private List<Manga> ObtenerMangas()
        {
            List<Manga> mangas = new List<Manga>();
            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = @"SELECT MangaID, Titulo, URLPortada
                                 FROM Mangas
                                 ORDER BY FechaPublicacion DESC
                                 OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@offset", (paginaActual - 1) * mangasPorPagina);
                cmd.Parameters.AddWithValue("@limit", mangasPorPagina);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mangas.Add(new Manga
                    {
                        MangaID = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        URLPortada = reader.GetString(2)
                    });
                }
            }

            return mangas;
        }
        private int ObtenerTotalMangas()
        {
            int total = 0;
            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Mangas";
                SqlCommand cmd = new SqlCommand(query, conn);
                total = (int)cmd.ExecuteScalar();
            }

            return total;
        }


        //obterner portadas
        private void MostrarPortadas()
        {
            flowLayoutPanel1.Controls.Clear();
            var mangas = ObtenerMangas();

            foreach (var manga in mangas)
            {
                PictureBox pb = new PictureBox
                {
                    Width = 185,
                    Height = 240,
                    Margin = new Padding(30, 20, 30, 20),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Cursor = Cursors.Hand,
                    Tag = manga.Titulo
                };
                pb.Image = Properties.Resources.LoadingGif;
                pb.LoadCompleted += (s, e) =>
                {
                    if (e.Error != null)
                    {
                        pb.Image = Properties.Resources.DefaultCover;
                    }
                    
                };

                try
                {
                    pb.LoadAsync(manga.URLPortada);
                }
                catch
                {
                    pb.Image = Properties.Resources.DefaultCover;
                }

                ToolTip tip = new ToolTip();
                tip.SetToolTip(pb, manga.Titulo);

                pb.Click += (s, e) =>
                {
                    string titulo = (string)((PictureBox)s).Tag;
                    MessageBox.Show($"Seleccionaste: {titulo}", "Manga seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                flowLayoutPanel1.Controls.Add(pb);

                int totalMangas = ObtenerTotalMangas();
                int totalPaginas = (int)Math.Ceiling((double)totalMangas / mangasPorPagina);

                // Mostrar u ocultar botones según la página actual
                anteriorButton.Visible = paginaActual > 1;
                siguienteButton.Visible = paginaActual < totalPaginas;
            }
        }
        //botones 
        private void siguienteButton_Click(object sender, EventArgs e)
        {
            paginaActual++;
            MostrarPortadas();
        }

        private void anteriorButton_Click(object sender, EventArgs e)
        {
            if (paginaActual > 1)
            {
                paginaActual--;
                MostrarPortadas();
            }
        }

        //redibujo para slidebar
        public void ForzarRedibujarLayout()
        {
            flowLayoutPanel1.PerformLayout();
            flowLayoutPanel1.Refresh();
        }

    }
}
