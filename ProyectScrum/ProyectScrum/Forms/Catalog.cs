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

            CargarCheckBoxGeneros();
            flowCheckBoxGeneros.FlowDirection = FlowDirection.TopDown;
            flowCheckBoxGeneros.WrapContents = false;
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
                    Tag = manga
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

                pb.Click += PictureBox_Click; // Usa el nuevo manejador de eventos

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
        //filtro 
        private void CargarCheckBoxGeneros()
        {
            flowCheckBoxGeneros.Controls.Clear();

            SqlDataAccess db = new SqlDataAccess();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT GeneroID, Nombre FROM Generos", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CheckBox chk = new CheckBox
                    {
                        Text = reader.GetString(1),
                        Tag = reader.GetInt32(0),
                        AutoSize = true,
                        ForeColor = Color.White,
                        Margin = new Padding(5, 3, 5, 3)
                    };
                    flowCheckBoxGeneros.Controls.Add(chk);
                }
            }
        }
        //mostrar portadas filtradas
        private void MostrarPortadasFiltradas(List<int> generosSeleccionados)
        {
            flowLayoutPanel1.Controls.Clear();

            if (generosSeleccionados.Count == 0)
            {
                MostrarPortadas();
                return;
            }

            List<Manga> mangas = new List<Manga>();
            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = $@"
                    SELECT MangaID, Titulo, URLPortada, GeneroID
                    FROM Mangas
                    WHERE GeneroID IN ({string.Join(",", generosSeleccionados)})
                    ORDER BY FechaPublicacion DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mangas.Add(new Manga
                    {
                        MangaID = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        URLPortada = reader.GetString(2),
                        GeneroID = reader.GetInt32(3)
                    });
                }
            }

            foreach (var manga in mangas)
            {
                PictureBox pb = new PictureBox
                {
                    Width = 185,
                    Height = 240,
                    Margin = new Padding(30, 20, 30, 20),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Cursor = Cursors.Hand,
                    Tag = manga,
                    Image = Properties.Resources.LoadingGif
                };

                pb.LoadCompleted += (s, e) =>
                {
                    if (e.Error != null)
                        pb.Image = Properties.Resources.DefaultCover;
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

                pb.Click += PictureBox_Click;

                flowLayoutPanel1.Controls.Add(pb);
            }

            anteriorButton.Visible = false;
            siguienteButton.Visible = false;
        }
        // btnfiltro click
        private void btnAplicarFiltro_Click(object sender, EventArgs e)
        {
            List<int> generosSeleccionados = new List<int>();

            foreach (CheckBox chk in flowCheckBoxGeneros.Controls)
            {
                if (chk.Checked)
                {
                    generosSeleccionados.Add((int)chk.Tag);
                }
            }

            MostrarPortadasFiltradas(generosSeleccionados);
            panelFiltro.Visible = false;
        }
        //cerrar filtro
        private void btnFiltro_Click(object sender, EventArgs e)
        {
            panelFiltro.Visible = !panelFiltro.Visible;
            panelFiltro.BringToFront();
        }

        private void Catalog_MouseDown(object sender, MouseEventArgs e)
        {
            // Solo cerrar si el panel está visible
            if (panelFiltro.Visible)
            {
                // Obtener punto del mouse en pantalla
                Point mousePos = this.PointToClient(Cursor.Position);
                // Si el mouse no está dentro del panel
                if (!panelFiltro.Bounds.Contains(mousePos))
                {
                    panelFiltro.Visible = false;
                }
            }
        }

        //Nuevo manejador de eventos para PictureBox al dar click
        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = (PictureBox)sender;
            Manga selectedManga = (Manga)clickedPictureBox.Tag;

            MostrarDetallesManga(selectedManga.MangaID);
        }

        //Para mostrar la info de los mangas desde la DB
        private void MostrarDetallesManga(int mangaId)
        {
            try
            {
                SqlDataAccess db = new SqlDataAccess();
                using (SqlConnection connection = db.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT Mangas.Titulo, Mangas.Autor, Mangas.Descripcion, Mangas.Estado, " +
                                    "Mangas.FechaPublicacion, Mangas.URLPortada, Generos.Nombre AS Genero FROM Mangas JOIN Generos " +
                                    "ON Mangas.GeneroID = Generos.GeneroID WHERE Mangas.MangaID = @MangaID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MangaID", mangaId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string titulo = reader.GetString(0);
                                string autor = reader.GetString(1);
                                string descripcion = reader.GetString(2);
                                string estado = reader.GetString(3);
                                DateTime fechaPublicacion = reader.GetDateTime(4);
                                string urlPortada = reader.GetString(5);
                                string genero = reader.GetString(6);

                                Form detallesForm = new Form();
                                detallesForm.Text = titulo;
                                detallesForm.Size = new Size(600, 400);

                                PictureBox portadaPictureBox = new PictureBox();
                                portadaPictureBox.Load(urlPortada);
                                portadaPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                                portadaPictureBox.Location = new Point(10, 10);
                                portadaPictureBox.Size = new Size(200, 300);

                                Label descripcionLabel = new Label();
                                descripcionLabel.Text = $"Descripción: {descripcion}";
                                descripcionLabel.Location = new Point(220, 10);
                                descripcionLabel.AutoSize = true;

                                Label autorLabel = new Label();
                                autorLabel.Text = $"Autor: {autor}";
                                autorLabel.Location = new Point(220, 50);
                                autorLabel.AutoSize = true;

                                Label estadoLabel = new Label();
                                estadoLabel.Text = $"Estado: {estado}";
                                estadoLabel.Location = new Point(220, 70);
                                estadoLabel.AutoSize = true;

                                Label fechaPublicacionLabel = new Label();
                                fechaPublicacionLabel.Text = $"Fecha de publicación: {fechaPublicacion.ToShortDateString()}";
                                fechaPublicacionLabel.Location = new Point(220, 90);
                                fechaPublicacionLabel.AutoSize = true;

                                Label generoLabel = new Label();
                                generoLabel.Text = $"Género: {genero}";
                                generoLabel.Location = new Point(220, 110);
                                generoLabel.AutoSize = true;

                                detallesForm.Controls.Add(portadaPictureBox);
                                detallesForm.Controls.Add(descripcionLabel);
                                detallesForm.Controls.Add(autorLabel);
                                detallesForm.Controls.Add(estadoLabel);
                                detallesForm.Controls.Add(fechaPublicacionLabel);
                                detallesForm.Controls.Add(generoLabel);

                                detallesForm.Show();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los detalles del manga: " + ex.Message);
            }
        }
    }
}
