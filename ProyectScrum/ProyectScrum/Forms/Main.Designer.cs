namespace ProyectScrum
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            slideBarTime = new System.Windows.Forms.Timer(components);
            menu = new TableLayoutPanel();
            SlideBar = new Guna.UI2.WinForms.Guna2Panel();
            panelMenu = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            button1 = new Guna.UI2.WinForms.Guna2Button();
            tableLayoutPanel10 = new TableLayoutPanel();
            cerrarSesionButton = new Guna.UI2.WinForms.Guna2Button();
            tableLayoutPanel8 = new TableLayoutPanel();
            perfilButton = new Guna.UI2.WinForms.Guna2Button();
            tableLayoutPanel7 = new TableLayoutPanel();
            menuButton = new Guna.UI2.WinForms.Guna2Button();
            tableLayoutPanel9 = new TableLayoutPanel();
            catalogbtn = new Guna.UI2.WinForms.Guna2Button();
            panelContenedor = new Panel();
            menu.SuspendLayout();
            SlideBar.SuspendLayout();
            panelMenu.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel10.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel9.SuspendLayout();
            SuspendLayout();
            // 
            // slideBarTime
            // 
            slideBarTime.Interval = 10;
            slideBarTime.Tick += slideBarTime_Tick;
            // 
            // menu
            // 
            menu.BackColor = Color.FromArgb(20, 24, 59, 255);
            menu.ColumnCount = 2;
            menu.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 225F));
            menu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            menu.Controls.Add(SlideBar, 0, 0);
            menu.Controls.Add(panelContenedor, 1, 0);
            menu.Dock = DockStyle.Fill;
            menu.Location = new Point(0, 0);
            menu.MinimumSize = new Size(77, 539);
            menu.Name = "menu";
            menu.RowCount = 1;
            menu.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            menu.Size = new Size(1129, 545);
            menu.TabIndex = 1;
            // 
            // SlideBar
            // 
            SlideBar.BackColor = Color.Transparent;
            SlideBar.BorderColor = Color.FromArgb(12, 19, 48, 255);
            SlideBar.BorderRadius = 10;
            SlideBar.Controls.Add(panelMenu);
            SlideBar.CustomizableEdges = customizableEdges11;
            SlideBar.Dock = DockStyle.Fill;
            SlideBar.Location = new Point(3, 3);
            SlideBar.Name = "SlideBar";
            SlideBar.ShadowDecoration.CustomizableEdges = customizableEdges12;
            SlideBar.Size = new Size(219, 539);
            SlideBar.TabIndex = 1;
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(19, 23, 58, 255);
            panelMenu.ColumnCount = 1;
            panelMenu.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            panelMenu.Controls.Add(tableLayoutPanel3, 0, 3);
            panelMenu.Controls.Add(tableLayoutPanel10, 0, 6);
            panelMenu.Controls.Add(tableLayoutPanel8, 0, 2);
            panelMenu.Controls.Add(tableLayoutPanel7, 0, 1);
            panelMenu.Controls.Add(tableLayoutPanel9, 0, 4);
            panelMenu.Dock = DockStyle.Fill;
            panelMenu.Location = new Point(0, 0);
            panelMenu.MinimumSize = new Size(77, 539);
            panelMenu.Name = "panelMenu";
            panelMenu.RowCount = 7;
            panelMenu.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            panelMenu.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            panelMenu.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            panelMenu.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            panelMenu.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            panelMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panelMenu.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            panelMenu.Size = new Size(219, 539);
            panelMenu.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.Transparent;
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(button1, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 263);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel3.Size = new Size(214, 64);
            tableLayoutPanel3.TabIndex = 4;
            // 
            // button1
            // 
            button1.Animated = true;
            button1.BackColor = Color.Transparent;
            button1.Cursor = Cursors.Hand;
            button1.CustomBorderColor = Color.FromArgb(224, 224, 224);
            button1.CustomBorderThickness = new Padding(0, 0, 0, 1);
            button1.CustomizableEdges = customizableEdges1;
            button1.DisabledState.BorderColor = Color.DarkGray;
            button1.DisabledState.CustomBorderColor = Color.DarkGray;
            button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            button1.Dock = DockStyle.Fill;
            button1.FillColor = Color.Transparent;
            button1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.ImageAlign = HorizontalAlignment.Left;
            button1.Location = new Point(3, 17);
            button1.Name = "button1";
            button1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            button1.Size = new Size(204, 44);
            button1.TabIndex = 0;
            button1.Text = "Favoritos";
            button1.Click += button1_Click;
            // 
            // tableLayoutPanel10
            // 
            tableLayoutPanel10.BackColor = Color.Transparent;
            tableLayoutPanel10.ColumnCount = 2;
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel10.Controls.Add(cerrarSesionButton, 0, 1);
            tableLayoutPanel10.Dock = DockStyle.Fill;
            tableLayoutPanel10.Location = new Point(3, 472);
            tableLayoutPanel10.Name = "tableLayoutPanel10";
            tableLayoutPanel10.RowCount = 2;
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel10.Size = new Size(214, 64);
            tableLayoutPanel10.TabIndex = 5;
            // 
            // cerrarSesionButton
            // 
            cerrarSesionButton.Animated = true;
            cerrarSesionButton.BackColor = Color.Transparent;
            cerrarSesionButton.Cursor = Cursors.Hand;
            cerrarSesionButton.CustomizableEdges = customizableEdges3;
            cerrarSesionButton.DisabledState.BorderColor = Color.DarkGray;
            cerrarSesionButton.DisabledState.CustomBorderColor = Color.DarkGray;
            cerrarSesionButton.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            cerrarSesionButton.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            cerrarSesionButton.Dock = DockStyle.Fill;
            cerrarSesionButton.FillColor = Color.Transparent;
            cerrarSesionButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cerrarSesionButton.ForeColor = Color.White;
            cerrarSesionButton.Image = (Image)resources.GetObject("cerrarSesionButton.Image");
            cerrarSesionButton.ImageAlign = HorizontalAlignment.Left;
            cerrarSesionButton.Location = new Point(3, 17);
            cerrarSesionButton.Name = "cerrarSesionButton";
            cerrarSesionButton.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cerrarSesionButton.Size = new Size(204, 44);
            cerrarSesionButton.TabIndex = 0;
            cerrarSesionButton.Text = "Cerrar Sesión";
            cerrarSesionButton.Click += cerrarSesionButton_Click;
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.BackColor = Color.Transparent;
            tableLayoutPanel8.ColumnCount = 2;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.Controls.Add(perfilButton, 0, 1);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(3, 193);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 2;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel8.Size = new Size(214, 64);
            tableLayoutPanel8.TabIndex = 4;
            // 
            // perfilButton
            // 
            perfilButton.Animated = true;
            perfilButton.BackColor = Color.Transparent;
            perfilButton.Cursor = Cursors.Hand;
            perfilButton.CustomBorderColor = Color.FromArgb(224, 224, 224);
            perfilButton.CustomBorderThickness = new Padding(0, 0, 0, 1);
            perfilButton.CustomizableEdges = customizableEdges5;
            perfilButton.DisabledState.BorderColor = Color.DarkGray;
            perfilButton.DisabledState.CustomBorderColor = Color.DarkGray;
            perfilButton.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            perfilButton.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            perfilButton.Dock = DockStyle.Fill;
            perfilButton.FillColor = Color.Transparent;
            perfilButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            perfilButton.ForeColor = Color.White;
            perfilButton.Image = (Image)resources.GetObject("perfilButton.Image");
            perfilButton.ImageAlign = HorizontalAlignment.Left;
            perfilButton.Location = new Point(3, 17);
            perfilButton.Name = "perfilButton";
            perfilButton.ShadowDecoration.CustomizableEdges = customizableEdges6;
            perfilButton.Size = new Size(204, 44);
            perfilButton.TabIndex = 0;
            perfilButton.Text = "Perfil";
            perfilButton.Click += perfilButton_Click;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.BackColor = Color.Transparent;
            tableLayoutPanel7.ColumnCount = 2;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Controls.Add(menuButton, 0, 1);
            tableLayoutPanel7.Location = new Point(3, 123);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 2;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel7.Size = new Size(213, 64);
            tableLayoutPanel7.TabIndex = 4;
            // 
            // menuButton
            // 
            menuButton.Animated = true;
            menuButton.BackColor = Color.Transparent;
            menuButton.BorderColor = Color.Transparent;
            menuButton.BorderThickness = 1;
            menuButton.Cursor = Cursors.Hand;
            menuButton.CustomBorderColor = Color.FromArgb(224, 224, 224);
            menuButton.CustomBorderThickness = new Padding(0, 0, 0, 1);
            menuButton.CustomizableEdges = customizableEdges7;
            menuButton.DisabledState.BorderColor = Color.DarkGray;
            menuButton.DisabledState.CustomBorderColor = Color.DarkGray;
            menuButton.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            menuButton.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            menuButton.Dock = DockStyle.Fill;
            menuButton.FillColor = Color.Transparent;
            menuButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            menuButton.ForeColor = Color.White;
            menuButton.Image = (Image)resources.GetObject("menuButton.Image");
            menuButton.ImageAlign = HorizontalAlignment.Left;
            menuButton.Location = new Point(3, 17);
            menuButton.Name = "menuButton";
            menuButton.ShadowDecoration.CustomizableEdges = customizableEdges8;
            menuButton.Size = new Size(204, 44);
            menuButton.TabIndex = 0;
            menuButton.Text = "Menu";
            menuButton.Click += menuButton_Click;
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.BackColor = Color.Transparent;
            tableLayoutPanel9.ColumnCount = 2;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel9.Controls.Add(catalogbtn, 0, 1);
            tableLayoutPanel9.Dock = DockStyle.Fill;
            tableLayoutPanel9.Location = new Point(3, 333);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowCount = 2;
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel9.Size = new Size(214, 64);
            tableLayoutPanel9.TabIndex = 3;
            // 
            // catalogbtn
            // 
            catalogbtn.Animated = true;
            catalogbtn.BackColor = Color.Transparent;
            catalogbtn.Cursor = Cursors.Hand;
            catalogbtn.CustomBorderColor = Color.FromArgb(224, 224, 224);
            catalogbtn.CustomBorderThickness = new Padding(0, 0, 0, 1);
            catalogbtn.CustomizableEdges = customizableEdges9;
            catalogbtn.DisabledState.BorderColor = Color.DarkGray;
            catalogbtn.DisabledState.CustomBorderColor = Color.DarkGray;
            catalogbtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            catalogbtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            catalogbtn.Dock = DockStyle.Fill;
            catalogbtn.FillColor = Color.Transparent;
            catalogbtn.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            catalogbtn.ForeColor = Color.White;
            catalogbtn.Image = (Image)resources.GetObject("catalogbtn.Image");
            catalogbtn.ImageAlign = HorizontalAlignment.Left;
            catalogbtn.Location = new Point(3, 17);
            catalogbtn.Name = "catalogbtn";
            catalogbtn.ShadowDecoration.CustomizableEdges = customizableEdges10;
            catalogbtn.Size = new Size(204, 44);
            catalogbtn.TabIndex = 0;
            catalogbtn.Text = "Catalogo";
            catalogbtn.Click += catalogbtn_Click;
            // 
            // panelContenedor
            // 
            panelContenedor.BackColor = Color.Transparent;
            panelContenedor.Dock = DockStyle.Fill;
            panelContenedor.Location = new Point(228, 3);
            panelContenedor.Name = "panelContenedor";
            panelContenedor.Size = new Size(898, 539);
            panelContenedor.TabIndex = 0;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1129, 545);
            Controls.Add(menu);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main";
            menu.ResumeLayout(false);
            SlideBar.ResumeLayout(false);
            panelMenu.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel10.ResumeLayout(false);
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel9.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer slideBarTime;
        private TableLayoutPanel menu;
        private Guna.UI2.WinForms.Guna2Panel SlideBar;
        private TableLayoutPanel panelMenu;
        private TableLayoutPanel tableLayoutPanel3;
        private Guna.UI2.WinForms.Guna2Button button1;
        private TableLayoutPanel tableLayoutPanel10;
        private Guna.UI2.WinForms.Guna2Button cerrarSesionButton;
        private TableLayoutPanel tableLayoutPanel8;
        private Guna.UI2.WinForms.Guna2Button perfilButton;
        private TableLayoutPanel tableLayoutPanel7;
        private Guna.UI2.WinForms.Guna2Button menuButton;
        private TableLayoutPanel tableLayoutPanel9;
        private Guna.UI2.WinForms.Guna2Button catalogbtn;
        private Panel panelContenedor;
    }
}