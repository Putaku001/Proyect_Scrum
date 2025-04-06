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
        public Catalog catalogForm;
       

        bool slideBarExpand;
        public Main()
        {
            InitializeComponent();

        }

        private void perfilButton_Click(object sender, EventArgs e)
        {

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
            // Limpia contenido anterior
            if (panelContenedor.Controls.Count > 0)
                panelContenedor.Controls.RemoveAt(0);

            // Ajusta el nuevo formulario
            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;

            panelContenedor.Controls.Add(formularioHijo);
            panelContenedor.Tag = formularioHijo;

            formularioHijo.Show();
        }

        private void catalogbtn_Click(object sender, EventArgs e)
        {
            if (catalogForm == null || catalogForm.IsDisposed)
                catalogForm = new Catalog();

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
    }
}
