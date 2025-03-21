using ProyectScrum.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectScrum.Forms
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void inicioSLinkLabel_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (RegistrarUsuario(txtNombreUsuario.Text, txtEmail.Text, txtContrasena.Text))
            {
                MessageBox.Show("Usuario registrado exitosamente!");
                Login loginForm = new Login();
                loginForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Error al registrar el usuario.");
            }
        }
        private bool RegistrarUsuario(string nombreUsuario, string email, string contrasena)
        {
            try
            {
                var dataAccess = new SqlDataAccess();
                using (var conn = dataAccess.GetConnection())
                {
                    conn.Open();
                    var cmd = new SqlCommand("INSERT INTO Usuarios (NombreUsuario, Email, ContrasenaHash, RolID) VALUES (@NombreUsuario, @Email, @ContrasenaHash, @RolID)", conn);
                    cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@ContrasenaHash", HashPassword(contrasena));
                    cmd.Parameters.AddWithValue("@RolID", 1); // 1 es el RolID de usuario
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}
