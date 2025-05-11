using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using ProyectScrum.Data;
using ProyectScrum.Entities;
using ProyectScrum.Forms;

namespace ProyectScrum.Forms
{
    public partial class MainAdminLayout : Form
    {
        private Form currentChildForm;
        private Button currentButton;
        private readonly EmailSettings _emailSettings;
        private List<byte[]> avataresDisponibles;
        private int avatarSeleccionadoIndex = 0;
        private byte[] _avatarSeleccionadoBytes;

        public MainAdminLayout(EmailSettings emailSettings)
        {
            InitializeComponent();
            _emailSettings = emailSettings;
            CustomizeDesign();
            CargarAvatarUsuario();
            LoadDataUser();
        }

        private void CargarAvatarUsuario()
        {
            var dataAccess = new SqlDataAccess();
            using (var conn = dataAccess.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT Avatar FROM Usuarios WHERE UsuarioID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", CapturedData.UsuarioID);

                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    byte[] avatarBytes = (byte[])result;
                    _avatarSeleccionadoBytes = avatarBytes;

                    using (var ms = new MemoryStream(avatarBytes))
                    {
                        pictureBoxAvatar.Image = Image.FromStream(ms);
                        pictureBoxAvatar.AccessibleName = "AvatarUsuario";
                    }
                }
                else
                {
                    MostrarAvatar(0);
                }
            }
        }

        private void MostrarAvatar(int index)
        {
            if (index >= 0 && index < avataresDisponibles.Count)
            {
                avatarSeleccionadoIndex = index;
                _avatarSeleccionadoBytes = avataresDisponibles[index];

                using (var ms = new MemoryStream(avataresDisponibles[index]))
                {
                    pictureBoxAvatar.Image = Image.FromStream(ms);
                }
            }
        }


        private void LoadDataUser()
        {
            labelWelcome.Text = $"Bienvenido, {CapturedData.NombreUsuario}";
        }


        private void CustomizeDesign()
        {
            panelUsersSubmenu.Visible = false;
            panelReportsSubmenu.Visible = false;
        }

        private void HideSubmenu()
        {
            if (panelUsersSubmenu.Visible)
                panelUsersSubmenu.Visible = false;
            if (panelReportsSubmenu.Visible)
                panelReportsSubmenu.Visible = false;
        }

        private void ShowSubmenu(Panel subMenu)
        {
            if (!subMenu.Visible)
            {
                HideSubmenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private void OpenChildForm(Form childForm)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }

            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitle.Text = childForm.Text;
        }

        private void ActivateButton(object senderBtn)
        {
            if (senderBtn != null)
            {
                DisableButton();
                currentButton = (Button)senderBtn;
                currentButton.Font = new Font(currentButton.Font, FontStyle.Bold);
            }
        }

        private void DisableButton()
        {
            if (currentButton != null)
            {
                currentButton.ForeColor = Color.Gainsboro;
                currentButton.Font = new Font(currentButton.Font, FontStyle.Regular);
            }
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            ShowSubmenu(panelUsersSubmenu);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildForm(new AdminDashboard());
            HideSubmenu();
        }

        private void btnListUsers_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildForm(new AdminUsersList());
            HideSubmenu();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildForm(new AdminAddUser());
            HideSubmenu();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login loginForm = new Login();
            loginForm.FormClosed += (s, args) => this.Close();
            loginForm.Show();
        }
        private void MainAdminLayout_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            Login login = new Login();
            login.Show();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildForm(new Perfil(_emailSettings));
            HideSubmenu();
        }
    }
}
