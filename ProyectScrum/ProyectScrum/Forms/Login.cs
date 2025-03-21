using ProyectScrum.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectScrum.Forms
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void registerLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register registerForm = new Register();
            registerForm.FormClosed += RegisterForm_FormClosed;
            registerForm.Show();
            this.Hide();
        }

        private void RegisterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }


        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            if (ValidarUsuario(txtUsuario.Text, txtContrasena.Text))
            {
                Main mainForm = new Main();
                mainForm.FormClosed += MainForm_FormClosed;
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.");
            }
        }
        private bool ValidarUsuario(string usuario, string contrasena)
        {
            var dataAccess = new SqlDataAccess();
            using (var conn = dataAccess.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT ContrasenaHash FROM Usuarios WHERE NombreUsuario = @Usuario", conn);
                cmd.Parameters.AddWithValue("@Usuario", usuario);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string hashedPassword = reader["ContrasenaHash"].ToString();
                    return VerificarContrasena(contrasena, hashedPassword);
                }
                else
                {
                    return false;
                }
            }
        }
        private bool VerificarContrasena(string contrasena, string hashedPassword)
        {

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedInputPwd = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasena));
                var hashInputPwdString = BitConverter.ToString(hashedInputPwd).Replace("-", "").ToLower();
                return hashInputPwdString == hashedPassword;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FPassword FPassForm = new FPassword();
            FPassForm.FormClosed += FPassForm_FormClosed;
            FPassForm.Show();
            this.Hide();
        }

        private void FPassForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }

    //boton redondeado
    public class customControl : Button
    {
        //field
        private int borderSize = 0;
        private int borderRadius = 20;
        private Color borderColor = Color.Transparent;

        //constructor
        public customControl()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackColor = Color.Transparent;
            this.ForeColor = Color.White;

        }

        //methods
        private GraphicsPath GetFiguePath(RectangleF rect, float Radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, Radius, Radius, 180,90);
            path.AddArc(rect.Width-Radius, rect.Y, Radius, Radius, 270,90);
            path.AddArc(rect.Width - Radius, rect.Height-Radius, Radius, Radius, 0, 90);
            path.AddArc(rect.X, rect.Height-Radius, Radius, Radius, 90, 90);
            path.CloseFigure();

            return path;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rectSurface = new RectangleF(0,0,this.Width, this.Height);
            RectangleF rectBorder = new RectangleF(1,1, this.Width-0.8F, this.Height-1);

            if (borderRadius > 2)
            {
                using (GraphicsPath pathSurface = GetFiguePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFiguePath(rectBorder, borderRadius - 1F))
                using (Pen penSurface = new Pen(this.Parent.BackColor, 2))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    penBorder.Alignment = PenAlignment.Inset;
                    this.Region = new Region(pathSurface);
                    pevent.Graphics.DrawPath(penSurface, pathSurface);
                    if (borderSize >= 1)

                        pevent.Graphics.DrawPath(penBorder, pathBorder);

                }

            }
            else
            {
                this.Region = new Region(rectSurface);
                if (borderSize >= 1)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penBorder, 0,0,this.Width-1, this.Height-1);
                    }
                }
            }
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }
        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                this.Invalidate();
            }
        }
    }
}
