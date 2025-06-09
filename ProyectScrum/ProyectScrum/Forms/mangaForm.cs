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

        private Dictionary<string, Image> cacheImagenes = new Dictionary<string, Image>();
        private Manga mangaActual;
        int usuarioID = CapturedData.UsuarioID;
        public event EventHandler FavoritoAgregado;
        private visorForm visorActual = null;




        public mangaForm(int usuarioID)
        {
            InitializeComponent();
            this.Resize += mangaForm_Resize;
            flowPanelVolumenes.Resize += flowPanelVolumenes_Resize;
        }
        private void mangaForm_Resize(object sender, EventArgs e)
        {
            foreach (Control control in flowPanelVolumenes.Controls)
            {
                if (control is Panel panel)
                {
                    panel.Width = flowPanelVolumenes.ClientSize.Width - 20;

                    foreach (Control sub in panel.Controls)
                    {
                        if (sub is Guna.UI2.WinForms.Guna2Button btn)
                        {
                            btn.Width = panel.Width;
                        }
                    }
                }
            }
        }
        private void flowPanelVolumenes_Resize(object sender, EventArgs e)
        {
            foreach (Control control in flowPanelVolumenes.Controls)
            {
                if (control is Panel panel)
                {
                    int nuevoAncho = flowPanelVolumenes.ClientSize.Width - 25;
                    panel.Width = nuevoAncho;

                    foreach (Control sub in panel.Controls)
                    {
                        if (sub is Guna.UI2.WinForms.Guna2Button btn)
                        {
                            btn.Width = nuevoAncho;
                        }
                    }
                }
            }
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
            ActualizarBotonFavoritos();

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

            // Obtener progreso general del manga
            var progreso = ObtenerProgresoLectura(CapturedData.UsuarioID, mangaActual.MangaID);

            // Solo mostrar detalles en el primer tomo por ahora
            string primerTomoId = files.FirstOrDefault()?.Id;

            // Obtener progreso general del manga
            var progresoGlobal = ObtenerProgresoLectura(CapturedData.UsuarioID, mangaActual.MangaID);

            foreach (var file in files)
            {
                var contenedor = new Panel
                {
                    AutoSize = true,
                    Margin = new Padding(2),
                    BackColor = Color.Transparent,
                    Width = flowPanelVolumenes.ClientSize.Width,
                    Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
                };


                // Botón del tomo
                bool esPremium = file.Name.StartsWith("[P]");

                var tomoBtn = new Guna.UI2.WinForms.Guna2Button
                {
                    Text = $"{(esPremium ? "🔒" : "📄")} {file.Name.Replace("[P]", "").Trim()}",
                    Width = contenedor.Width,
                    Height = 45,
                    BorderRadius = 5,
                    FillColor = Color.FromArgb(35, 35, 35),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    TextAlign = HorizontalAlignment.Left,
                    Cursor = Cursors.Hand,
                    Enabled = !esPremium || CapturedData.EsPremium,
                    Tag = file.Id,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    Margin = new Padding(0),
                    DisabledState = {
                        FillColor = Color.FromArgb(50, 50, 50),
                        ForeColor = Color.Gray
                    },
                    HoverState = {
                        FillColor = Color.FromArgb(50, 50, 50)
                    }
                };

                // Evento click para abrir visor

                if (esPremium && !CapturedData.EsPremium)
                {
                    // Usuario normal intenta abrir tomo premium → solo aviso
                    tomoBtn.Click += (_, __) =>
                        MessageBox.Show("Este tomo es exclusivo para usuarios Premium.",
                                        "Acceso restringido", MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                }
                else
                {
                    tomoBtn.Click += async (s, e) =>
                    {

                        if (visorActual != null && !visorActual.IsDisposed)
                        {
                            MessageBox.Show("Ya tienes un visor abierto. Ciérralo antes de abrir otro tomo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        string fileId = ((Guna.UI2.WinForms.Guna2Button)s).Tag.ToString();

                        foreach (Control control in flowPanelVolumenes.Controls)
                            if (control is Panel p)
                                foreach (Control c in p.Controls)
                                    if (c is Button b) b.Enabled = false;

                        panelCargandoManga.Visible = true;
                        panelCargandoManga.BringToFront();
                        panelCargandoManga.Refresh();

                        try
                        {
                            var request = service.Files.Get(fileId);
                            var stream = new MemoryStream();
                            await request.DownloadAsync(stream);
                            stream.Position = 0;

                            visorActual = new visorForm(stream, this, mangaActual);
                            visorActual.AsignarMangaActual(mangaActual);

                            visorActual.FormClosed += (sender2, args) =>
                            {
                                visorActual = null;
                                CargarVolumenes(mangaActual.URLMangaDrive); // Recarga progreso actualizado
                            };

                            Main mainForm = (Main)Application.OpenForms["Main"];
                            mainForm.AbrirFormularioEnPanel(visorActual);
                        }
                        finally
                        {
                            panelCargandoManga.Visible = false;
                        }
                    };
                }
                ;

                contenedor.Controls.Add(tomoBtn);

                // Mostrar progreso solo si existe y solo en el primer tomo
                if (progresoGlobal is (int pagina, string status, int tiempo, int veces)
                    && pagina > 0
                    && file == files.First())
                {
                    FlowLayoutPanel panelProgreso = new FlowLayoutPanel
                    {
                        FlowDirection = FlowDirection.LeftToRight,
                        Width = contenedor.Width,
                        Height = 35,
                        Location = new Point(0, tomoBtn.Bottom),
                        AutoSize = true,
                        Margin = new Padding(0)
                    };

                    panelProgreso.Controls.Add(CrearBloqueProgreso($"Continuar (pág {pagina + 1})"));
                    panelProgreso.Controls.Add(CrearBloqueProgreso($"Estado: {status}"));
                    panelProgreso.Controls.Add(CrearBloqueProgreso($"Veces leído: {veces}"));
                    panelProgreso.Controls.Add(CrearBloqueProgreso($"Tiempo: {tiempo} min"));

                    // bloque ya no me interesa
                    Label lblEliminar = CrearBloqueProgreso("Ya no me interesa");
                    lblEliminar.Cursor = Cursors.Hand;
                    lblEliminar.BackColor = Color.FromArgb(40, 0, 0); // color diferente
                    lblEliminar.Click += (s, e) =>
                    {
                        var confirm = MessageBox.Show("¿Seguro que deseas eliminar tu progreso de lectura?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (confirm == DialogResult.Yes)
                        {
                            EliminarProgresoLectura(CapturedData.UsuarioID, mangaActual.MangaID);
                            CargarVolumenes(mangaActual.URLMangaDrive); // Refrescar la vista
                        }
                    };
                    panelProgreso.Controls.Add(lblEliminar);

                    contenedor.Controls.Add(panelProgreso);
                    contenedor.Height += 35;
                }


                flowPanelVolumenes.Controls.Add(contenedor);
            }




            flowPanelVolumenes.ResumeLayout();
            flowPanelVolumenes.Refresh();
        }
        private (int PaginaActual, string Status, int TiempoLecturaTotal, int VecesLeido)? ObtenerProgresoLectura(int usuarioID, int mangaID)
        {
            SqlDataAccess db = new SqlDataAccess();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = @"
            SELECT PaginaActual, Status, TiempoLecturaTotal, VecesLeido
            FROM ProgresoLectura
            WHERE UsuarioID = @UsuarioID AND MangaID = @MangaID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                cmd.Parameters.AddWithValue("@MangaID", mangaID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetInt32(2),
                            reader.GetInt32(3)
                        );
                    }
                }
            }

            return null;
        }

        //metodo no me interesa
        private void EliminarProgresoLectura(int usuarioID, int mangaID)
        {
            SqlDataAccess db = new SqlDataAccess();
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM ProgresoLectura WHERE UsuarioID = @UsuarioID AND MangaID = @MangaID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                cmd.Parameters.AddWithValue("@MangaID", mangaID);
                cmd.ExecuteNonQuery();
            }
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
                if (mainForm.favsForm != null && !mainForm.favsForm.IsDisposed)
                {
                    mainForm.AbrirFormularioEnPanel(mainForm.favsForm);
                }
                else if (mainForm.catalogForm != null && !mainForm.catalogForm.IsDisposed)
                {
                    mainForm.AbrirFormularioEnPanel(mainForm.catalogForm);
                }
                else
                {
                    // Fallback
                    mainForm.catalogForm = new Catalog(mainForm._emailSettings);
                    mainForm.AbrirFormularioEnPanel(mainForm.catalogForm);
                }
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

                    ActualizarBotonFavoritos();
                    FavoritoAgregado?.Invoke(this, EventArgs.Empty);


                    FavoritoAgregado?.Invoke(this, EventArgs.Empty);
                }
                else
                {

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

                    ActualizarBotonFavoritos();
                    FavoritoAgregado?.Invoke(this, EventArgs.Empty);

                }
                else
                {
                    MessageBox.Show("No se pudo eliminar.");
                }
            }
        }
        private void ActualizarBotonFavoritos()
        {
            bool enFavoritos = EstaEnFavoritos(mangaActual.MangaID, CapturedData.UsuarioID);

            if (enFavoritos)
            {
                btnAgregarFavoritos.Text = "Eliminar de 💔";
                btnAgregarFavoritos.FillColor = Color.DarkRed;
                btnAgregarFavoritos.Click -= btnAgregarFavoritos_Click;
                btnAgregarFavoritos.Click += btnQuitarFavoritos_Click;
            }
            else
            {
                btnAgregarFavoritos.Text = "Agregar a ♡";
                btnAgregarFavoritos.FillColor = Color.Transparent;
                btnAgregarFavoritos.Click -= btnQuitarFavoritos_Click;
                btnAgregarFavoritos.Click += btnAgregarFavoritos_Click;
            }
        }


        private Label CrearBloqueProgreso(string texto)
        {
            return new Label
            {
                Text = texto,
                BackColor = Color.FromArgb(25, 25, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 7, FontStyle.Regular),
                AutoSize = false,
                Height = 30,
                Width = 110,
                Margin = new Padding(3, 0, 3, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle
            };
        }
    }
}
