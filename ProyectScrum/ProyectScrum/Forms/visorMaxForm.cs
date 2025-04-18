using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PdfiumViewer;

namespace ProyectScrum.Forms
{
    public partial class visorMaxForm : Form
    {
        private PdfiumViewer.PdfDocument documentoPdf;
        private int paginaActual;
        private string modoActual;

        private PictureBox visor;
        private Label lblContador;
        private Button btnAnterior;
        private Button btnSiguiente;
        private Button btnCerrar;
        private Button btnLibro;
        private Button btnManga;
        private Button btnCascada;
        private PdfRenderer pdfRenderer;

        public visorMaxForm(PdfiumViewer.PdfDocument document, int paginaInicial, string modo)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Black;

            documentoPdf = document;
            paginaActual = paginaInicial;
            modoActual = modo;

            CrearControles();
            CambiarModo(modo);
        }

        private void CrearControles()
        {
            // Label contador
            lblContador = new Label
            {
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            this.Controls.Add(lblContador);

            // Botón cerrar
            btnCerrar = new Button
            {
                Text = "Cerrar",
                ForeColor = Color.White,
                BackColor = Color.DarkRed,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(this.ClientSize.Width - 95, 10),
                Size = new Size(90, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnCerrar.Click += (s, e) => this.Close();
            this.Controls.Add(btnCerrar);

            // Botones anterior / siguiente
            btnAnterior = new Button
            {
                Text = "<",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                Size = new Size(50, 80),
                Location = new Point(20, this.ClientSize.Height / 2 - 40),
                Anchor = AnchorStyles.Left
            };
            btnAnterior.Click += BtnAnterior_Click;
            this.Controls.Add(btnAnterior);

            btnSiguiente = new Button
            {
                Text = ">",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                Size = new Size(50, 80),
                Location = new Point(this.ClientSize.Width - 70, this.ClientSize.Height / 2 - 40),
                Anchor = AnchorStyles.Right
            };
            btnSiguiente.Click += BtnSiguiente_Click;
            this.Controls.Add(btnSiguiente);

            // Botones de modo
            btnCascada = new Button
            {
                Text = "Cascada",
                ForeColor = Color.White,
                Location = new Point(20, 60),
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            btnCascada.Click += (s, e) => CambiarModo("Cascada");
            this.Controls.Add(btnCascada);

            btnLibro = new Button
            {
                Text = "Libro",
                ForeColor = Color.White,
                Location = new Point(130, 60),
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            btnLibro.Click += (s, e) => CambiarModo("Libro");
            this.Controls.Add(btnLibro);

            btnManga = new Button
            {
                Text = "Manga",
                ForeColor = Color.White,
                Location = new Point(240, 60),
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            btnManga.Click += (s, e) => CambiarModo("Manga");
            this.Controls.Add(btnManga);
        }

        private void CambiarModo(string modo)
        {
            modoActual = modo;
            this.Controls.Remove(visor);
            this.Controls.Remove(pdfRenderer);

            if (modo == "Cascada")
            {
                pdfRenderer = new PdfRenderer
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Black
                };
                pdfRenderer.Load(documentoPdf);
                this.Controls.Add(pdfRenderer);

                btnAnterior.Visible = false;
                btnSiguiente.Visible = false;
                lblContador.Visible = false;
            }
            else
            {
                visor = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Black
                };
                this.Controls.Add(visor);

                btnAnterior.Visible = true;
                btnSiguiente.Visible = true;
                lblContador.Visible = true;
                MostrarPagina();
            }
        }

        private void MostrarPagina()
        {
            if (paginaActual < 0 || paginaActual >= documentoPdf.PageCount) return;

            var img = documentoPdf.Render(paginaActual, this.ClientSize.Width, this.ClientSize.Height, true);

            // Solo rota si NO estás en modo manga (ya corregido)
            visor.Image?.Dispose();
            visor.Image = img;

            lblContador.Text = $"{paginaActual + 1}/{documentoPdf.PageCount}";
        }

        private void CambiarPagina(int delta)
        {
            int nuevaPagina = paginaActual;

            if (modoActual == "Manga")
                nuevaPagina -= delta; // invertido
            else
                nuevaPagina += delta;

            if (nuevaPagina >= 0 && nuevaPagina < documentoPdf.PageCount)
            {
                paginaActual = nuevaPagina;
                MostrarPagina();
            }
        }

        private void BtnAnterior_Click(object sender, EventArgs e)
        {
            CambiarPagina(-1);
        }

        private void BtnSiguiente_Click(object sender, EventArgs e)
        {
            CambiarPagina(1);
        }
    }
}
