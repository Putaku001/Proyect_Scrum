namespace ProyectScrum.Forms
{
    partial class MainAdminLayout
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainAdminLayout));
            panelSidebar = new Panel();
            btnAdminManga = new Button();
            btnProfile = new Button();
            panelReportsSubmenu = new Panel();
            btnUserReports = new Button();
            btnActivityReports = new Button();
            panelUsersSubmenu = new Panel();
            btnAddUser = new Button();
            btnListUsers = new Button();
            btnUsers = new Button();
            btnDashboard = new Button();
            panelLogo = new Panel();
            pictureBoxAvatar = new PictureBox();
            labelWelcome = new Label();
            btnLogout = new Button();
            panelTitleBar = new Panel();
            panel1 = new Panel();
            lblTitle = new Label();
            panelDesktop = new Panel();
            panelSidebar.SuspendLayout();
            panelReportsSubmenu.SuspendLayout();
            panelUsersSubmenu.SuspendLayout();
            panelLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAvatar).BeginInit();
            panelTitleBar.SuspendLayout();
            SuspendLayout();
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = Color.FromArgb(64, 64, 64);
            panelSidebar.Controls.Add(btnAdminManga);
            panelSidebar.Controls.Add(btnProfile);
            panelSidebar.Controls.Add(panelReportsSubmenu);
            panelSidebar.Controls.Add(panelUsersSubmenu);
            panelSidebar.Controls.Add(btnUsers);
            panelSidebar.Controls.Add(btnDashboard);
            panelSidebar.Controls.Add(panelLogo);
            panelSidebar.Controls.Add(btnLogout);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Margin = new Padding(4);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Size = new Size(200, 750);
            panelSidebar.TabIndex = 0;
            // 
            // btnAdminManga
            // 
            btnAdminManga.Dock = DockStyle.Top;
            btnAdminManga.FlatAppearance.BorderSize = 0;
            btnAdminManga.FlatStyle = FlatStyle.Flat;
            btnAdminManga.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnAdminManga.ForeColor = Color.White;
            btnAdminManga.Image = (Image)resources.GetObject("btnAdminManga.Image");
            btnAdminManga.ImageAlign = ContentAlignment.MiddleLeft;
            btnAdminManga.Location = new Point(0, 533);
            btnAdminManga.Margin = new Padding(4);
            btnAdminManga.Name = "btnAdminManga";
            btnAdminManga.Padding = new Padding(11, 0, 0, 0);
            btnAdminManga.Size = new Size(200, 65);
            btnAdminManga.TabIndex = 8;
            btnAdminManga.Text = "Administar Manga";
            btnAdminManga.UseVisualStyleBackColor = true;
            btnAdminManga.Click += btnAdminManga_Click;
            // 
            // btnProfile
            // 
            btnProfile.Dock = DockStyle.Top;
            btnProfile.FlatAppearance.BorderSize = 0;
            btnProfile.FlatStyle = FlatStyle.Flat;
            btnProfile.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnProfile.ForeColor = Color.White;
            btnProfile.Image = (Image)resources.GetObject("btnProfile.Image");
            btnProfile.ImageAlign = ContentAlignment.MiddleLeft;
            btnProfile.Location = new Point(0, 468);
            btnProfile.Margin = new Padding(4);
            btnProfile.Name = "btnProfile";
            btnProfile.Padding = new Padding(11, 0, 0, 0);
            btnProfile.Size = new Size(200, 65);
            btnProfile.TabIndex = 7;
            btnProfile.Text = "Mi perfil";
            btnProfile.UseVisualStyleBackColor = true;
            btnProfile.Click += btnProfile_Click;
            // 
            // panelReportsSubmenu
            // 
            panelReportsSubmenu.BackColor = Color.FromArgb(46, 79, 102);
            panelReportsSubmenu.Controls.Add(btnUserReports);
            panelReportsSubmenu.Controls.Add(btnActivityReports);
            panelReportsSubmenu.Dock = DockStyle.Top;
            panelReportsSubmenu.Location = new Point(0, 376);
            panelReportsSubmenu.Margin = new Padding(4);
            panelReportsSubmenu.Name = "panelReportsSubmenu";
            panelReportsSubmenu.Size = new Size(200, 92);
            panelReportsSubmenu.TabIndex = 5;
            // 
            // btnUserReports
            // 
            btnUserReports.BackColor = Color.FromArgb(64, 64, 64);
            btnUserReports.Dock = DockStyle.Top;
            btnUserReports.FlatAppearance.BorderSize = 0;
            btnUserReports.FlatStyle = FlatStyle.Flat;
            btnUserReports.ForeColor = Color.White;
            btnUserReports.Location = new Point(0, 46);
            btnUserReports.Margin = new Padding(4);
            btnUserReports.Name = "btnUserReports";
            btnUserReports.Padding = new Padding(41, 0, 0, 0);
            btnUserReports.Size = new Size(200, 46);
            btnUserReports.TabIndex = 1;
            btnUserReports.Text = "Reportes de Usuarios";
            btnUserReports.TextAlign = ContentAlignment.MiddleLeft;
            btnUserReports.UseVisualStyleBackColor = false;
            // 
            // btnActivityReports
            // 
            btnActivityReports.BackColor = Color.FromArgb(64, 64, 64);
            btnActivityReports.Dock = DockStyle.Top;
            btnActivityReports.FlatAppearance.BorderSize = 0;
            btnActivityReports.FlatStyle = FlatStyle.Flat;
            btnActivityReports.ForeColor = Color.White;
            btnActivityReports.Location = new Point(0, 0);
            btnActivityReports.Margin = new Padding(4);
            btnActivityReports.Name = "btnActivityReports";
            btnActivityReports.Padding = new Padding(41, 0, 0, 0);
            btnActivityReports.Size = new Size(200, 46);
            btnActivityReports.TabIndex = 0;
            btnActivityReports.Text = "Reportes de Actividad";
            btnActivityReports.TextAlign = ContentAlignment.MiddleLeft;
            btnActivityReports.UseVisualStyleBackColor = false;
            // 
            // panelUsersSubmenu
            // 
            panelUsersSubmenu.BackColor = Color.FromArgb(46, 79, 102);
            panelUsersSubmenu.Controls.Add(btnAddUser);
            panelUsersSubmenu.Controls.Add(btnListUsers);
            panelUsersSubmenu.Dock = DockStyle.Top;
            panelUsersSubmenu.Location = new Point(0, 284);
            panelUsersSubmenu.Margin = new Padding(4);
            panelUsersSubmenu.Name = "panelUsersSubmenu";
            panelUsersSubmenu.Size = new Size(200, 92);
            panelUsersSubmenu.TabIndex = 3;
            // 
            // btnAddUser
            // 
            btnAddUser.BackColor = Color.FromArgb(64, 64, 64);
            btnAddUser.Dock = DockStyle.Top;
            btnAddUser.FlatAppearance.BorderSize = 0;
            btnAddUser.FlatStyle = FlatStyle.Flat;
            btnAddUser.ForeColor = Color.White;
            btnAddUser.Location = new Point(0, 46);
            btnAddUser.Margin = new Padding(4);
            btnAddUser.Name = "btnAddUser";
            btnAddUser.Padding = new Padding(41, 0, 0, 0);
            btnAddUser.Size = new Size(200, 46);
            btnAddUser.TabIndex = 1;
            btnAddUser.Text = "Agregar Usuario";
            btnAddUser.TextAlign = ContentAlignment.MiddleLeft;
            btnAddUser.UseVisualStyleBackColor = false;
            btnAddUser.Click += btnAddUser_Click;
            // 
            // btnListUsers
            // 
            btnListUsers.BackColor = Color.FromArgb(64, 64, 64);
            btnListUsers.Dock = DockStyle.Top;
            btnListUsers.FlatAppearance.BorderSize = 0;
            btnListUsers.FlatStyle = FlatStyle.Flat;
            btnListUsers.ForeColor = Color.White;
            btnListUsers.Location = new Point(0, 0);
            btnListUsers.Margin = new Padding(4);
            btnListUsers.Name = "btnListUsers";
            btnListUsers.Padding = new Padding(41, 0, 0, 0);
            btnListUsers.Size = new Size(200, 46);
            btnListUsers.TabIndex = 0;
            btnListUsers.Text = "Lista de Usuarios";
            btnListUsers.TextAlign = ContentAlignment.MiddleLeft;
            btnListUsers.UseVisualStyleBackColor = false;
            btnListUsers.Click += btnListUsers_Click;
            // 
            // btnUsers
            // 
            btnUsers.Dock = DockStyle.Top;
            btnUsers.FlatAppearance.BorderSize = 0;
            btnUsers.FlatStyle = FlatStyle.Flat;
            btnUsers.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnUsers.ForeColor = Color.White;
            btnUsers.Image = (Image)resources.GetObject("btnUsers.Image");
            btnUsers.ImageAlign = ContentAlignment.MiddleLeft;
            btnUsers.Location = new Point(0, 219);
            btnUsers.Margin = new Padding(4);
            btnUsers.Name = "btnUsers";
            btnUsers.Padding = new Padding(11, 0, 0, 0);
            btnUsers.Size = new Size(200, 65);
            btnUsers.TabIndex = 2;
            btnUsers.Text = "Usuarios";
            btnUsers.UseVisualStyleBackColor = true;
            btnUsers.Click += btnUsers_Click;
            // 
            // btnDashboard
            // 
            btnDashboard.Dock = DockStyle.Top;
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnDashboard.ForeColor = Color.White;
            btnDashboard.Image = (Image)resources.GetObject("btnDashboard.Image");
            btnDashboard.ImageAlign = ContentAlignment.MiddleLeft;
            btnDashboard.Location = new Point(0, 154);
            btnDashboard.Margin = new Padding(4);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Padding = new Padding(11, 0, 0, 0);
            btnDashboard.Size = new Size(200, 65);
            btnDashboard.TabIndex = 1;
            btnDashboard.Text = "Dashboard";
            btnDashboard.UseVisualStyleBackColor = true;
            btnDashboard.Click += btnDashboard_Click;
            // 
            // panelLogo
            // 
            panelLogo.Controls.Add(pictureBoxAvatar);
            panelLogo.Controls.Add(labelWelcome);
            panelLogo.Dock = DockStyle.Top;
            panelLogo.Location = new Point(0, 0);
            panelLogo.Margin = new Padding(4);
            panelLogo.Name = "panelLogo";
            panelLogo.Size = new Size(200, 154);
            panelLogo.TabIndex = 0;
            // 
            // pictureBoxAvatar
            // 
            pictureBoxAvatar.Location = new Point(40, 9);
            pictureBoxAvatar.Margin = new Padding(3, 2, 3, 2);
            pictureBoxAvatar.Name = "pictureBoxAvatar";
            pictureBoxAvatar.Size = new Size(116, 94);
            pictureBoxAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxAvatar.TabIndex = 1;
            pictureBoxAvatar.TabStop = false;
            // 
            // labelWelcome
            // 
            labelWelcome.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelWelcome.ForeColor = Color.LavenderBlush;
            labelWelcome.Location = new Point(0, 118);
            labelWelcome.Name = "labelWelcome";
            labelWelcome.Size = new Size(200, 21);
            labelWelcome.TabIndex = 0;
            labelWelcome.Text = "Bienvenido";
            labelWelcome.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnLogout
            // 
            btnLogout.Dock = DockStyle.Bottom;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold);
            btnLogout.ForeColor = Color.Gainsboro;
            btnLogout.Location = new Point(0, 704);
            btnLogout.Margin = new Padding(4);
            btnLogout.Name = "btnLogout";
            btnLogout.Padding = new Padding(11, 0, 0, 0);
            btnLogout.Size = new Size(200, 46);
            btnLogout.TabIndex = 6;
            btnLogout.Text = "Cerrar Sesión";
            btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // panelTitleBar
            // 
            panelTitleBar.BackColor = Color.Black;
            panelTitleBar.Controls.Add(panel1);
            panelTitleBar.Controls.Add(lblTitle);
            panelTitleBar.Dock = DockStyle.Top;
            panelTitleBar.Location = new Point(200, 0);
            panelTitleBar.Margin = new Padding(4);
            panelTitleBar.Name = "panelTitleBar";
            panelTitleBar.Size = new Size(966, 46);
            panelTitleBar.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.ForeColor = Color.White;
            panel1.Location = new Point(0, 37);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(966, 8);
            panel1.TabIndex = 14;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Microsoft Sans Serif", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.Gainsboro;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(966, 34);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Dashboard";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelDesktop
            // 
            panelDesktop.BackColor = Color.Transparent;
            panelDesktop.Dock = DockStyle.Fill;
            panelDesktop.Location = new Point(200, 46);
            panelDesktop.Margin = new Padding(4);
            panelDesktop.Name = "panelDesktop";
            panelDesktop.Size = new Size(966, 704);
            panelDesktop.TabIndex = 2;
            // 
            // MainAdminLayout
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1166, 750);
            Controls.Add(panelDesktop);
            Controls.Add(panelTitleBar);
            Controls.Add(panelSidebar);
            Margin = new Padding(4);
            MinimumSize = new Size(1182, 782);
            Name = "MainAdminLayout";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Panel de Administración";
            panelSidebar.ResumeLayout(false);
            panelReportsSubmenu.ResumeLayout(false);
            panelUsersSubmenu.ResumeLayout(false);
            panelLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxAvatar).EndInit();
            panelTitleBar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelSidebar;
        private Panel panelLogo;
        private Button btnDashboard;
        private Button btnUsers;
        private Panel panelUsersSubmenu;
        private Button btnListUsers;
        private Button btnAddUser;
        private Panel panelReportsSubmenu;
        private Button btnUserReports;
        private Button btnActivityReports;
        private Button btnLogout;
        private Panel panelTitleBar;
        private Label lblTitle;
        private Panel panelDesktop;
        private PictureBox pictureBoxAvatar;
        private Label labelWelcome;
        private Button btnProfile;
        private Panel panel1;
        private Button btnAdminManga;
    }
}