using ProyectScrum.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using File = Google.Apis.Drive.v3.Data.File;
using ProyectScrum.Data;
using System.Data.SqlClient;

namespace ProyectScrum.Forms
{
    public partial class mangaForm : Form
    {

        private Panel panelCargandoManga;
        private PictureBox pictureBoxCargandoManga;

        private Dictionary<string, Image> cacheImagenes = new Dictionary<string, Image>();
        private Manga mangaActual;
        int usuarioID = CapturedData.UsuarioID;
        public event EventHandler FavoritoAgregado;
        private visorForm visorActual = null;

        public mangaForm(int usuarioID)
        {
            InitializeComponent();
            btnQuitarFavoritos.Click += btnQuitarFavoritos_Click;

        }
        public void CargarManga(Manga manga, string genero)
        {
            // Mostrar imagen de carga
            mangaActual = manga;
            picturePortada.Image = Properties.Resources.LoadingGif;

            if (cacheImagenes.ContainsKey(manga.URLPortada))
            {
                picturePortada.Image = cacheImagenes[manga.URLPortada];
            }
            else
            {
                try
                {
                    picturePortada.LoadCompleted += (s, e) =>
                    {
                        if (e.Error != null)
                        {
                            picturePortada.Image = Properties.Resources.DefaultCover;
                        }
                        else
                        {
                            // Guardar en cache si se carga bien
                            cacheImagenes[manga.URLPortada] = picturePortada.Image;
                        }
                    };

                    picturePortada.LoadAsync(manga.URLPortada);
                }
                catch
                {
                    picturePortada.Image = Properties.Resources.DefaultCover;
                }
            }

            textTitle.Text = manga.Titulo;
            labelTitulo.Text = manga.Titulo;
            labelAutor.Text = manga.Autor;
            labelFecha.Text = manga.FechaPublicacion.ToShortDateString();
            labelDescripcion.Text = manga.Descripcion;
            labelGenero.Text = genero;
            CargarVolumenes(manga.URLMangaDrive);

            bool enFavoritos = EstaEnFavoritos(manga.MangaID, CapturedData.UsuarioID);
            btnAgregarFavoritos.Visible = !enFavoritos;
            btnQuitarFavoritos.Visible = enFavoritos;


        }
        //extraer carpetas de los links
        private string ExtraerIdCarpeta(string url)
        {
            var partes = url.Split(new string[] { "folders/" }, StringSplitOptions.None);
            if (partes.Length > 1)
            {
                return partes[1].Split('?')[0];
            }
            return string.Empty;
        }
        //carga de volumenes
        private void CargarVolumenes(string urlMangaDrive)
        {
            flowPanelVolumenes.Controls.Clear();
            flowPanelVolumenes.SuspendLayout();
            flowPanelVolumenes.FlowDirection = FlowDirection.TopDown;
            flowPanelVolumenes.WrapContents = false;
            flowPanelVolumenes.AutoScroll = true;

            string folderId = ExtraerIdCarpeta(urlMangaDrive);
            if (string.IsNullOrEmpty(folderId))
            {
                MessageBox.Show("No se pudo obtener el ID de la carpeta de Drive.", "Error");
                return;
            }

            var service = ObtenerServicioDrive();
            if (service == null)
            {
                MessageBox.Show("Error al conectar con Google Drive.", "Error");
                return;
            }

            var files = new List<File>();
            string pageToken = null;

            do
            {
                var request = service.Files.List();
                request.Q = $"'{folderId}' in parents and mimeType='application/pdf'";
                request.Fields = "nextPageToken, files(id, name)";
                request.PageSize = 1000;
                request.PageToken = pageToken;

                var result = request.Execute();
                files.AddRange(result.Files);
                pageToken = result.NextPageToken;
            } while (pageToken != null);

            files = files.OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase).ToList();

