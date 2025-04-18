using System;
using System.IO;
using System.Windows.Forms;
using PdfiumViewer;

namespace ProyectScrum.Forms
{
    public partial class visorForm : Form
    {
        private MemoryStream pdfStream;
        private Form mangaFormReferencia;
        private PdfRenderer pdfRenderer;
        private ClickOutsidePanelFilter filtroClick;
        private PdfiumViewer.PdfDocument documentoPdf;
        private PictureBox visorUnico;
        private int paginaActual = 0;
        private enum ModoLectura { Cascada, Libro, Manga }
        private ModoLectura modoActual = ModoLectura.Cascada;

        private Panel panelOverlay;
        private Label lblOverlay;

        public visorForm(MemoryStream stream, Form origen)
        {
            InitializeComponent();
            pdfStream = stream;
            mangaFormReferencia = origen;
            this.Load += visorForm_Load;
            btnCerrar.Click += btnCerrar_Click;

            //btn herramientas

            filtroClick = new ClickOutsidePanelFilter(panelHerramientas, this);
            Application.AddMessageFilter(filtroClick);

            //mensaje de maximizado
            panelOverlay = new Panel
            {
                BackColor = Color.FromArgb(180, 0, 0, 0),
                Dock = DockStyle.Fill,
                Visible = false
            };
            lblOverlay = new Label
            {
                Text = "⚠ Manga reproduciéndose " +
                "en modo maximizado",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblOverlay.Location = new Point(
                (this.Width - lblOverlay.Width) / 2,
                (this.Height - lblOverlay.Height) / 2
            );

            panelOverlay.Controls.Add(lblOverlay);
            this.Controls.Add(panelOverlay);
            panelOverlay.BringToFront();

        }

        private void visorForm_Load(object sender, EventArgs e)
        {
            try
            {
                panelHerramientas.Visible = false;
                btnAnterior.Visible = false;
                btnSiguiente.Visible = false;

                pdfStream.Position = 0;
                documentoPdf = PdfiumViewer.PdfDocument.Load(pdfStream);

                paginaActual = 0;
                CambiarModo(ModoLectura.Cascada);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el PDF: " + ex.Message);
            }
        }

        public void MostrarOverlayMaximizado(bool mostrar)
        {
            panelOverlay.Visible = mostrar;
            this.Enabled = !mostrar;
        }

        private void CambiarModo(ModoLectura modo)
        {
            modoActual = modo;
            pnlContenedorPdf.Controls.Clear();

            if (modo == ModoLectura.Cascada)
            {
                pdfRenderer = new PdfRenderer
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Black
                };

                pdfRenderer.Load(documentoPdf);
                pnlContenedorPdf.Controls.Add(pdfRenderer);
                btnAnterior.Visible = false;
                btnSiguiente.Visible = false;
                lblContadorPaginas.Visible = false;
            }
            else
            {
                PictureBox visor = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Black,
                    SizeMode = PictureBoxSizeMode.Zoom
                };

                pnlContenedorPdf.Controls.Add(visor);
                btnAnterior.Visible = true;
                btnSiguiente.Visible = true;
                lblContadorPaginas.Visible = true;
                MostrarPagina();
            }
        }

        private void MostrarPagina()
        {
            if (documentoPdf == null || paginaActual < 0 || paginaActual >= documentoPdf.PageCount)
                return;

            var imagen = documentoPdf.Render(paginaActual, pnlContenedorPdf.Width, pnlContenedorPdf.Height, true);

            //if (modoActual == ModoLectura.Manga)
            //    imagen.RotateFlip(RotateFlipType.RotateNoneFlipX);

            var visor = pnlContenedorPdf.Controls.OfType<PictureBox>().FirstOrDefault();
            if (visor != null)
            {
                visor.Image?.Dispose();
                visor.Image = imagen;
            }

            ActualizarContadorPaginas();
        }



        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (mangaFormReferencia != null)
            {
                Main mainForm = (Main)Application.OpenForms["Main"];
                mainForm.AbrirFormularioEnPanel(mangaFormReferencia);
            }
            this.Close();
        }

        private void btnHerramientas_Click(object sender, EventArgs e)
        {
            panelHerramientas.Visible = !panelHerramientas.Visible;
            panelHerramientas.BringToFront();
        }

        private void visorForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (!panelHerramientas.Visible)
                return;

            // Obtener posición relativa al formulario
            Point mousePos = this.PointToClient(Cursor.Position);

            // Verifica si el mouse NO está sobre el panel
            if (!panelHerramientas.Bounds.Contains(mousePos))
            {
                panelHerramientas.Visible = false;
            }
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Application.RemoveMessageFilter(filtroClick);
            base.OnFormClosed(e);
        }

        // contador
        private void ActualizarContadorPaginas()
        {
            if (documentoPdf != null && lblContadorPaginas != null)
            {
                int totalPaginas = documentoPdf.PageCount;
                int actual = paginaActual + 1;
                lblContadorPaginas.Text = $"Página: {actual} / {totalPaginas}";
            }
        }




        //botones

        private void btnCascada_Click(object sender, EventArgs e)
        {
            CambiarModo(ModoLectura.Cascada);
        }

        private void btnLibro_Click(object sender, EventArgs e)
        {
            CambiarModo(ModoLectura.Libro);
        }

        private void btnManga_Click(object sender, EventArgs e)
        {
            CambiarModo(ModoLectura.Manga);
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (modoActual == ModoLectura.Manga)
            {
                if (paginaActual < documentoPdf.PageCount - 1)
                {
                    paginaActual++;
                    MostrarPagina();
                }
            }
            else
            {
                if (paginaActual > 0)
                {
                    paginaActual--;
                    MostrarPagina();
                }
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (modoActual == ModoLectura.Manga)
            {
                if (paginaActual > 0)
                {
                    paginaActual--;
                    MostrarPagina();
                }
            }
            else
            {
                if (paginaActual < documentoPdf.PageCount - 1)
                {
                    paginaActual++;
                    MostrarPagina();
                }
            }
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            
            visorMaxForm visorFull = new visorMaxForm(documentoPdf, paginaActual, modoActual.ToString());
            visorFull.FormClosed += (s, args) => MostrarOverlayMaximizado(false); 
            MostrarOverlayMaximizado(true); 
            visorFull.Show();
        }
    }
    //clase para recorrer el mouse y cerrar
    public class ClickOutsidePanelFilter : IMessageFilter
    {
        private Control panel;
        private Form form;

        public ClickOutsidePanelFilter(Control panel, Form form)
        {
            this.panel = panel;
            this.form = form;
        }

        public bool PreFilterMessage(ref Message m)
        {
            const int WM_LBUTTONDOWN = 0x0201;

            if (m.Msg == WM_LBUTTONDOWN && panel.Visible)
            {
                Point pos = Control.MousePosition;
                Point relative = form.PointToClient(pos);

                if (!panel.Bounds.Contains(relative))
                {
                    panel.Visible = false;
                }
            }

            return false;
        }
    }


}
