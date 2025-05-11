using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using ProyectScrum.Data;

namespace ProyectScrum.Forms
{
    public partial class AdminUsersList : Form
    {
        private readonly SqlDataAccess _dataAccess;

        public AdminUsersList()
        {
            InitializeComponent();
            _dataAccess = new SqlDataAccess();
            LoadUsers();
            LoadButtons();
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
                    dataGridViewUsers.Columns["UsuarioID"].Visible = false;

                    dataGridViewUsers.Columns["NombreUsuario"].HeaderText = "Usuario";
                    dataGridViewUsers.Columns["Email"].HeaderText = "Correo Electrónico";
                    dataGridViewUsers.Columns["EsPremium"].HeaderText = "Premium";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadButtons()
        {
            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
            btnEdit.Name = "Edit";
            btnEdit.Text = "Editar";
            btnEdit.UseColumnTextForButtonValue = true;
            dataGridViewUsers.Columns.Add(btnEdit);

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.Name = "Delete";
            btnDelete.Text = "Eliminar";
            btnDelete.UseColumnTextForButtonValue = true;
            dataGridViewUsers.Columns.Add(btnDelete);
        }

        private void dataGridViewUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dataGridViewUsers.Columns[e.ColumnIndex].Name == "Edit")
                {
                    int userId = Convert.ToInt32(dataGridViewUsers.Rows[e.RowIndex].Cells["UsuarioID"].Value);
                    EditUserForm editForm = new EditUserForm(userId);
                    editForm.ShowDialog();
                    LoadUsers();
                }
                else if (dataGridViewUsers.Columns[e.ColumnIndex].Name == "Delete")
                {
                    int userId = Convert.ToInt32(dataGridViewUsers.Rows[e.RowIndex].Cells["UsuarioID"].Value);
                    string userName = dataGridViewUsers.Rows[e.RowIndex].Cells["NombreUsuario"].Value.ToString();

                    var result = MessageBox.Show($"¿Está seguro que desea eliminar al usuario {userName}?",
                        "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        DeleteUser(userId);
                    }
                }
            }
        }

        private void DeleteUser(int userId)
        {
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }
    }
}