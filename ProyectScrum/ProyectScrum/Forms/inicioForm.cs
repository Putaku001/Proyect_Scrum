using ProyectScrum.Data;
using ProyectScrum.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProyectScrum.Forms
{
    public partial class inicioForm : Form
    {
        // ───────── Variables de instancia ─────────
        private readonly Dictionary<string, Image> _cacheImagenes = new();
        private const int AlturaSeccion = 265;          // alto de cada fila de portadas
        private const int AnchoPortada = 180;
        private const int AltoPortada = 240;
        private const int MaxAleatorios = 20;

        public inicioForm()
        {
            InitializeComponent();
            flowLayoutPanelMangas.SizeChanged += (s, e) => AjustarFilas();
        }
        private void AjustarFilas()
        {
            // ancho útil = ancho client - scrollbar vertical (si aparece) - 1 px de margen
            int anchoUtil = flowLayoutPanelMangas.ClientSize.Width;
            if (flowLayoutPanelMangas.VerticalScroll.Visible)
                anchoUtil -= SystemInformation.VerticalScrollBarWidth;

            foreach (Control ctr in flowLayoutPanelMangas.Controls)
            {
                if (ctr is Label || ctr is FlowLayoutPanel)
                    ctr.Width = anchoUtil - 5;   // 5 px para que no toque el borde
                RedimensionarPortadas();
            }
        }
        private void RedimensionarPortadas()
        {
            // 1) ancho útil de una fila cualquiera (todas tienen el mismo)
            if (flowLayoutPanelMangas.Controls.OfType<FlowLayoutPanel>().FirstOrDefault() is not FlowLayoutPanel filaEjemplo)
                return;

            int anchoFila = filaEjemplo.ClientSize.Width;

            // 2) decide cuántas portadas quieres por fila según el ancho
            int portadasPorFila = anchoFila < 800 ? 4 : 5;   // puedes ajustar

            // 3) márgenes laterales por PictureBox (= Padding.Left + Padding.Right del Flow + Margin.Left + Margin.Right)
            const int margenPB = 10 * 2; // 10 izq + 10 der (por tu 'Margin = new Padding(10)')
            int anchoDisponible = anchoFila - margenPB * portadasPorFila;
            if (anchoDisponible <= 0) return;

            int nuevoAncho = anchoDisponible / portadasPorFila;
            int nuevoAlto = (int)(nuevoAncho * 4f / 3f);    // relación 4:3

            // 4) aplica a todas las filas
            foreach (var fila in flowLayoutPanelMangas.Controls.OfType<FlowLayoutPanel>())
            {
                fila.SuspendLayout();
                foreach (PictureBox pb in fila.Controls.OfType<PictureBox>())
                {
                    pb.Width = nuevoAncho;
                    pb.Height = nuevoAlto;
                }
                fila.ResumeLayout();
                // Ajusta la altura de la fila (alto portada + margen superior e inferior + scrollbar)
                fila.Height = nuevoAlto + 20 + SystemInformation.HorizontalScrollBarHeight;
            }
        }
        // ───────── CARGA INICIAL ─────────
        private void inicioForm_Load(object sender, EventArgs e)
        {
            lblBienvenida.Text = $"¡Bienvenido, {CapturedData.NombreUsuario}! 👋";
            CargarSecciones();
        }

        // ───────── SECCIONES PRINCIPALES ─────────
        private void CargarSecciones()
        {
            flowLayoutPanelMangas.SuspendLayout();
            flowLayoutPanelMangas.Controls.Clear();
            flowLayoutPanelMangas.AutoScroll = true;
            flowLayoutPanelMangas.WrapContents = false;
            flowLayoutPanelMangas.FlowDirection = FlowDirection.TopDown;

            // 1) Recomendaciones aleatorias
            var aleatorios = ObtenerMangasAleatorios(MaxAleatorios);
            AgregarSeccion("Nuestras recomendaciones", aleatorios);

            // 2) Seguir leyendo
            var leyendo = ObtenerMangasConProgreso(CapturedData.UsuarioID);
            if (leyendo.Count > 0)
                AgregarSeccion("Seguir leyendo", leyendo);

            // 3) Recomendación por género favorito
            (int generoFavID, string generoFavNombre) = ObtenerGeneroFavorito(CapturedData.UsuarioID);
            if (generoFavID > 0)
            {
                var favoritosPorGenero = ObtenerMangasPorGenero(generoFavID);
                AgregarSeccion($"Porque te gusta el género {generoFavNombre}", favoritosPorGenero);
            }

            flowLayoutPanelMangas.ResumeLayout();
            AjustarFilas();
        }

        private void AgregarSeccion(string titulo, List<Manga> mangas)
        {
            // -------- 1) Encabezado  --------
            var lbl = new Label
            {
                Text = titulo,
                AutoSize = false,
                Height = 28,
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Padding = new Padding(3, 3, 0, 0)
            };
            flowLayoutPanelMangas.Controls.Add(lbl);

            // -------- 2) Fila horizontal --------
            const int margen = 10;                         // el mismo que usas en el PictureBox
            int altoContenido = AltoPortada + margen * 2;   // 240 + 20 = 260
            int altoScrollbar = SystemInformation.HorizontalScrollBarHeight;   // ≈17
            int altoFila = altoContenido + altoScrollbar;                 // 277-ish

            var fila = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true,
                Height = altoFila,
                Width = flowLayoutPanelMangas.Width - 25,
                Margin = new Padding(0)
            };

            // Desactiva la barra vertical dentro de la fila
            fila.VerticalScroll.Enabled = false;
            fila.VerticalScroll.Visible = false;
            fila.VerticalScroll.Maximum = 0;

            foreach (var m in mangas)
                fila.Controls.Add(CrearPictureBox(m));

            flowLayoutPanelMangas.Controls.Add(fila);
        }



        private PictureBox CrearPictureBox(Manga m)
        {
            var pb = new PictureBox
            {
                Width = AnchoPortada,
                Height = AltoPortada,
                SizeMode = PictureBoxSizeMode.Zoom,
                Cursor = Cursors.Hand,
                Margin = new Padding(10),
                Tag = m.MangaID,
                Image = Properties.Resources.LoadingGif
            };

            if (_cacheImagenes.TryGetValue(m.URLPortada, out var img))
            {
                pb.Image = img;
            }
            else
            {
                pb.LoadCompleted += (s, ev) =>
                {
                    pb.Image ??= Properties.Resources.DefaultCover;
                    if (ev.Error == null && pb.Image != Properties.Resources.DefaultCover)
                        _cacheImagenes[m.URLPortada] = pb.Image;
                };
                try { pb.LoadAsync(m.URLPortada); }
                catch { pb.Image = Properties.Resources.DefaultCover; }
            }

            pb.Click += Pb_Click;
            return pb;
        }

        // ───────── EVENTO AL HACER CLICK EN UNA PORTADA ─────────
        private void Pb_Click(object sender, EventArgs e)
        {
            int mangaID = (int)((PictureBox)sender).Tag;
            Manga m = ObtenerMangaPorId(mangaID);
            if (m == null) return;

            string generoNombre = ObtenerGenero(m.GeneroID);

            var mf = new mangaForm(CapturedData.UsuarioID);
            mf.CargarManga(m, generoNombre);

            if (Application.OpenForms["Main"] is Main main)
                main.AbrirFormularioEnPanel(mf);
        }

        // ───────── CONSULTAS A BASE DE DATOS ─────────
        private List<Manga> ObtenerMangasAleatorios(int limite)
        {
            var list = new List<Manga>();
            using var conn = new SqlDataAccess().GetConnection();
            conn.Open();

            string q = $"""
                         SELECT TOP ({limite}) MangaID, Titulo, URLPortada, GeneroID
                         FROM Mangas
                         ORDER BY NEWID();
                         """;

            using var cmd = new SqlCommand(q, conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Manga
                {
                    MangaID = r.GetInt32(0),
                    Titulo = r.GetString(1),
                    URLPortada = r.GetString(2),
                    GeneroID = r.GetInt32(3)
                });
            }
            return list;
        }

        private List<Manga> ObtenerMangasConProgreso(int usuarioId)
        {
            var list = new List<Manga>();
            using var conn = new SqlDataAccess().GetConnection();
            conn.Open();

            string q = """
                       SELECT M.MangaID, M.Titulo, M.URLPortada, M.GeneroID
                       FROM ProgresoLectura P
                       JOIN Mangas M ON P.MangaID = M.MangaID
                       WHERE P.UsuarioID = @uid
                       ORDER BY P.FechaUltimaLectura DESC;
                       """;
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@uid", usuarioId);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Manga
                {
                    MangaID = r.GetInt32(0),
                    Titulo = r.GetString(1),
                    URLPortada = r.GetString(2),
                    GeneroID = r.GetInt32(3)
                });
            }
            return list;
        }

        private (int generoID, string generoNombre) ObtenerGeneroFavorito(int usuarioId)
        {
            using var conn = new SqlDataAccess().GetConnection();
            conn.Open();

            string q = """
                       SELECT TOP 1 M.GeneroID, G.Nombre, COUNT(*) AS Total
                       FROM Favoritos F
                       JOIN Mangas    M ON F.MangaID = M.MangaID
                       JOIN Generos   G ON M.GeneroID = G.GeneroID
                       WHERE F.UsuarioID = @uid
                       GROUP BY M.GeneroID, G.Nombre
                       ORDER BY Total DESC;
                       """;

            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@uid", usuarioId);
            using var r = cmd.ExecuteReader();
            if (r.Read())
                return (r.GetInt32(0), r.GetString(1));

            return (0, string.Empty);   // sin favoritos
        }

        private List<Manga> ObtenerMangasPorGenero(int generoId)
        {
            var list = new List<Manga>();
            using var conn = new SqlDataAccess().GetConnection();
            conn.Open();

            string q = """
                       SELECT MangaID, Titulo, URLPortada, GeneroID
                       FROM Mangas
                       WHERE GeneroID = @gid
                       ORDER BY FechaPublicacion DESC;
                       """;
            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@gid", generoId);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Manga
                {
                    MangaID = r.GetInt32(0),
                    Titulo = r.GetString(1),
                    URLPortada = r.GetString(2),
                    GeneroID = r.GetInt32(3)
                });
            }
            return list;
        }

        private Manga ObtenerMangaPorId(int id)
        {
            using var conn = new SqlDataAccess().GetConnection();
            conn.Open();

            const string q = """
                             SELECT MangaID, Titulo, Autor, Descripcion, Estado,
                                    FechaPublicacion, URLMangaDrive, URLPortada, GeneroID
                             FROM Mangas
                             WHERE MangaID = @id;
                             """;

            using var cmd = new SqlCommand(q, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var r = cmd.ExecuteReader();
            if (r.Read())
            {
                return new Manga
                {
                    MangaID = r.GetInt32(0),
                    Titulo = r.GetString(1),
                    Autor = r.GetString(2),
                    Descripcion = r.GetString(3),
                    Estado = r.GetString(4),
                    FechaPublicacion = r.GetDateTime(5),
                    URLMangaDrive = r.GetString(6),
                    URLPortada = r.GetString(7),
                    GeneroID = r.GetInt32(8)
                };
            }
            return null;
        }

        private string ObtenerGenero(int generoID)
        {
            using var conn = new SqlDataAccess().GetConnection();
            conn.Open();
            using var cmd = new SqlCommand("SELECT Nombre FROM Generos WHERE GeneroID=@g", conn);
            cmd.Parameters.AddWithValue("@g", generoID);
            return cmd.ExecuteScalar()?.ToString() ?? "Sin género";
        }
    }



}