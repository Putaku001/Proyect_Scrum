using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using ProyectScrum.Data;
using ProyectScrum.Entities;

namespace ProyectScrum.Forms
{
    public partial class EditUserForm : Form
    {
        private readonly int _userId;
        private readonly SqlDataAccess _dataAccess;

        public EditUserForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
            _dataAccess = new SqlDataAccess();
            LoadUserData();
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

        private void LoadUserData()
        {
            try
            {
                using (var conn = _dataAccess.GetConnection())
                {
                    conn.Open();
                    var cmd = new SqlCommand("SELECT NombreUsuario, Email, EsPremium, RolID FROM Usuarios WHERE UsuarioID = @UsuarioID", conn);
                    cmd.Parameters.AddWithValue("@UsuarioID", _userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtUsername.Text = reader["NombreUsuario"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                            chkPremium.Checked = Convert.ToBoolean(reader["EsPremium"]);
                            cmbRole.SelectedIndex = Convert.ToInt32(reader["RolID"]) - 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = _dataAccess.GetConnection())
                {
                    conn.Open();
                    var cmd = new SqlCommand("UPDATE Usuarios SET NombreUsuario = @NombreUsuario, Email = @Email, EsPremium = @EsPremium, RolID = @RolID WHERE UsuarioID = @UsuarioID", conn);

                    cmd.Parameters.AddWithValue("@NombreUsuario", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@EsPremium", chkPremium.Checked);
                    cmd.Parameters.AddWithValue("@RolID", cmbRole.SelectedIndex + 1);
                    cmd.Parameters.AddWithValue("@UsuarioID", _userId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Usuario actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkPremium_CheckedChanged(object sender, EventArgs e)
        {
            LabelSuscripcion();
        }
    }
}