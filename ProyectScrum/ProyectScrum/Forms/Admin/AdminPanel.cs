using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using ProyectScrum.Data;
using ProyectScrum.Entities;

namespace ProyectScrum.Forms
{
    public partial class AdminPanel : Form
    {
        private readonly EmailSettings _emailSettings;
        private SqlDataAccess _dataAccess;

        public AdminPanel(EmailSettings emailSettings)
        {
            InitializeComponent();
            _emailSettings = emailSettings;
            _dataAccess = new SqlDataAccess();
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                using (var conn = _dataAccess.GetConnection())
                {
                    conn.Open();
                    var cmd = new SqlCommand("SELECT UsuarioID, NombreUsuario, Email, EsPremium, RolID FROM Usuarios", conn);
                    var adapter = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridViewUsers.DataSource = dt;
                    dataGridViewUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewUsers.SelectedRows[0];
                int userId = Convert.ToInt32(selectedRow.Cells["UsuarioID"].Value);

                EditUserForm editForm = new EditUserForm(userId);
                editForm.ShowDialog();
                LoadUsers(); 
            }
            else
            {
                MessageBox.Show("Por favor seleccione un usuario para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show("¿Está seguro que desea eliminar este usuario?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var selectedRow = dataGridViewUsers.SelectedRows[0];
                    int userId = Convert.ToInt32(selectedRow.Cells["UsuarioID"].Value);

                    try
                    {
                        using (var conn = _dataAccess.GetConnection())
                        {
                            conn.Open();
                            var cmd = new SqlCommand("DELETE FROM Usuarios WHERE UsuarioID = @UsuarioID", conn);
                            cmd.Parameters.AddWithValue("@UsuarioID", userId);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Usuario eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadUsers();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor seleccione un usuario para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            Register registerForm = new Register();
            registerForm.ShowDialog();
            LoadUsers(); 
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
