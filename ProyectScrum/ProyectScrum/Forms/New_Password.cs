using ProyectScrum.Data;
using ProyectScrum.Entities;
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
    public partial class New_Password : Form
    {
        private readonly EmailSettings _emailSettings;
        public New_Password(EmailSettings emailSettings)
        {
            InitializeComponent();
            _emailSettings = emailSettings;
        }

        private void customControl1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == textBox2.Text)
            {
                if (string.IsNullOrEmpty(textBox1.Text) || textBox1.Text.Length < 8)
                {
                    MessageBox.Show("La contraseña debe tener al menos 8 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dataAccess = new SqlDataAccess();
                using (var connection = dataAccess.GetConnection())
                {
                    try
                    {
                        connection.Open();

                        var query = new SqlCommand("UPDATE Usuarios SET ContrasenaHash = @ContrasenaHash WHERE Email = @Email", connection);
                        query.Parameters.AddWithValue("@ContrasenaHash", textBox1.Text);
                        query.Parameters.AddWithValue("@Email", _emailSettings.EmailDestino.ToString());

                        int rowsAffected = query.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Contraseña actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                            Login login = new Login();
                            login.Show();
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el usuario especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar la contraseña: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}