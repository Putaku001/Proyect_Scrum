using ProyectScrum.Entities;
using ProyectScrum.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectScrum
{
    public partial class Main : Form
    {
        public EmailSettings _emailSettings;
        public Catalog catalogForm;
        public Perfil perfilForm;
        public favs favsForm;
        public int UsuarioID { get; set; }

        bool slideBarExpand;

        public Main(EmailSettings emailSettings)
        {
            InitializeComponent();

            _emailSettings = emailSettings;
            UsuarioID = CapturedData.UsuarioID;
            perfilForm = new Perfil(_emailSettings);
            // Mostrar inicioForm por defecto al iniciar
            AbrirFormularioEnPanel(new inicioForm());
        }

        private void perfilButton_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(perfilForm);
        }

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

        private void menuButton_Click(object sender, EventArgs e)
        {
            slideBarTime.Start();
        }

        public void AbrirFormularioEnPanel(Form formularioHijo)
        {
            if (panelContenedor.Controls.Count > 0)
                panelContenedor.Controls.RemoveAt(0);

            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;

            if (formularioHijo is mangaForm manga)
            {
                manga.FavoritoAgregado += Manga_FavoritoAgregado;
            }

            panelContenedor.Controls.Add(formularioHijo);
            panelContenedor.Tag = formularioHijo;
            formularioHijo.Show();
        }

        private void Manga_FavoritoAgregado(object sender, EventArgs e)
        {
            if (favsForm != null && !favsForm.IsDisposed && favsForm.Visible)
            {
                favsForm.CargarFavoritos(); // Refrescar favoritos en tiempo real
            }
        }

        private void catalogbtn_Click(object sender, EventArgs e)
        {
            if (catalogForm == null || catalogForm.IsDisposed)
                catalogForm = new Catalog(_emailSettings);

            AbrirFormularioEnPanel(catalogForm);
        }

        private void cerrarSesionButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Estás seguro que deseas cerrar sesión?", "Cerrar sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                Login loginForm = new Login();
                loginForm.FormClosed += (s, args) => this.Close();
                loginForm.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (favsForm == null || favsForm.IsDisposed)
                favsForm = new favs(UsuarioID);

            AbrirFormularioEnPanel(favsForm);
        }
    }
}
