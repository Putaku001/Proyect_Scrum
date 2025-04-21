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

namespace ProyectScrum.Forms
{
    public partial class favs : Form
    {
        private Dictionary<string, Image> cacheImagenes = new Dictionary<string, Image>();
        private int UsuarioID = CapturedData.UsuarioID;
        // <-- Añadido
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
                SELECT M.MangaID, M.Titulo, M.URLPortada, M.GeneroID
                FROM Favoritos F
                JOIN Mangas M ON F.MangaID = M.MangaID
                WHERE F.UsuarioID = @UsuarioID";  // <-- Filtrado por usuario

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioID", UsuarioID);  // <-- Pasar el ID

                SqlDataReader reader = cmd.ExecuteReader();

                List<Manga> mangasFavoritos = new List<Manga>();

                while (reader.Read())
                {
                    mangasFavoritos.Add(new Manga
                    {
                        MangaID = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        URLPortada = reader.GetString(2),
                        GeneroID = reader.GetInt32(3)
                    });
                }

                foreach (var manga in mangasFavoritos)
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
                            {
                                cacheImagenes[manga.URLPortada] = pb.Image;
                            }
                            else
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
                    }

                    ToolTip tip = new ToolTip();
                    tip.SetToolTip(pb, manga.Titulo);

                    pb.Click += (s, e) =>
                    {
                        string titulo = (string)((PictureBox)s).Tag;
                        Manga m = ObtenerMangaPorTitulo(titulo);

                        if (m != null)
                        {
                            string genero = ObtenerGenero(m.GeneroID);
                            mangaForm mangaForm = new mangaForm(UsuarioID);
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
            }
        }

        private Manga ObtenerMangaPorTitulo(string titulo)
        {
            Manga manga = null;
            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT MangaID, Titulo, URLPortada, GeneroID FROM Mangas WHERE Titulo = @titulo";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@titulo", titulo);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    manga = new Manga
                    {
                        MangaID = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        URLPortada = reader.GetString(2),
                        GeneroID = reader.GetInt32(3)
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
