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
        private Dictionary<string, Image> cacheImagenes = new Dictionary<string, Image>();
        public EmailSettings _emailSettings;
        private int paginaActual = 1;
        private int mangasPorPagina = 12;

        public Catalog(EmailSettings emailSettings)
        {
            InitializeComponent();
            _emailSettings = emailSettings;
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            Main mainForm = new Main( _emailSettings);
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
            SqlDataAccess db = new SqlDataAccess();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Mangas", conn);
                return (int)cmd.ExecuteScalar();
            }
        }

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

                if (cacheImagenes.ContainsKey(manga.URLPortada))
                {
                    pb.Image = cacheImagenes[manga.URLPortada];
                }
                else
                {
                    pb.LoadCompleted += (s, e) =>
                    {
                        if (e.Error == null && pb.Image != null)
                            cacheImagenes[manga.URLPortada] = pb.Image;
                        else
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
                }

                pb.Click += (s, e) =>
                {
                    string titulo = (string)((PictureBox)s).Tag;
                    Manga m = ObtenerMangaPorTitulo(titulo);

                    if (m != null)
                    {
                        string genero = ObtenerGenero(m.GeneroID);
                        mangaForm mangaForm = new mangaForm(CapturedData.UsuarioID);
                        mangaForm.CargarManga(m, genero);

                        if (this.TopLevelControl is Main main)
                        {
                            main.AbrirFormularioEnPanel(mangaForm);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se pudo cargar el manga.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                flowLayoutPanel1.Controls.Add(pb);
            }

            int totalMangas = ObtenerTotalMangas();
            int totalPaginas = (int)Math.Ceiling((double)totalMangas / mangasPorPagina);
            anteriorButton.Visible = paginaActual > 1;
            siguienteButton.Visible = paginaActual < totalPaginas;
        }

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

        public void ForzarRedibujarLayout()
        {
            flowLayoutPanel1.PerformLayout();
            flowLayoutPanel1.Refresh();
        }

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

        private void btnAplicarFiltro_Click(object sender, EventArgs e)
        {
            List<int> generosSeleccionados = new List<int>();

            foreach (CheckBox chk in flowCheckBoxGeneros.Controls)
                if (chk.Checked)
                    generosSeleccionados.Add((int)chk.Tag);

            MostrarPortadasFiltradas(generosSeleccionados);
            panelFiltro.Visible = false;
        }

        private void MostrarPortadasFiltradas(List<int> generosSeleccionados)
        {
            if (generosSeleccionados.Count == 0)
            {
                MostrarPortadas();
                return;
            }

            flowLayoutPanel1.Controls.Clear();
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
                    Tag = manga.Titulo
                };

                pb.Image = Properties.Resources.LoadingGif;

                if (cacheImagenes.ContainsKey(manga.URLPortada))
                {
                    pb.Image = cacheImagenes[manga.URLPortada];
                }
                else
                {
                    pb.LoadCompleted += (s, e) =>
                    {
                        if (e.Error == null && pb.Image != null)
                            cacheImagenes[manga.URLPortada] = pb.Image;
                        else
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
                }

                pb.Click += (s, e) =>
                {
                    string titulo = (string)((PictureBox)s).Tag;
                    Manga m = ObtenerMangaPorTitulo(titulo);
                    if (m != null)
                    {
                        string genero = ObtenerGenero(m.GeneroID);
                        mangaForm form = new mangaForm(CapturedData.UsuarioID);
                        form.CargarManga(m, genero);

                        if (this.TopLevelControl is Main main)
                            main.AbrirFormularioEnPanel(form);
                    }
                };

                flowLayoutPanel1.Controls.Add(pb);
            }

            anteriorButton.Visible = false;
            siguienteButton.Visible = false;
        }

        private Manga ObtenerMangaPorTitulo(string titulo)
        {
            SqlDataAccess db = new SqlDataAccess();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = @"SELECT MangaID, Titulo, Autor, Descripcion, Estado, FechaPublicacion, URLMangaDrive, URLPortada, GeneroID
                                 FROM Mangas WHERE Titulo = @Titulo";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Titulo", titulo);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Manga
                    {
                        MangaID = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        Autor = reader.GetString(2),
                        Descripcion = reader.GetString(3),
                        Estado = reader.GetString(4),
                        FechaPublicacion = reader.GetDateTime(5),
                        URLMangaDrive = reader.GetString(6),
                        URLPortada = reader.GetString(7),
                        GeneroID = reader.GetInt32(8)
                    };
                }
            }

            return null;
        }

        private string ObtenerGenero(int generoId)
        {
            SqlDataAccess db = new SqlDataAccess();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Nombre FROM Generos WHERE GeneroID = @GeneroID", conn);
                cmd.Parameters.AddWithValue("@GeneroID", generoId);

                object result = cmd.ExecuteScalar();
                return result?.ToString() ?? "Sin género";
            }
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            panelFiltro.Visible = !panelFiltro.Visible;
            panelFiltro.BringToFront();
        }

        private void Catalog_MouseDown(object sender, MouseEventArgs e)
        {
            if (panelFiltro.Visible)
            {
                Point mousePos = this.PointToClient(Cursor.Position);
                if (!panelFiltro.Bounds.Contains(mousePos))
                {
                    panelFiltro.Visible = false;
                }
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string texto = txtBuscar.Text.Trim();

            if (string.IsNullOrEmpty(texto))
                MostrarPortadas();
            else
                MostrarPortadasBusqueda(texto);
        }

        private void MostrarPortadasBusqueda(string texto)
        {
            flowLayoutPanel1.Controls.Clear();

            List<Manga> mangas = new List<Manga>();
            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = @"SELECT m.MangaID, m.Titulo, m.URLPortada 
                                 FROM Mangas m
                                 LEFT JOIN TitulosAlternativos t ON m.MangaID = t.MangaID
                                 WHERE m.Titulo LIKE @texto OR t.TituloAlternativo LIKE @texto
                                 GROUP BY m.MangaID, m.Titulo, m.URLPortada
                                 ORDER BY MAX(m.FechaPublicacion) DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@texto", "%" + texto + "%");

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

            foreach (var manga in mangas)
            {
                PictureBox pb = new PictureBox
                {
                    Width = 185,
                    Height = 240,
                    Margin = new Padding(30, 20, 30, 20),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Cursor = Cursors.Hand,
                    Tag = manga.Titulo,
                    Image = Properties.Resources.LoadingGif
                };

                if (cacheImagenes.ContainsKey(manga.URLPortada))
                {
                    pb.Image = cacheImagenes[manga.URLPortada];
                }
                else
                {
                    pb.LoadCompleted += (s, e) =>
                    {
                        if (e.Error == null && pb.Image != null)
                            cacheImagenes[manga.URLPortada] = pb.Image;
                        else
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
                }

                pb.Click += (s, e) =>
                {
                    string titulo = (string)((PictureBox)s).Tag;
                    Manga m = ObtenerMangaPorTitulo(titulo);

                    if (m != null)
                    {
                        string genero = ObtenerGenero(m.GeneroID);
                        mangaForm form = new mangaForm(CapturedData.UsuarioID);
                        form.CargarManga(m, genero);

                        if (this.TopLevelControl is Main main)
                        {
                            main.AbrirFormularioEnPanel(form);
                        }
                    }
                };

                flowLayoutPanel1.Controls.Add(pb);
            }
        }
    }
}
