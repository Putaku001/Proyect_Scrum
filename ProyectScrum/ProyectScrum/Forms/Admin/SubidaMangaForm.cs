using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using ProyectScrum.Data;
using ProyectScrum.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectScrum.Forms
{
    public partial class SubidaMangaForm : Form
    {
        private string rutaPortadaSeleccionada = "";
        private List<string> rutasTomosSeleccionados = new List<string>();
        private int mangaSeleccionadoID = 0;
        private bool nuevaPortadaSeleccionada = false;

        private readonly Image defaultCoverPorada;
        private class TomoSeleccion
        {
            public string RutaLocal { get; init; }
            public bool EsPremium { get; set; }
        }
        private readonly List<TomoSeleccion> tomosSeleccionados = new();


        public SubidaMangaForm()
        {
            InitializeComponent();
            CargarGeneros();


            byte[] bytes = Properties.Resources.DefaultCoverPortada;
            defaultCoverPorada = Image.FromStream(new MemoryStream(bytes));
            pictureBoxPortada.Image = defaultCoverPorada;



            
        }

        private void btnElegirImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            openFileDialog.Title = "Seleccionar portada del manga";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                rutaPortadaSeleccionada = openFileDialog.FileName;
                nuevaPortadaSeleccionada = true; 

                using (var fs = new FileStream(rutaPortadaSeleccionada, FileMode.Open, FileAccess.Read))
                {
                    pictureBoxPortada.Image = Image.FromStream(new MemoryStream(ReadFully(fs)));
                }
            }
        }

        //funcion para que no se rompa la img
        private void MostrarPortadaDefault()
        {
            pictureBoxPortada.Image = defaultCoverPorada;
        }


        // Función auxiliar para copiar todo el contenido del FileStream
        private byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private void btnElegirTomos_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new()
            {
                Multiselect = true,
                Filter = "Archivos PDF (*.pdf)|*.pdf",
                Title = "Seleccionar tomos del manga"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;
            if (dlg.FileNames.Length > 1000)
            {
                MessageBox.Show("Máximo 1000 archivos.");
                return;
            }

            // limpiamos la lista y el panel
            tomosSeleccionados.Clear();
            panelTomos.Controls.Clear();
            panelTomos.SuspendLayout();

            ToolTip tip = new();
            int y = 10;

            foreach (string ruta in dlg.FileNames)
            {
                var tomo = new TomoSeleccion { RutaLocal = ruta, EsPremium = false };
                tomosSeleccionados.Add(tomo);

                Panel cont = new()
                {
                    Width = panelTomos.Width - 35,
                    Height = 40,
                    Location = new Point(10, y),
                    BackColor = Color.FromArgb(35, 35, 35),
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label lbl = new()
                {
                    Text = Path.GetFileName(ruta),
                    Width = cont.Width - 90,
                    Height = 40,
                    Location = new Point(5, 10),
                    ForeColor = Color.White
                };

                // ---------- candado ----------
                Button btnLock = new()
                {
                    Width = 30,
                    Height = 30,
                    Text = "🔓",
                    Location = new Point(cont.Width - 75, 5),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Transparent,
                    ForeColor = Color.Red
                };
                btnLock.FlatAppearance.BorderSize = 0;
                tip.SetToolTip(btnLock, "Tomo disponible para todos");

                btnLock.Click += (_, __) =>
                {
                    tomo.EsPremium = !tomo.EsPremium;
                    if (tomo.EsPremium)
                    {
                        btnLock.Text = "🔒";
                        tip.SetToolTip(btnLock, "Tomo exclusivo Premium");
                    }
                    else
                    {
                        btnLock.Text = "🔓";
                        tip.SetToolTip(btnLock, "Tomo disponible para todos");
                    }
                };

                // ---------- eliminar ----------
                Button btnDel = new()
                {
                    Width = 30,
                    Height = 30,
                    Text = "❌",
                    Location = new Point(cont.Width - 40, 5),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Transparent,
                    ForeColor = Color.Red
                };
                btnDel.FlatAppearance.BorderSize = 0;
                tip.SetToolTip(btnDel, "Quitar tomo");

                btnDel.Click += (_, __) =>
                {
                    tomosSeleccionados.Remove(tomo);
                    panelTomos.Controls.Remove(cont);
                };

                cont.Controls.AddRange(new Control[] { lbl, btnLock, btnDel });
                panelTomos.Controls.Add(cont);
                y += cont.Height + 10;
            }
            panelTomos.ResumeLayout();
        }



        private async Task<(string urlPortada, string urlVolumenes)> SubirCarpetaMangaYArchivos()
        {
            var drive = ObtenerServicioDrive();
            var carpetaManga = await CrearCarpetaEnDrive(drive, txtTitulo.Text.Trim(), "1LgM-Yh70-ShdG4jT96DuxMEGn1L3MZPe");
            var carpetaPortada = await CrearCarpetaEnDrive(drive, "Portada", carpetaManga.Id);
            var carpetaVolumenes = await CrearCarpetaEnDrive(drive, "Volumenes", carpetaManga.Id);

            // portada
            var portadaId = await SubirArchivoDrive(drive, rutaPortadaSeleccionada, carpetaPortada.Id, "image/jpeg");

            // tomos
            foreach (var tomo in tomosSeleccionados)
            {
                string nombreFinal = tomo.EsPremium
                    ? $"[P] {Path.GetFileName(tomo.RutaLocal)}"
                    : Path.GetFileName(tomo.RutaLocal);

                await SubirArchivoDrive(
                    drive,
                    tomo.RutaLocal,
                    carpetaVolumenes.Id,
                    "application/pdf",
                    nombreFinal);
            }

            string urlPortada = $"https://drive.google.com/uc?export=view&id={portadaId}";
            string urlVolumenes = $"https://drive.google.com/drive/folders/{carpetaVolumenes.Id}?usp=sharing";
            return (urlPortada, urlVolumenes);
        }

        private async Task<Google.Apis.Drive.v3.Data.File> CrearCarpetaEnDrive(DriveService service, string nombre, string parentId)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = nombre,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string> { parentId }
            };

            var request = service.Files.Create(fileMetadata);
            request.Fields = "id";
            return await request.ExecuteAsync();
        }

        private async Task<string> SubirArchivoDrive(DriveService svc, string path, string parentId, string mime, string nombreForzado = null)
        {
            var meta = new Google.Apis.Drive.v3.Data.File
            {
                Name = nombreForzado ?? Path.GetFileName(path),
                Parents = new List<string> { parentId }
            };
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            var req = svc.Files.Create(meta, fs, mime);
            req.Fields = "id";
            var res = await req.UploadAsync();
            return res.Status == Google.Apis.Upload.UploadStatus.Completed
                   ? req.ResponseBody.Id
                   : null;
        }

        // === MÉTODO FALTANTE PARA AUTENTICAR CON DRIVE ===
        private DriveService ObtenerServicioDrive()
        {
            try
            {
                string credPath = @"Recursos\client_secret_575258247418-j2j23d6abc5a7jtnnrbgn4ldpg1is6tn.apps.googleusercontent.com.json";

                using var stream = new FileStream(credPath, FileMode.Open, FileAccess.Read);
                var cred = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { DriveService.Scope.Drive },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("token_subida", true)
                ).Result;

                return new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = cred,
                    ApplicationName = "SubidaMangaApp"
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al autenticar con Google Drive: " + ex.Message);
                return null;
            }
        }
        private void GuardarMangaEnBaseDeDatos(string titulo, string autor, string descripcion, string estado, DateTime fechaPublicacion, string urlDrive, string urlPortada, int generoID, string tituloAlternativo = "")
        {
            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                // Insertar el manga
                string insertMangaQuery = @"
            INSERT INTO Mangas (Titulo, Autor, Descripcion, Estado, FechaPublicacion, URLMangaDrive, URLPortada, GeneroID)
            OUTPUT INSERTED.MangaID
            VALUES (@Titulo, @Autor, @Descripcion, @Estado, @FechaPublicacion, @URLMangaDrive, @URLPortada, @GeneroID)";

                SqlCommand cmd = new SqlCommand(insertMangaQuery, conn);
                cmd.Parameters.AddWithValue("@Titulo", titulo);
                cmd.Parameters.AddWithValue("@Autor", autor);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                cmd.Parameters.AddWithValue("@Estado", estado);
                cmd.Parameters.AddWithValue("@FechaPublicacion", fechaPublicacion);
                cmd.Parameters.AddWithValue("@URLMangaDrive", urlDrive);
                cmd.Parameters.AddWithValue("@URLPortada", urlPortada);
                cmd.Parameters.AddWithValue("@GeneroID", generoID);

                int mangaID = (int)cmd.ExecuteScalar();

                // Si hay título alternativo
                if (!string.IsNullOrWhiteSpace(tituloAlternativo))
                {
                    string insertAlternativo = @"
                INSERT INTO TitulosAlternativos (MangaID, TituloAlternativo)
                VALUES (@MangaID, @TituloAlternativo)";

                    SqlCommand cmdAlt = new SqlCommand(insertAlternativo, conn);
                    cmdAlt.Parameters.AddWithValue("@MangaID", mangaID);
                    cmdAlt.Parameters.AddWithValue("@TituloAlternativo", tituloAlternativo);
                    cmdAlt.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Manga subido y registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
           
        }
        //btn subir
        private async void btnSubir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text) || string.IsNullOrWhiteSpace(txtAutor.Text) || string.IsNullOrWhiteSpace(rutaPortadaSeleccionada))
            {
                MessageBox.Show("Por favor llena todos los campos requeridos y selecciona portada/tomos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var (urlPortada, urlVolumenes) = await SubirCarpetaMangaYArchivos();

            GuardarMangaEnBaseDeDatos(
                txtTitulo.Text.Trim(),
                txtAutor.Text.Trim(),
                txtDescripcion.Text.Trim(),
                cmbEstado.Text.Trim(),
                dtpFecha.Value.Date,
                urlVolumenes,
                urlPortada,
                (int)cmbGenero.SelectedValue,
                txtTituloAlt.Text.Trim()
            );
            BtnCancelar_Click(null!, null!);
        }
        //cargar Genero
        private void CargarGeneros()
        {
            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT GeneroID, Nombre FROM Generos";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                cmbGenero.DisplayMember = "Nombre";
                cmbGenero.ValueMember = "GeneroID";
                cmbGenero.DataSource = dt;
            }
        }
        // modificar
        private void btnModificar_Click(object sender, EventArgs e)
        {
            panelSeleccionManga.Controls.Clear();
            panelSeleccionManga.Visible = true;
            btnSubir.Visible = false;
            btnModificar.Visible = false;
            btnGuardar.Visible = true;
            BtnCancelar.Visible = true;
            btnEliminar.Visible = true;

            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT MangaID, Titulo FROM Mangas";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                int y = 10;
                while (reader.Read())
                {
                    int mangaId = reader.GetInt32(0);
                    string titulo = reader.GetString(1);

                    Button btn = new Button
                    {
                        Text = titulo,
                        Tag = mangaId,
                        Width = 300,
                        Height = 40,
                        Top = y,
                        Left = 10,
                        BackColor = Color.DarkSlateBlue,
                        ForeColor = Color.White
                    };

                    btn.Click += async (s, args) => await CargarDatosManga((int)((Button)s).Tag);
                    panelSeleccionManga.Controls.Add(btn);
                    y += 50;
                }
            }
        }
        // cargar datos 
        private async Task CargarDatosManga(int mangaID)
        {
            SqlDataAccess db = new SqlDataAccess();
            mangaSeleccionadoID = mangaID;


            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = @"SELECT M.Titulo, M.Autor, M.Descripcion, M.Estado, 
                         M.FechaPublicacion, M.URLMangaDrive, M.URLPortada, M.GeneroID,
                         TA.TituloAlternativo
                         FROM Mangas M
                         LEFT JOIN TitulosAlternativos TA ON M.MangaID = TA.MangaID
                         WHERE M.MangaID = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", mangaID);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtTitulo.Text = reader.GetString(0);
                    txtAutor.Text = reader.GetString(1);
                    txtDescripcion.Text = reader.GetString(2);
                    cmbEstado.Text = reader.GetString(3);
                    dtpFecha.Value = reader.GetDateTime(4);
                    txtTituloAlt.Text = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    cmbGenero.SelectedValue = reader.GetInt32(7);
                    pictureBoxPortada.ImageLocation = reader.GetString(6);
                    panelSeleccionManga.Visible = false;

                    
                    string urlDrive = reader.GetString(5);
                    await CargarTomosDesdeDrive(urlDrive);
                }
            }
        }




        // para mangas
        private string ExtraerIdCarpeta(string url)
        {
            var partes = url.Split(new string[] { "folders/" }, StringSplitOptions.None);
            if (partes.Length > 1)
            {
                return partes[1].Split('?')[0];
            }
            return string.Empty;
        }


        private async Task CargarTomosDesdeDrive(string urlVolumenes)
        {
            panelTomos.Controls.Clear();
            panelTomos.SuspendLayout();
            panelTomos.AutoScroll = true;

            ToolTip toolTip = new ToolTip();

            string folderId = ExtraerIdCarpeta(urlVolumenes);
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

            var files = new List<Google.Apis.Drive.v3.Data.File>();
            string pageToken = null;

            do
            {
                var request = service.Files.List();
                request.Q = $"'{folderId}' in parents and mimeType='application/pdf' and trashed = false";
                request.Fields = "nextPageToken, files(id, name)";
                request.PageToken = pageToken;
                request.PageSize = 1000;

                var result = await request.ExecuteAsync();
                if (result.Files != null)
                    files.AddRange(result.Files);

                pageToken = result.NextPageToken;
            } while (pageToken != null);

            files = files.OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase).ToList();

            int y = 10;
            foreach (var file in files)
            {
                Panel contenedor = new Panel
                {
                    Width = panelTomos.Width - 35,
                    Height = 40,
                    Location = new Point(10, y),
                    BackColor = Color.FromArgb(35, 35, 35),
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label lblNombre = new Label
                {
                    Text = file.Name,
                    AutoSize = false,
                    Width = contenedor.Width - 90,
                    Height = 40,
                    Location = new Point(5, 10),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9),
                };


                var fileId = file.Id; 
                var nombreActual = file.Name;

                // Botón candado
                Button btnLock = new Button
                {
                    Width = 30,
                    Height = 30,
                    Location = new Point(contenedor.Width - 75, 5),
                    Text = nombreActual.StartsWith("[P]") ? "🔒" : "🔓",
                    BackColor = Color.Transparent,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.Red,
                    Tag = nombreActual.StartsWith("[P]")    // true = es premium
                };
                btnLock.FlatAppearance.BorderSize = 0;
                toolTip.SetToolTip(btnLock,
                        (bool)btnLock.Tag ? "Tomo exclusivo Premium" : "Tomo disponible para todos");

                btnLock.Click += async (s, e) =>
                {
                    bool bloqueado = (bool)btnLock.Tag;
                    string nombreNuevo = bloqueado
                        ? nombreActual.Replace("[P] ", "")         
                        : $"[P] {nombreActual}";                   

                    try
                    {
                        // renombrar en Google Drive
                        var update = service.Files.Update(
                            new Google.Apis.Drive.v3.Data.File { Name = nombreNuevo },
                            fileId);
                        update.Fields = "name";
                        await update.ExecuteAsync();               

                        // reflejar cambios en la interfaz
                        lblNombre.Text = nombreNuevo;
                        btnLock.Tag = !bloqueado;                
                        btnLock.Text = bloqueado ? "🔓" : "🔒";
                        toolTip.SetToolTip(btnLock,
                            bloqueado ? "Tomo disponible para todos"
                                      : "Tomo exclusivo Premium");
                        nombreActual = nombreNuevo;               
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No se pudo cambiar el estado premium:\n" + ex.Message,
                                        "Error en Drive", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                };

                // Botón eliminar
                Button btnDelete = new Button
                {
                    Width = 30,
                    Height = 30,
                    Location = new Point(contenedor.Width - 40, 5),
                    Text = "❌",
                    BackColor = Color.Transparent,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.Red,
                    Tag = file.Id
                };
                btnDelete.FlatAppearance.BorderSize = 0;
                toolTip.SetToolTip(btnDelete, "Eliminar tomo");

                btnDelete.Click += async (s, e) =>
                {
                    DialogResult confirm = MessageBox.Show("¿Deseas eliminar este tomo de Drive?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        try
                        {
                            await service.Files.Delete(file.Id).ExecuteAsync();
                            panelTomos.Controls.Remove(contenedor);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al eliminar el archivo: " + ex.Message);
                        }
                    }
                };

                contenedor.Controls.Add(lblNombre);
                contenedor.Controls.Add(btnLock);
                contenedor.Controls.Add(btnDelete);
                panelTomos.Controls.Add(contenedor);

                y += contenedor.Height + 10;
            }

            panelTomos.ResumeLayout();
        }

        private bool UsuarioEsPremium()
        {
            return CapturedData.EsPremium;
                                           
        }

        private string ExtraerIdArchivoDesdeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return "";

            try
            {
                // Extrae por id=... aunque haya más parámetros
                var uri = new Uri(url);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                var id = query["id"];
                if (!string.IsNullOrEmpty(id))
                    return id;

                // Alternativa si es formato /file/d/XXXXX/view
                if (url.Contains("/file/d/"))
                {
                    var partes = url.Split(new[] { "/file/d/" }, StringSplitOptions.None);
                    if (partes.Length > 1)
                        return partes[1].Split('/')[0];
                }
            }
            catch
            {
                // fallback manual
                if (url.Contains("id="))
                {
                    var partes = url.Split(new[] { "id=" }, StringSplitOptions.None);
                    return partes[1].Split('&')[0];
                }
            }

            return "";
        }
        private async Task<bool> EliminarPortadaAnterior(string urlPortada)
        {
            string idPortada = ExtraerIdArchivoDesdeUrl(urlPortada);
            if (string.IsNullOrEmpty(idPortada))
                return false;

            try
            {
                var service = ObtenerServicioDrive();
                await service.Files.Delete(idPortada).ExecuteAsync();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar portada anterior: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private async Task<string> SubirNuevaPortada(string pathImagen, string idCarpetaPortada)
        {
            try
            {
                var service = ObtenerServicioDrive();
                string extension = Path.GetExtension(pathImagen).ToLower();
                string mimeType = extension switch
                {
                    ".png" => "image/png",
                    ".jpeg" => "image/jpeg",
                    ".jpg" => "image/jpeg",
                    _ => "application/octet-stream"
                };

                var fileMetadata = new Google.Apis.Drive.v3.Data.File
                {
                    Name = Path.GetFileName(pathImagen),
                    Parents = new List<string> { idCarpetaPortada }
                };

                using var stream = new FileStream(pathImagen, FileMode.Open, FileAccess.Read);
                var request = service.Files.Create(fileMetadata, stream, mimeType);
                request.Fields = "id";
                var result = await request.UploadAsync();

                if (result.Status == Google.Apis.Upload.UploadStatus.Completed)
                {
                    string nuevoID = request.ResponseBody.Id;
                    return $"https://drive.google.com/uc?export=view&id={nuevoID}";
                }
                else
                {
                    MessageBox.Show("Error al subir nueva portada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al subir la nueva portada: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }


        //metodo de actualizar 
        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (mangaSeleccionadoID == 0) { MessageBox.Show("…"); return; }

            var drive = ObtenerServicioDrive();
            if (drive == null) return;

            // ─── URLs actuales ──────────────────────────────────────────────────────
            string urlPortadaActual, urlVolumenes;
            using (var c = new SqlDataAccess().GetConnection())
            {
                c.Open();
                using var cmd = new SqlCommand(
                    "SELECT URLPortada, URLMangaDrive FROM Mangas WHERE MangaID=@id", c);
                cmd.Parameters.AddWithValue("@id", mangaSeleccionadoID);
                using var rd = cmd.ExecuteReader();
                rd.Read();
                urlPortadaActual = rd.GetString(0);
                urlVolumenes = rd.GetString(1);
            }

            // ─── PORTADA ─────────────────────────────────────────────────────────────
            string urlPortadaFinal = urlPortadaActual;

            if (nuevaPortadaSeleccionada && File.Exists(rutaPortadaSeleccionada))
            {
                // ① Intento por nombre
                string carpetaPortadaId =
                    await ObtenerCarpetaID(drive, mangaSeleccionadoID, "Portada")
                    // ② Fallback → usa parent de la portada vieja
                    ?? await ObtenerCarpetaPortadaDesdeArchivo(drive, urlPortadaActual);

                if (!string.IsNullOrEmpty(carpetaPortadaId))
                {
                    await EliminarPortadaAnterior(urlPortadaActual);
                    string nuevaUrl = await SubirNuevaPortada(rutaPortadaSeleccionada,
                                                              carpetaPortadaId);
                    if (!string.IsNullOrEmpty(nuevaUrl))
                    {
                        urlPortadaFinal = nuevaUrl;
                        pictureBoxPortada.ImageLocation = nuevaUrl;  // refresco UI
                    }
                }
                else
                {
                    MessageBox.Show("No se encontró la carpeta «Portada» en Drive.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // ─── NUEVOS TOMOS ────────────────────────────────────────────────────────
            if (tomosSeleccionados.Any())
            {
                string carpetaVolumenesId = ExtraerIdCarpeta(urlVolumenes);

                foreach (var tomo in tomosSeleccionados)
                {
                    string nombreFinal = tomo.EsPremium
                        ? $"[P] {Path.GetFileName(tomo.RutaLocal)}"
                        : Path.GetFileName(tomo.RutaLocal);

                    await SubirArchivoDrive(drive, tomo.RutaLocal,
                                            carpetaVolumenesId, "application/pdf",
                                            nombreFinal);
                }
                tomosSeleccionados.Clear();   // limpia la lista tras subir
            }

            // ─── UPDATE BD (igual que antes, sólo cambia URLPortadaFinal) ────────────
            using (var conn = new SqlDataAccess().GetConnection())
            {
                conn.Open();
                var tx = conn.BeginTransaction();
                try
                {
                    using var up = new SqlCommand(@"
                UPDATE Mangas SET
                    Titulo=@T, Autor=@A, Descripcion=@D, Estado=@E,
                    FechaPublicacion=@F, GeneroID=@G, URLPortada=@U
                WHERE MangaID=@id", conn, tx);

                    up.Parameters.AddWithValue("@T", txtTitulo.Text.Trim());
                    up.Parameters.AddWithValue("@A", txtAutor.Text.Trim());
                    up.Parameters.AddWithValue("@D", txtDescripcion.Text.Trim());
                    up.Parameters.AddWithValue("@E", cmbEstado.Text.Trim());
                    up.Parameters.AddWithValue("@F", dtpFecha.Value.Date);
                    up.Parameters.AddWithValue("@G", cmbGenero.SelectedValue);
                    up.Parameters.AddWithValue("@U", urlPortadaFinal);
                    up.Parameters.AddWithValue("@id", mangaSeleccionadoID);
                    up.ExecuteNonQuery();

                    // TitulosAlternativos (sin cambios) …
                    //   ─────────────────────────────────

                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    MessageBox.Show("BD: " + ex.Message);
                    return;
                }
            }

            rutasTomosSeleccionados.Clear();
            nuevaPortadaSeleccionada = false;
            MessageBox.Show("Cambios guardados correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

            BtnCancelar_Click(null!, null!);
        }





        private async Task<string?> ObtenerCarpetaID(DriveService service,
                                             int mangaID,
                                             string nombreSubcarpeta)
        {
            // 1) ─── Carpeta raíz del manga (se obtiene de la URL almacenada)
            string urlManga = string.Empty;

            using (SqlConnection conn = new SqlDataAccess().GetConnection())
            {
                conn.Open();
                using SqlCommand cmd = new SqlCommand(
                    "SELECT URLMangaDrive FROM Mangas WHERE MangaID = @id", conn);
                cmd.Parameters.AddWithValue("@id", mangaID);
                urlManga = (string?)cmd.ExecuteScalar() ?? "";
            }

            string carpetaRaizId = ExtraerIdCarpeta(urlManga);
            if (string.IsNullOrWhiteSpace(carpetaRaizId)) return null;

            // 2) ─── Buscar la sub-carpeta (soporta paginación y case-insensitive)
            string pageToken = null;
            do
            {
                var list = service.Files.List();
                list.Q =
                    $"'{carpetaRaizId}' in parents " +
                    "and mimeType='application/vnd.google-apps.folder' " +
                    "and trashed=false";
                list.Fields = "nextPageToken, files(id, name)";
                list.PageSize = 1000;
                list.PageToken = pageToken;

                var res = await list.ExecuteAsync();
                foreach (var f in res.Files)
                {
                    if (string.Equals(f.Name.Trim(), nombreSubcarpeta.Trim(),
                                      StringComparison.OrdinalIgnoreCase))
                        return f.Id;
                }
                pageToken = res.NextPageToken;
            }
            while (pageToken != null);

            return null; // no encontrada
        }
        private async Task<string?> ObtenerCarpetaPortadaDesdeArchivo(DriveService svc,
                                                              string urlPortadaActual)
        {
            string idArchivo = ExtraerIdArchivoDesdeUrl(urlPortadaActual);
            if (string.IsNullOrWhiteSpace(idArchivo)) return null;

            var get = svc.Files.Get(idArchivo);
            get.Fields = "parents";
            var info = await get.ExecuteAsync();
            return info.Parents?.FirstOrDefault();
        }


        //btn Cancelar

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            // Restaurar UI
            panelSeleccionManga.Visible = false;
            btnSubir.Visible = true;
            btnModificar.Visible = true;
            btnGuardar.Visible = false;
            BtnCancelar.Visible = false;
            btnEliminar.Visible = false;

            // Limpiar campos del formulario
            txtTitulo.Text = "";
            txtAutor.Text = "";
            txtDescripcion.Text = "";
            txtTituloAlt.Text = "";
            cmbEstado.SelectedIndex = -1;
            cmbGenero.SelectedIndex = -1;
            dtpFecha.Value = DateTime.Now;
            MostrarPortadaDefault();
            panelTomos.Controls.Clear();
            rutaPortadaSeleccionada = "";
            rutasTomosSeleccionados.Clear();
            mangaSeleccionadoID = 0;
            nuevaPortadaSeleccionada = false;

        }

        //btn Eliminar
        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (mangaSeleccionadoID == 0)
            {
                MessageBox.Show("Primero carga un manga con el botón «Modificar».", "Aviso",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmar = MessageBox.Show(
                "¡¡ATENCIÓN!!\n\nSe eliminará el manga COMPLETO –portada, tomos y carpetas– " +
                "tanto en Google Drive como en la base de datos.\n\n¿Deseas continuar?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmar != DialogResult.Yes) return;

            // 1) -----------------------------  Recuperar datos del manga
            string urlVolumenes = "";
            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                using SqlCommand cmd = new SqlCommand(
                    "SELECT URLMangaDrive FROM Mangas WHERE MangaID = @id", conn);
                cmd.Parameters.AddWithValue("@id", mangaSeleccionadoID);
                urlVolumenes = (string?)cmd.ExecuteScalar() ?? "";
            }

            if (string.IsNullOrWhiteSpace(urlVolumenes))
            {
                MessageBox.Show("No se encontró la URL de Volúmenes en la BD.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string volumenesId = ExtraerIdCarpeta(urlVolumenes);
            if (string.IsNullOrWhiteSpace(volumenesId))
            {
                MessageBox.Show("No se pudo extraer el ID de la carpeta «Volumenes».", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2) -----------------------------  Eliminar en Google Drive
            var service = ObtenerServicioDrive();
            if (service == null) return;   // ya se mostró el mensaje de error dentro de la función

            try
            {
                // Obtener la carpeta padre (carpeta raíz del manga)
                var getVol = service.Files.Get(volumenesId);
                getVol.Fields = "parents";
                var volInfo = await getVol.ExecuteAsync();

                if (volInfo.Parents == null || volInfo.Parents.Count == 0)
                    throw new Exception("No se encontró la carpeta raíz en Drive.");

                string carpetaRaizId = volInfo.Parents[0];

                // Con una sola llamada eliminamos la raíz → se lleva todo lo de adentro
                await service.Files.Delete(carpetaRaizId).ExecuteAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar en Drive: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3) -----------------------------  Eliminar en la base de datos
            try
            {
                using SqlConnection conn = db.GetConnection();
                conn.Open();

                SqlTransaction tx = conn.BeginTransaction();

                // Si tienes otras tablas que referencian MangaID (ProgresoLectura, Favoritos, etc.),
                // elimínalas primero aquí para evitar FK violations.
                string[] tablasHijas = { "ProgresoLectura", "Favoritos", "TitulosAlternativos" };
                foreach (string tabla in tablasHijas)
                {
                    string qHija = $"DELETE FROM {tabla} WHERE MangaID = @id";
                    using SqlCommand cmdH = new SqlCommand(qHija, conn, tx);
                    cmdH.Parameters.AddWithValue("@id", mangaSeleccionadoID);
                    cmdH.ExecuteNonQuery();
                }

                // Finalmente la tabla principal
                using SqlCommand cmdDel = new SqlCommand(
                    "DELETE FROM Mangas WHERE MangaID = @id", conn, tx);
                cmdDel.Parameters.AddWithValue("@id", mangaSeleccionadoID);
                cmdDel.ExecuteNonQuery();

                tx.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar en la base de datos: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4) -----------------------------  Limpiar la interfaz
            MessageBox.Show("Manga eliminado correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

            BtnCancelar_Click(null!, null!);
        }

    }
}
