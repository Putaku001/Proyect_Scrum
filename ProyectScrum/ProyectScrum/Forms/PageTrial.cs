using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Guna.UI2.WinForms.Suite;
using ProyectScrum.Forms;


namespace ReDiseño
{
    public partial class PageTrial : System.Windows.Forms.Form
    {
        private List<Image> imagenes = new List<Image>();
        //private List<string> descripciones = new List<string>();
        private int indiceActual = 0;
        public PageTrial()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi;
        }
        private void ISButton_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.FormClosed += LoginForm_FormClosed;
            loginForm.Show();
            this.Hide();
        }
        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }
        private void CargarImagenes()
        {
            // Puedes cargar desde recursos, archivos o URLs.
            

            imagenes.Add(Image.FromFile("Recursos\\img\\koe no katachi.jpg"));
            

            imagenes.Add(Image.FromFile("Recursos\\img\\ruri-dragon-vol1.jpg"));

            imagenes.Add(Image.FromFile("Recursos\\img\\KS.jpg"));

            imagenes.Add(Image.FromFile("Recursos\\img\\Kurosawa.jpg"));

            imagenes.Add(Image.FromFile("Recursos\\img\\yourname.jpg"));

        }

        private void MostrarImagenActual()
        {
            guna2PictureBox1.Image = imagenes[indiceActual];
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            indiceActual = (indiceActual + 1) % imagenes.Count;
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            indiceActual = (indiceActual + 1) % imagenes.Count;
            timer1.Start();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            indiceActual = (indiceActual - 1 + imagenes.Count) % imagenes.Count;
            timer1.Start();
        }
    }
}
