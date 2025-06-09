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
        private Guna.UI2.WinForms.Guna2Panel panelFiltro;
        private FlowLayoutPanel flowCheckBoxGeneros;

        public Catalog(EmailSettings emailSettings)
        {
            InitializeComponent();
            _emailSettings = emailSettings;
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            Main mainForm = new Main(_emailSettings);
            mainForm.Show();
            this.Close();
        }

        private void Catalog_Load(object sender, EventArgs e)
        {
            InicializarPanelFiltro();
            CargarCheckBoxGeneros();
            MostrarPortadas();

            anteriorButton.Visible = false;
            siguienteButton.Visible = false;

            flowCheckBoxGeneros.FlowDirection = FlowDirection.TopDown;
            flowCheckBoxGeneros.WrapContents = false;


        }
        private void InicializarPanelFiltro()
        {
            // Crear panel principal
            panelFiltro = new Guna.UI2.WinForms.Guna2Panel
            {
                Name = "panelFiltro",
                Size = new Size(220, 400),
                BorderRadius = 10,
                BorderThickness = 2,
                BorderColor = Color.White,
                BackColor = Color.FromArgb(30, 30, 30),
                Visible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(this.ClientSize.Width - 240, 60),
                ShadowDecoration = { Enabled = true }
            };

            // FlowLayoutPanel para los géneros
            flowCheckBoxGeneros = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            // Botón aplicar filtro
            var btnAplicar = new Guna.UI2.WinForms.Guna2Button
            {
                Text = "Aplicar",
                Dock = DockStyle.Bottom,
                FillColor = Color.DimGray,
                ForeColor = Color.White,
                Height = 40,
                BorderRadius = 5
            };
            btnAplicar.Click += btnAplicarFiltro_Click;

            // Agregar controles al panel
            panelFiltro.Controls.Add(flowCheckBoxGeneros);
            panelFiltro.Controls.Add(btnAplicar);

            // Agregar panel al formulario
            this.Controls.Add(panelFiltro);
            panelFiltro.BringToFront();

        }

        private Manga ObtenerMangaAleatorio()
        {
            SqlDataAccess db = new SqlDataAccess();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                // Usamos NEWID() para ordenar aleatoriamente en SQL Server
                string query = @"SELECT TOP 1 MangaID, Titulo, Autor, Descripcion, Estado, 
                        FechaPublicacion, URLMangaDrive, URLPortada, GeneroID
                        FROM Mangas
                        ORDER BY NEWID()";

                SqlCommand cmd = new SqlCommand(query, conn);
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
                var contenedor = new Guna.UI2.WinForms.Guna2Panel
                {
                    Width = 200,
                    Height = 290,
                    BorderRadius = 10,
                    BorderThickness = 1,
                    BorderColor = Color.FromArgb(60, 60, 60),
                    ShadowDecoration = { Enabled = true },
                    BackColor = Color.FromArgb(40, 40, 40),
                    Margin = new Padding(20, 10, 20, 10),
                    Cursor = Cursors.Hand,
                    Tag = manga.Titulo
                };

                var portada = new PictureBox
                {
                    Width = 185,
                    Height = 240,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = Properties.Resources.LoadingGif,
                    Location = new Point(7, 7),
                    Tag = manga.Titulo,
                    Cursor = Cursors.Hand
                };

                if (cacheImagenes.ContainsKey(manga.URLPortada))
                {
                    portada.Image = cacheImagenes[manga.URLPortada];
                }
                else
                {
                    portada.LoadCompleted += (s, e) =>
                    {
                        if (e.Error == null && portada.Image != null)
                            cacheImagenes[manga.URLPortada] = portada.Image;
                        else
                            portada.Image = Properties.Resources.DefaultCover;
                    };

                    try
                    {
                        portada.LoadAsync(manga.URLPortada);
                    }
                    catch
                    {
                        portada.Image = Properties.Resources.DefaultCover;
                    }
                }

                // Evento al hacer clic en la portada o el contenedor
                EventHandler abrirManga = (s, e) =>
                {
                    if (!UsuarioTieneSuscripcionActiva(CapturedData.UsuarioID))
                    {
                        MessageBox.Show("Necesitas una suscripción activa para leer este manga.", "Suscripción requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string titulo = (string)((Control)s).Tag;
                    Manga m = ObtenerMangaPorTitulo(titulo);

                    if (m != null)
                    {
                        string genero = ObtenerGenero(m.GeneroID);
                        mangaForm form = new mangaForm(CapturedData.UsuarioID);
                        form.CargarManga(m, genero);

                        if (this.TopLevelControl is Main main)
                            main.AbrirFormularioEnPanel(form);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo cargar el manga.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                contenedor.Click += abrirManga;
                portada.Click += abrirManga;

                var lblTitulo = new Label
                {
                    Text = manga.Titulo,
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    AutoSize = false,
                    Width = contenedor.Width,
                    Height = 40,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, portada.Bottom + 5)
                };

                // Click también en el label
                lblTitulo.Click += abrirManga;
                lblTitulo.AutoEllipsis = true;
                contenedor.Controls.Add(portada);
                contenedor.Controls.Add(lblTitulo);
                flowLayoutPanel1.Controls.Add(contenedor);
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
                var contenedor = new Guna.UI2.WinForms.Guna2Panel
                {
                    Width = 200,
                    Height = 290,
                    BorderRadius = 10,
                    BorderThickness = 1,
                    BorderColor = Color.FromArgb(60, 60, 60),
                    ShadowDecoration = { Enabled = true },
                    BackColor = Color.FromArgb(40, 40, 40),
                    Margin = new Padding(20, 10, 20, 10),
                    Cursor = Cursors.Hand,
                    Tag = manga.Titulo
                };

                var portada = new PictureBox
                {
                    Width = 185,
                    Height = 240,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = Properties.Resources.LoadingGif,
                    Location = new Point(7, 7),
                    Tag = manga.Titulo,
                    Cursor = Cursors.Hand
                };

                if (cacheImagenes.ContainsKey(manga.URLPortada))
                {
                    portada.Image = cacheImagenes[manga.URLPortada];
                }
                else
                {
                    portada.LoadCompleted += (s, e) =>
                    {
                        if (e.Error == null && portada.Image != null)
                            cacheImagenes[manga.URLPortada] = portada.Image;
                        else
                            portada.Image = Properties.Resources.DefaultCover;
                    };

                    try
                    {
                        portada.LoadAsync(manga.URLPortada);
                    }
                    catch
                    {
                        portada.Image = Properties.Resources.DefaultCover;
                    }
                }

                EventHandler abrirManga = (s, e) =>
                {
                    if (!UsuarioTieneSuscripcionActiva(CapturedData.UsuarioID))
                    {
                        MessageBox.Show("Necesitas una suscripción activa para leer este manga.", "Suscripción requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string titulo = (string)((Control)s).Tag;
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

                var lblTitulo = new Label
                {
                    Text = manga.Titulo,
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    AutoSize = false,
                    Width = contenedor.Width,
                    Height = 40,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, portada.Bottom + 5),
                    AutoEllipsis = true
                };

                contenedor.Click += abrirManga;
                portada.Click += abrirManga;
                lblTitulo.Click += abrirManga;

                contenedor.Controls.Add(portada);
                contenedor.Controls.Add(lblTitulo);
                flowLayoutPanel1.Controls.Add(contenedor);
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
                var contenedor = new Guna.UI2.WinForms.Guna2Panel
                {
                    Width = 200,
                    Height = 290,
                    BorderRadius = 10,
                    BorderThickness = 1,
                    BorderColor = Color.FromArgb(60, 60, 60),
                    ShadowDecoration = { Enabled = true },
                    BackColor = Color.FromArgb(40, 40, 40),
                    Margin = new Padding(20, 10, 20, 10),
                    Cursor = Cursors.Hand,
                    Tag = manga.Titulo
                };

                var portada = new PictureBox
                {
                    Width = 185,
                    Height = 240,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = Properties.Resources.LoadingGif,
                    Location = new Point(7, 7),
                    Tag = manga.Titulo,
                    Cursor = Cursors.Hand
                };

                if (cacheImagenes.ContainsKey(manga.URLPortada))
                {
                    portada.Image = cacheImagenes[manga.URLPortada];
                }
                else
                {
                    portada.LoadCompleted += (s, e) =>
                    {
                        if (e.Error == null && portada.Image != null)
                            cacheImagenes[manga.URLPortada] = portada.Image;
                        else
                            portada.Image = Properties.Resources.DefaultCover;
                    };

                    try
                    {
                        portada.LoadAsync(manga.URLPortada);
                    }
                    catch
                    {
                        portada.Image = Properties.Resources.DefaultCover;
                    }
                }

                EventHandler abrirManga = (s, e) =>
                {
                    if (!UsuarioTieneSuscripcionActiva(CapturedData.UsuarioID))
                    {
                        MessageBox.Show("Necesitas una suscripción activa para leer este manga.", "Suscripción requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string titulo = (string)((Control)s).Tag;
                    Manga m = ObtenerMangaPorTitulo(titulo);

                    if (m != null)
                    {
                        string genero = ObtenerGenero(m.GeneroID);
                        mangaForm form = new mangaForm(CapturedData.UsuarioID);
                        form.CargarManga(m, genero);

                        if (this.TopLevelControl is Main main)
                            main.AbrirFormularioEnPanel(form);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo cargar el manga.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                var lblTitulo = new Label
                {
                    Text = manga.Titulo,
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    AutoSize = false,
                    Width = contenedor.Width,
                    Height = 40,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(0, portada.Bottom + 5),
                    AutoEllipsis = true
                };

                contenedor.Click += abrirManga;
                portada.Click += abrirManga;
                lblTitulo.Click += abrirManga;

                contenedor.Controls.Add(portada);
                contenedor.Controls.Add(lblTitulo);
                flowLayoutPanel1.Controls.Add(contenedor);
            }
        }

        private void btnSorprendeme_Click_1(object sender, EventArgs e)
        {
            Manga mangaAleatorio = ObtenerMangaAleatorio();

            if (mangaAleatorio != null)
            {
                string genero = ObtenerGenero(mangaAleatorio.GeneroID);
                mangaForm form = new mangaForm(CapturedData.UsuarioID);
                form.CargarManga(mangaAleatorio, genero);

                if (this.TopLevelControl is Main main)
                    main.AbrirFormularioEnPanel(form);
            }
            else
            {
                MessageBox.Show("No se pudo encontrar un manga aleatorio.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public List<string> ObtenerTitulosAlternativos(int mangaId)
        {
            List<string> titulos = new List<string>();
            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = @"SELECT TituloAlternativo FROM TitulosAlternativos WHERE MangaID = @MangaID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MangaID", mangaId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    titulos.Add(reader.GetString(0));
                }
            }

            return titulos;
        }

        private bool UsuarioTieneSuscripcionActiva(int usuarioId)
        {
            SqlDataAccess db = new SqlDataAccess();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                // Verifica si tiene suscripción activa o es premium
                string query = @"
                    SELECT 
                        CASE 
                            WHEN EXISTS (
                                SELECT 1 FROM Suscripciones 
                                WHERE UsuarioID = @UsuarioID AND CONVERT(date, FechaFin) >= CONVERT(date, GETDATE())
                            ) THEN 1
                            WHEN EXISTS (
                                SELECT 1 FROM Usuarios 
                                WHERE UsuarioID = @UsuarioID AND EsPremium = 1
                            ) THEN 1
                            ELSE 0
                        END";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioID", usuarioId);
                int result = (int)cmd.ExecuteScalar();
                return result == 1;
            }
        }
    }
}
