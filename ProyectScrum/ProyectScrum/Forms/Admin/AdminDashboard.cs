using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using ProyectScrum.Data;

namespace ProyectScrum.Forms
{
    public partial class AdminDashboard : Form
    {
        private readonly SqlDataAccess _dataAccess;

        public AdminDashboard()
        {
            InitializeComponent();
            _dataAccess = new SqlDataAccess();
            LoadDashboardData();
            //SetupStatsPanels();
        }

        private void LoadDashboardData()
        {
            try
            {
                using (var conn = _dataAccess.GetConnection())
                {
                    conn.Open();

                    var cmdUsers = new SqlCommand(
                        "SELECT COUNT(*) AS TotalUsers, " +
                        "SUM(CASE WHEN EsPremium = 1 THEN 1 ELSE 0 END) AS PremiumUsers, " +
                        "SUM(CASE WHEN RolID = 1 THEN 1 ELSE 0 END) AS AdminUsers " +
                        "FROM Usuarios", conn);

                    using (var reader = cmdUsers.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblTotalUsers.Text = reader["TotalUsers"].ToString();
                            lblPremiumUsers.Text = reader["PremiumUsers"].ToString();
                            lblAdminUsers.Text = reader["AdminUsers"].ToString();
                        }
                    }

                    var cmdRecentUsers = new SqlCommand(
                        "SELECT TOP 5 NombreUsuario, Email, FechaRegistro " +
                        "FROM Usuarios ORDER BY FechaRegistro DESC", conn);

                    var adapter = new SqlDataAdapter(cmdRecentUsers);
                    var dtRecentUsers = new DataTable();
                    adapter.Fill(dtRecentUsers);

                    dataGridRecentUsers.DataSource = dtRecentUsers;
                    dataGridRecentUsers.Columns["NombreUsuario"].HeaderText = "Usuario";
                    dataGridRecentUsers.Columns["Email"].HeaderText = "Correo Electrónico";
                    dataGridRecentUsers.Columns["FechaRegistro"].HeaderText = "Fecha Registro";

                    var cmdRoles = new SqlCommand(
                        "SELECT r.Nombre, COUNT(u.UsuarioID) AS Cantidad " +
                        "FROM Usuarios u " +
                        "JOIN Roles r ON u.RolID = r.RolID " +
                        "GROUP BY r.Nombre", conn);

                    adapter = new SqlDataAdapter(cmdRoles);
                    var dtRoles = new DataTable();
                    adapter.Fill(dtRoles);

                    listRoles.Items.Clear();
                    foreach (DataRow row in dtRoles.Rows)
                    {
                        listRoles.Items.Add($"{row["Nombre"]}: {row["Cantidad"]} usuarios");
                    }

                    var cmdRegistrations = new SqlCommand(
                        "SELECT FORMAT(FechaRegistro, 'yyyy-MM') AS Mes, COUNT(*) AS Cantidad " +
                        "FROM Usuarios " +
                        "WHERE FechaRegistro >= DATEADD(MONTH, -6, GETDATE()) " +
                        "GROUP BY FORMAT(FechaRegistro, 'yyyy-MM') " +
                        "ORDER BY Mes", conn);

                    adapter = new SqlDataAdapter(cmdRegistrations);
                    var dtRegistrations = new DataTable();
                    adapter.Fill(dtRegistrations);

                    listRegistrations.Items.Clear();
                    foreach (DataRow row in dtRegistrations.Rows)
                    {
                        listRegistrations.Items.Add($"{row["Mes"]}: {row["Cantidad"]} registros");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del dashboard: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupStatsPanels()
        {
            panelTotalUsers.BackColor = Color.FromArgb(46, 79, 102);
            panelPremiumUsers.BackColor = Color.FromArgb(46, 79, 102);
            panelAdminUsers.BackColor = Color.FromArgb(46, 79, 102);

            listRoles.BackColor = Color.FromArgb(34, 60, 80);
            listRoles.ForeColor = Color.White;

            listRegistrations.BackColor = Color.FromArgb(34, 60, 80);
            listRegistrations.ForeColor = Color.White;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }
    }
}