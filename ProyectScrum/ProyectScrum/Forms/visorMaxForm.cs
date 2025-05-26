using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PdfiumViewer;
using ProyectScrum.Data;
using ProyectScrum.Entities;

namespace ProyectScrum.Forms
{
    public partial class visorMaxForm : Form
    {
        private TableLayoutPanel layoutPrincipal;

        private PdfiumViewer.PdfDocument documentoPdf;
        private int paginaActual;
        private string modoActual;
        private int mangaID;

        private DateTime tiempoInicioLectura;
        private int TiempoLecturaTotal => (int)(DateTime.Now - tiempoInicioLectura).TotalMinutes;

        private PictureBox visor;
        private Label lblContador;
        private Button btnAnterior;
        private Button btnSiguiente;
        private Button btnCerrar;
        private Button btnLibro;
        private Button btnManga;
        private Button btnCascada;
        private PdfRenderer pdfRenderer;


        public visorMaxForm(PdfiumViewer.PdfDocument document, int paginaInicial, string modo, int mangaID)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Black;

            documentoPdf = document;
            paginaActual = paginaInicial;
            modoActual = modo;
            this.mangaID = mangaID;

            
            CrearControles();         
            CambiarModo(modo);

            tiempoInicioLectura = DateTime.Now;
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
            panelContenedor.Controls.Add(lblContador);

            // Botón cerrar
            btnCerrar = new Button
            {
                Text = "Cerrar",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(200, 40, 40), // Rojo oscuro bonito
                FlatStyle = FlatStyle.Flat,
                Width = 70,
                Height = 30,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 50, 50);

            // Posición al extremo superior derecho con margen
            btnCerrar.Location = new Point(panelContenedor.Width - btnCerrar.Width - 20, 10);
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            btnCerrar.Click += (s, e) => this.Close();
            panelContenedor.Controls.Add(btnCerrar);


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
            panelContenedor.Controls.Add(btnAnterior);

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
            panelContenedor.Controls.Add(btnSiguiente);

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
            panelContenedor.Controls.Add(btnCascada);

            btnLibro = new Button
            {
                Text = "Libro",
                ForeColor = Color.White,
                Location = new Point(130, 60),
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            btnLibro.Click += (s, e) => CambiarModo("Libro");
            panelContenedor.Controls.Add(btnLibro);

            btnManga = new Button
            {
                Text = "Manga",
                ForeColor = Color.White,
                Location = new Point(240, 60),
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            btnManga.Click += (s, e) => CambiarModo("Manga");
            panelContenedor.Controls.Add(btnManga);
        }


        private void CambiarModo(string modo)
        {
            modoActual = modo;
            panelContenedor.Controls.Remove(visor);
            panelContenedor.Controls.Remove(pdfRenderer);

            if (modo == "Cascada")
            {
                pdfRenderer = new PdfRenderer
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Black
                };
                pdfRenderer.Load(documentoPdf);
                panelContenedor.Controls.Add(pdfRenderer);

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
                panelContenedor.Controls.Add(visor);

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

        //metodos de guardado de pagina 

        public event Action<int> PaginaCerrada;

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            PaginaCerrada?.Invoke(paginaActual); // Notificamos la última página
            GuardarProgresoLectura(CapturedData.UsuarioID, mangaID, paginaActual, TiempoLecturaTotal); //para guardarla en la db para volver a leer
            base.OnFormClosed(e);
        }

        //metodo para guardar en la db
        private void GuardarProgresoLectura(int usuarioId, int mangaId, int paginaActual, int tiempoMinutos)
        {
            if (usuarioId <= 0 || mangaId <= 0) return;

            SqlDataAccess db = new SqlDataAccess();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
            IF EXISTS (
                SELECT 1 FROM ProgresoLectura 
                WHERE UsuarioID = @UsuarioID AND MangaID = @MangaID
            )
            BEGIN
                UPDATE ProgresoLectura
                SET PaginaActual = @PaginaActual,
                    TiempoLecturaTotal = TiempoLecturaTotal + @Tiempo,
                    Status = 'Pausado',
                    FechaUltimaLectura = GETDATE(),
                    VecesLeido = VecesLeido + 1
                WHERE UsuarioID = @UsuarioID AND MangaID = @MangaID
            END
            ELSE
            BEGIN
                INSERT INTO ProgresoLectura (UsuarioID, MangaID, PaginaActual, TiempoLecturaTotal, Status, FechaUltimaLectura, VecesLeido)
                VALUES (@UsuarioID, @MangaID, @PaginaActual, @Tiempo, 'Leyendo', GETDATE(), 1)
            END";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioID", usuarioId);
                cmd.Parameters.AddWithValue("@MangaID", mangaId);
                cmd.Parameters.AddWithValue("@PaginaActual", paginaActual);
                cmd.Parameters.AddWithValue("@Tiempo", tiempoMinutos);

                cmd.ExecuteNonQuery();
            }
        }



    }

}
