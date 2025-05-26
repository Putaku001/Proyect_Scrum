using ProyectScrum.Data;
using ProyectScrum.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;


namespace ProyectScrum.Forms
{
    public partial class favs : Form
    {
        private Dictionary<string, Image> cacheImagenes = new Dictionary<string, Image>();
        private int UsuarioID = CapturedData.UsuarioID;
        public favs(int usuarioID)
        {
            InitializeComponent();
            UsuarioID = usuarioID;
            CargarFavoritos();

        }

        public void CargarFavoritos()
        {
            flowLayoutPanel1.Controls.Clear();

            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = @"
                SELECT M.MangaID, M.Titulo, M.URLPortada, M.GeneroID, M.URLMangaDrive
                FROM Favoritos F
                JOIN Mangas M ON F.MangaID = M.MangaID
                WHERE F.UsuarioID = @UsuarioID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioID", UsuarioID);

                SqlDataReader reader = cmd.ExecuteReader();

                List<Manga> mangasFavoritos = new List<Manga>();

                while (reader.Read())
                {
                    mangasFavoritos.Add(new Manga
                    {
                        MangaID = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        URLPortada = reader.GetString(2),
                        GeneroID = reader.GetInt32(3),
                        URLMangaDrive = reader.GetString(4)
                    });
                }

                foreach (var manga in mangasFavoritos)
                {
                    var tarjeta = new Guna2Panel
                    {
                        Width = 185,
                        Height = 270,
                        BorderRadius = 15,
                        FillColor = Color.FromArgb(33, 37, 50), // Fondo oscuro para que combine
                        BorderColor = Color.FromArgb(2, 5, 20),
                        BorderThickness = 1,
                        ShadowDecoration = { Enabled = true, Depth = 6 },
                        Margin = new Padding(20),
                        Cursor = Cursors.Hand
                    };

                    var pb = new Guna2PictureBox
                    {
                        Width = 170,
                        Height = 220,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Location = new Point(7, 7),
                        BorderRadius = 10,
                        Tag = manga.Titulo,
                        Image = Properties.Resources.LoadingGif
                    };

                    pb.LoadCompleted += (s, e) =>
                    {
                        if (e.Error != null)
                            pb.Image = Properties.Resources.DefaultCover;
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

                        try { pb.LoadAsync(manga.URLPortada); }
                        catch { pb.Image = Properties.Resources.DefaultCover; }
                    }

                    ToolTip tip = new ToolTip();
                    tip.SetToolTip(pb, manga.Titulo);

                    pb.Click += (s, e) =>
                    {
                        string titulo = (string)((Guna2PictureBox)s).Tag;
                        Manga m = ObtenerMangaPorTitulo(titulo);

                        if (m != null)
                        {
                            string genero = ObtenerGenero(m.GeneroID);
                            mangaForm mangaForm = new mangaForm(UsuarioID);
                            mangaForm.CargarManga(m, genero);

                            if (this.TopLevelControl is Main main)
                            {
                                main.catalogForm = null;
                                main.favsForm = this;
                                main.AbrirFormularioEnPanel(mangaForm);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se pudo cargar el manga.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

                    // Botón moderno "Eliminar de 💔"
                    var btnEliminar = new Guna2Button
                    {
                        Text = "Eliminar de 💔",
                        Width = 160,
                        Height = 30,
                        BorderRadius = 10,
                        FillColor = Color.FromArgb(255, 69, 96),
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        Location = new Point(12, 235),
                        Tag = manga.MangaID
                    };

                    btnEliminar.Click += (s, e) =>
                    {
                        int mangaIdEliminar = (int)((Guna2Button)s).Tag;

                        SqlDataAccess dbEliminar = new SqlDataAccess();
                        using (SqlConnection connEliminar = dbEliminar.GetConnection())
                        {
                            connEliminar.Open();
                            string deleteQuery = "DELETE FROM Favoritos WHERE MangaID = @MangaID AND UsuarioID = @UsuarioID";

                            SqlCommand cmdEliminar = new SqlCommand(deleteQuery, connEliminar);
                            cmdEliminar.Parameters.AddWithValue("@MangaID", mangaIdEliminar);
                            cmdEliminar.Parameters.AddWithValue("@UsuarioID", UsuarioID);

                            int result = cmdEliminar.ExecuteNonQuery();
                            if (result > 0)
                                CargarFavoritos();
                            else
                                MessageBox.Show("No se pudo eliminar.");
                        }
                    };

                    tarjeta.Controls.Add(pb);
                    tarjeta.Controls.Add(btnEliminar);
                    flowLayoutPanel1.Controls.Add(tarjeta);
                }


            }
        }


        private Manga ObtenerMangaPorTitulo(string titulo)
        {
            Manga manga = null;
            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = @"
                        SELECT MangaID, Titulo, Autor, Descripcion, Estado, FechaPublicacion,
                               URLMangaDrive, URLPortada, GeneroID
                        FROM Mangas
                        WHERE Titulo = @titulo";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@titulo", titulo);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    manga = new Manga
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

            return manga;
        }


        private string ObtenerGenero(int generoID)
        {
            string nombreGenero = "";
            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT Nombre FROM Generos WHERE GeneroID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", generoID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    nombreGenero = reader.GetString(0);
                }
            }

            return nombreGenero;
        }
    }
}


