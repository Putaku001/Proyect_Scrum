using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using ProyectScrum.Data;

namespace ProyectScrum.Forms
{
    public partial class AdminAddUser : Form
    {
        private readonly SqlDataAccess _dataAccess;

        public AdminAddUser()
        {
            InitializeComponent();
            _dataAccess = new SqlDataAccess();
        }

        private void LabelSuscripcion()
        {
            if (chkPremium.Checked)
            {
                labelEsPremium.Text = "Premium";
                labelEsPremium.ForeColor = System.Drawing.Color.Green;

            }
            else
            {
                labelEsPremium.Text = "No Premium";
                labelEsPremium.ForeColor = System.Drawing.Color.Red;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string hashedPassword = HashPassword(txtPassword.Text);

                using (var conn = _dataAccess.GetConnection())
                {
                    conn.Open();
                    var cmd = new SqlCommand("INSERT INTO Usuarios (NombreUsuario, Email, ContrasenaHash, EsPremium, RolID) VALUES (@NombreUsuario, @Email, @ContrasenaHash, @EsPremium, @RolID)", conn);

                    cmd.Parameters.AddWithValue("@NombreUsuario", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@ContrasenaHash", hashedPassword);
                    cmd.Parameters.AddWithValue("@EsPremium", chkPremium.Checked);
                    cmd.Parameters.AddWithValue("@RolID", cmbRole.SelectedIndex + 1);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Usuario agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                MessageBox.Show("El nombre de usuario o email ya existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkPremium_CheckedChanged(object sender, EventArgs e)
        {
            LabelSuscripcion();
        }
    }
}