            foreach (var file in files)
            {
                Button tomoBtn = new Button
                {
                    Text = $" {file.Name}",
                    Width = flowPanelVolumenes.Width - 40,
                    Height = 45,
                    BackColor = Color.FromArgb(35, 35, 35),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(5),
                    Tag = file.Id,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                tomoBtn.Click += async (s, e) =>
                {
                    if (visorActual != null && !visorActual.IsDisposed)
                    {
                        MessageBox.Show("Ya tienes un visor abierto. Ciérralo antes de abrir otro tomo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string fileId = ((Button)s).Tag.ToString();

                    // Deshabilitar todos los botones mientras carga
                    foreach (Control control in flowPanelVolumenes.Controls)
                        if (control is Button btn) btn.Enabled = false;


                    panelCargandoManga.Visible = true;
                    panelCargandoManga.BringToFront();
                    panelCargandoManga.Refresh();



                    panelCargandoManga.Visible = true;
                    panelCargandoManga.BringToFront();
                    panelCargandoManga.Refresh();

                    try
                    {
                        var request = service.Files.Get(fileId);
                        var stream = new MemoryStream();
                        await request.DownloadAsync(stream);
                        stream.Position = 0;

                        visorActual = new visorForm(stream, this);

                        visorActual.FormClosed += (sender2, args) =>
                        {
                            visorActual = null;

                            foreach (Control control in flowPanelVolumenes.Controls)
                                if (control is Button btn) btn.Enabled = true;
                        };

                        Main mainForm = (Main)Application.OpenForms["Main"];
                        mainForm.AbrirFormularioEnPanel(visorActual);
                    }
                    finally
                    {
                        panelCargandoManga.Visible = false;
                    }

                };




                flowPanelVolumenes.Controls.Add(tomoBtn);
            }

            flowPanelVolumenes.ResumeLayout();
            flowPanelVolumenes.Refresh();
        }


        //obtener servivios Drive
        private DriveService ObtenerServicioDrive()
        {
            try
            {
                UserCredential credential;
                string path = @"Recursos\client_secret_575258247418-j2j23d6abc5a7jtnnrbgn4ldpg1is6tn.apps.googleusercontent.com.json";



                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json";

                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        new[] { DriveService.Scope.DriveReadonly },
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }

                return new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "MangaVerseApp"
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar Drive: " + ex.Message);
                return null;
            }
        }
        //Btn o Label para Cerrar
        private void labelCerrar_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Main"] is Main mainForm)
            {
                mainForm.AbrirFormularioEnPanel(mainForm.catalogForm);
            }
        }

        // btn Favoritos
        private void btnAgregarFavoritos_Click(object sender, EventArgs e)
        {
            if (mangaActual == null)
            {
                MessageBox.Show("No hay manga cargado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CapturedData.UsuarioID <= 0)
            {
                MessageBox.Show("No se ha identificado un usuario válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int mangaID = mangaActual.MangaID;

            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
            IF NOT EXISTS (
                SELECT 1 FROM Favoritos 
                WHERE MangaID = @MangaID AND UsuarioID = @UsuarioID
            )
            BEGIN
                INSERT INTO Favoritos (MangaID, UsuarioID) VALUES (@MangaID, @UsuarioID)
            END";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MangaID", mangaID);
                cmd.Parameters.AddWithValue("@UsuarioID", CapturedData.UsuarioID);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    
                    btnAgregarFavoritos.Visible = false;
                    btnQuitarFavoritos.Visible = true;

                    FavoritoAgregado?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Este manga ya está en favoritos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        // esta en favoritos
        private bool EstaEnFavoritos(int mangaID, int usuarioID)
        {
            SqlDataAccess db = new SqlDataAccess();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Favoritos WHERE MangaID = @MangaID AND UsuarioID = @UsuarioID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MangaID", mangaID);
                cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        //quitar de favoritos
        private void btnQuitarFavoritos_Click(object sender, EventArgs e)
        {
            if (mangaActual == null || CapturedData.UsuarioID <= 0)
            {
                MessageBox.Show("Datos inválidos.");
                return;
            }

            SqlDataAccess db = new SqlDataAccess();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Favoritos WHERE MangaID = @MangaID AND UsuarioID = @UsuarioID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MangaID", mangaActual.MangaID);
                cmd.Parameters.AddWithValue("@UsuarioID", CapturedData.UsuarioID);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                   
                    btnAgregarFavoritos.Visible = true;
                    btnQuitarFavoritos.Visible = false;
                    FavoritoAgregado?.Invoke(this, EventArgs.Empty); // opcional
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar.");
                }
            }
        }

    }
}
