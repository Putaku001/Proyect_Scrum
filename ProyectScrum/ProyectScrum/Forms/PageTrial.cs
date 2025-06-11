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
            CargarImagenes();
            MostrarImagenActual();

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
            // Subimos desde bin\Debug\net8.0-windows hasta el proyecto raíz
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Recursos\img");
            basePath = Path.GetFullPath(basePath); // Resuelve correctamente la ruta

            string[] archivos = {
    "koe no katachi.jpg",
    "ruri-dragon-vol1.jpg",
    "KS.jpg",
    "Kurosawa.jpg",
    "yourname.jpg"
};

            foreach (var archivo in archivos)
            {
                string rutaCompleta = Path.Combine(basePath, archivo);
                if (File.Exists(rutaCompleta))
                {
                    imagenes.Add(Image.FromFile(rutaCompleta));
                }
                else
                {
                    MessageBox.Show($"Imagen no encontrada: {rutaCompleta}");
                }
            }
        }



        private void MostrarImagenActual()
        {
            if (imagenes.Count > 0)
            {
                guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                guna2PictureBox1.Image = imagenes[indiceActual];
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            indiceActual = (indiceActual + 1) % imagenes.Count;
            MostrarImagenActual();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            indiceActual = (indiceActual + 1) % imagenes.Count;
            MostrarImagenActual();
            timer1.Start();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            indiceActual = (indiceActual - 1 + imagenes.Count) % imagenes.Count;
            MostrarImagenActual();
            timer1.Start();
        }
    }
}
