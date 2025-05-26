using ProyectScrum.Entities;
using ProyectScrum.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectScrum
{
    public partial class Main : Form
    {
        public EmailSettings _emailSettings;

        // Referencias de formularios que reutilizaremos
        public Catalog catalogForm;
        public Perfil perfilForm;
        public favs favsForm;
        public inicioForm inicioForm;   // ⬅️ NUEVO

        public int UsuarioID { get; set; }

        private bool slideBarExpand;

        public Main(EmailSettings emailSettings)
        {
            InitializeComponent();

            _emailSettings = emailSettings;
            UsuarioID = CapturedData.UsuarioID;

            // Instanciamos una sola vez los formularios que se reutilizarán
            perfilForm = new Perfil(_emailSettings);
            inicioForm = new inicioForm();          // ⬅️ NUEVO

            // Al iniciar la aplicación se muestra el inicio
            AbrirFormularioEnPanel(inicioForm);
        }

        /* ---------- Botones del sidebar ---------- */

        private void menuButton_Click(object sender, EventArgs e)
        {
            // Muestra el formulario de inicio cada vez que se presiona el logo / botón "Inicio"
            if (inicioForm == null || inicioForm.IsDisposed)
                inicioForm = new inicioForm();

            AbrirFormularioEnPanel(inicioForm);
        }

        private void catalogbtn_Click(object sender, EventArgs e)
        {
            if (catalogForm == null || catalogForm.IsDisposed)
                catalogForm = new Catalog(_emailSettings);

            AbrirFormularioEnPanel(catalogForm);
        }

        private void perfilButton_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(perfilForm);
        }

        private void button1_Click(object sender, EventArgs e) // botón de Favoritos
        {
            if (favsForm == null || favsForm.IsDisposed)
                favsForm = new favs(UsuarioID);

            AbrirFormularioEnPanel(favsForm);
        }

        private void cerrarSesionButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "¿Estás seguro que deseas cerrar sesión?",
                "Cerrar sesión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Hide();
                Login loginForm = new Login();
                loginForm.FormClosed += (s, args) => Close();
                loginForm.Show();
            }
        }

        /* ---------- Animación del SlideBar ---------- */

        private void slideBarTime_Tick(object sender, EventArgs e)
        {
            if (slideBarExpand)
            {
                SlideBar.Width -= 10;
                if (SlideBar.Width == SlideBar.MinimumSize.Width)
                {
                    slideBarExpand = false;
                    slideBarTime.Stop();
                }
            }
            else
            {
                SlideBar.Width += 10;
                if (SlideBar.Width == SlideBar.MaximumSize.Width)
                {
                    slideBarExpand = true;
                    slideBarTime.Stop();
                }
            }
        }

        /* ---------- Gestión interna de formularios ---------- */

        public void AbrirFormularioEnPanel(Form formularioHijo)
        {
            if (panelContenedor.Controls.Count > 0)
                panelContenedor.Controls.RemoveAt(0);

            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;

            // Hook de eventos si el hijo es un mangaForm
            if (formularioHijo is mangaForm manga)
                manga.FavoritoAgregado += Manga_FavoritoAgregado;

            panelContenedor.Controls.Add(formularioHijo);
            panelContenedor.Tag = formularioHijo;
            formularioHijo.Show();
        }

        private void Manga_FavoritoAgregado(object sender, EventArgs e)
        {
            // Refresca favoritos en tiempo real si el formulario está visible
            if (favsForm != null && !favsForm.IsDisposed && favsForm.Visible)
                favsForm.CargarFavoritos();
        }
    }
}
