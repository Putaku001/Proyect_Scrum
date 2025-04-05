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

namespace ProyectScrum.Forms
{
    public partial class mangaForm : Form
    {
        public mangaForm()
        {
            InitializeComponent();
        }
        public void CargarManga(Manga manga, string genero)
        {
            try
            {
                picturePortada.LoadAsync(manga.URLPortada);
            }
            catch
            {
                picturePortada.Image = Properties.Resources.DefaultCover;
            }

            textTitle.Text = manga.Titulo;
            labelTitulo.Text = manga.Titulo;
            labelAutor.Text = manga.Autor;
            labelFecha.Text = manga.FechaPublicacion.ToShortDateString();
            labelDescripcion.Text = manga.Descripcion;
            labelGenero.Text = genero;
            CargarVolumenes(manga.URLMangaDrive);

        }
        private string ExtraerIdCarpeta(string url)
        {
            var partes = url.Split(new string[] { "folders/" }, StringSplitOptions.None);
            if (partes.Length > 1)
            {
                return partes[1].Split('?')[0];
            }
            return string.Empty;
        }

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
                request.Fields = "nextPageToken, files(id, name, webViewLink)";
                request.PageSize = 1000;
                request.PageToken = pageToken;

                var result = request.Execute();
                files.AddRange(result.Files);
                pageToken = result.NextPageToken;
            } while (pageToken != null);

            files = files
                .OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var file in files)
            {
                Button tomoBtn = new Button
                {
                    Text = $"📖 {file.Name}",
                    Width = flowPanelVolumenes.Width - 40,
                    Height = 45,
                    BackColor = Color.FromArgb(35, 35, 35),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(5),
                    Tag = file.WebViewLink,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                tomoBtn.Click += (s, e) =>
                {
                    string link = ((Button)s).Tag.ToString();
                    MessageBox.Show($"Este tomo se abrirá en el visor en el futuro.\nURL: {link}", "Tomo detectado", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

    }
}
