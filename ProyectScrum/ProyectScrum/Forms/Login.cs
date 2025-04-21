using Microsoft.VisualBasic.ApplicationServices;
using ProyectScrum.Data;
using ProyectScrum.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ProyectScrum.Forms.customControl;

namespace ProyectScrum.Forms
{
    public partial class Login : Form
    {
        private readonly EmailSettings _emailSettings = new EmailSettings();
        private DateTime _codeTime;
        private int usuarioID;
        public Login()
        {
            InitializeComponent();
            _codeTime = DateTime.Now;

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
            var usuario = ValidarUsuario(txtUsuario.Text, txtContrasena.Text);


            if (usuario != null)
            {
                usuarioID = usuario.UsuarioID;
                CapturedData.UsuarioID = usuario.UsuarioID;
                CapturedData.NombreUsuario = usuario.NombreUsuario;
                CapturedData.Email = usuario.Email;
                CapturedData.EsPremium = usuario.EsPremium;
                CapturedData.RolID = usuario.RolID;
                CapturedData.Avatar = usuario.Avatar;

                _emailSettings.EmailDestino = usuario.Email;

                Main mainForm = new Main( _emailSettings);
                mainForm.FormClosed += MainForm_FormClosed;
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.");
            }
        }
        private Users ValidarUsuario(string usuario, string contrasena)
        {
            var dataAccess = new SqlDataAccess();
            using (var conn = dataAccess.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT UsuarioID, Avatar, NombreUsuario, Email, ContrasenaHash, EsPremium, RolID FROM Usuarios WHERE NombreUsuario = @Usuario", conn);
                cmd.Parameters.AddWithValue("@Usuario", usuario);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string hashedPassword = reader["ContrasenaHash"].ToString();

                    if (VerificarContrasena(contrasena, hashedPassword))
                    {
                        return new Users
                        {
                            UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                            Avatar = reader["Avatar"].ToString(),
                            NombreUsuario = reader["NombreUsuario"].ToString(),
                            Email = reader["Email"].ToString(),
                            EsPremium = Convert.ToBoolean(reader["EsPremium"]),
                            RolID = Convert.ToInt32(reader["RolID"]),
                        };
                    }
                }
                return null;
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

        private string GenerateCodeVerification()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            _emailSettings.codigoVerificacion = GenerateCodeVerification();

            var dataAccess = new SqlDataAccess();
            using (var connection = dataAccess.GetConnection())
            {
                connection.Open();

                var cmd = new SqlCommand("SELECT Email FROM Usuarios WHERE NombreUsuario = @NombreUsuario", connection);
                cmd.Parameters.AddWithValue("@NombreUsuario", txtUsuario.Text);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        _emailSettings.EmailDestino = reader["Email"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Usuario no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                try
                {
                    MailMessage mailMessage = new MailMessage(
                        _emailSettings.EmailOrigen,
                        _emailSettings.EmailDestino,
                        "Código de Verificación",
                        $@"
                        <p>Estimado/a {_emailSettings.EmailDestino},</p>

                        <p>Hemos recibido una solicitud para restablecer la contraseña de tu cuenta. Para completar el proceso de recuperación de contraseña, utiliza el siguiente código de verificación:</p>

                        <p style='font-size: 18px; color: #2E86C1;'><b>{_emailSettings.codigoVerificacion}</b></p>

                        <p>Por favor, introduce este código en la pantalla de verificación para restablecer tu contraseña y acceder nuevamente a tu cuenta. Este código es válido solo por un período limitado, por lo que te recomendamos que completes el proceso lo antes posible.</p>

                        <p>Si no solicitaste restablecer tu contraseña, puedes ignorar este mensaje. La seguridad de tu cuenta es muy importante para nosotros, y ningún cambio será realizado sin tu autorización.</p>

                        <p>Atentamente,</p>
                        <p>Equipo de Soporte LosTilinazos77</p>

                        <p style='font-size: 12px; color: #888;'>Este es un mensaje automático, por favor no respondas a este correo.</p>
                        "
                    );

                    mailMessage.IsBodyHtml = true;

                    SmtpClient smtpClient = new SmtpClient("smpt.gmail.com");
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new System.Net.NetworkCredential(_emailSettings.EmailOrigen, _emailSettings.Contraseña);

                    _emailSettings.time = _codeTime;

                    smtpClient.Send(mailMessage);

                    smtpClient.Dispose();

                    MessageBox.Show("Se ha enviado el código de verificación a su correo.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al enviar el correo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            FPassword FPassForm = new FPassword(_emailSettings);
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
            path.AddArc(rect.X, rect.Y, Radius, Radius, 180, 90);
            path.AddArc(rect.Width - Radius, rect.Y, Radius, Radius, 270, 90);
            path.AddArc(rect.Width - Radius, rect.Height - Radius, Radius, Radius, 0, 90);
            path.AddArc(rect.X, rect.Height - Radius, Radius, Radius, 90, 90);
            path.CloseFigure();

            return path;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rectSurface = new RectangleF(0, 0, this.Width, this.Height);
            RectangleF rectBorder = new RectangleF(1, 1, this.Width - 0.8F, this.Height - 1);

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
                        pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
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