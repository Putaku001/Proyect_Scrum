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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            SlideBar = new FlowLayoutPanel();
            panel1 = new Panel();
            panel6 = new Panel();
            menuButton = new Button();
            panel2 = new Panel();
            perfilButton = new Button();
            panel3 = new Panel();
            button1 = new Button();
            panel4 = new Panel();
            catalogbtn = new Button();
            panel5 = new Panel();
            cerrarSesionButton = new Button();
            slideBarTime = new System.Windows.Forms.Timer(components);
            panelContenedor = new Panel();
            SlideBar.SuspendLayout();
            panel1.SuspendLayout();
            panel6.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            SuspendLayout();
            // 
            // SlideBar
            // 
            SlideBar.BackColor = Color.FromArgb(64, 64, 64);
            SlideBar.Controls.Add(panel1);
            SlideBar.Controls.Add(panel2);
            SlideBar.Controls.Add(panel3);
            SlideBar.Controls.Add(panel4);
            SlideBar.Controls.Add(panel5);
            SlideBar.Dock = DockStyle.Left;
            SlideBar.Location = new Point(0, 0);
            SlideBar.Margin = new Padding(3, 4, 3, 4);
            SlideBar.MaximumSize = new Size(222, 719);
            SlideBar.MinimumSize = new Size(88, 719);
            SlideBar.Name = "SlideBar";
            SlideBar.Size = new Size(222, 719);
            SlideBar.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel6);
            panel1.Location = new Point(3, 4);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(218, 133);
            panel1.TabIndex = 0;
            // 
            // panel6
            // 
            panel6.Controls.Add(menuButton);
            panel6.Location = new Point(0, 12);
            panel6.Margin = new Padding(3, 4, 3, 4);
            panel6.Name = "panel6";
            panel6.Size = new Size(218, 87);
            panel6.TabIndex = 5;
            // 
            // menuButton
            // 
            menuButton.BackColor = Color.Transparent;
            menuButton.Cursor = Cursors.Hand;
            menuButton.Dock = DockStyle.Left;
            menuButton.FlatAppearance.BorderSize = 0;
            menuButton.FlatStyle = FlatStyle.Flat;
            menuButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            menuButton.ForeColor = Color.White;
            menuButton.Image = (Image)resources.GetObject("menuButton.Image");
            menuButton.ImageAlign = ContentAlignment.MiddleLeft;
            menuButton.Location = new Point(0, 0);
            menuButton.Margin = new Padding(3, 4, 3, 4);
            menuButton.Name = "menuButton";
            menuButton.Padding = new Padding(17, 0, 0, 0);
            menuButton.Size = new Size(215, 87);
            menuButton.TabIndex = 1;
            menuButton.Text = "Menu";
            menuButton.UseVisualStyleBackColor = false;
            menuButton.Click += menuButton_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(perfilButton);
            panel2.Location = new Point(3, 145);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(218, 87);
            panel2.TabIndex = 1;
            // 
            // perfilButton
            // 
            perfilButton.Cursor = Cursors.Hand;
            perfilButton.Dock = DockStyle.Left;
            perfilButton.FlatAppearance.BorderSize = 0;
            perfilButton.FlatStyle = FlatStyle.Flat;
            perfilButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            perfilButton.ForeColor = Color.White;
            perfilButton.Image = (Image)resources.GetObject("perfilButton.Image");
            perfilButton.ImageAlign = ContentAlignment.MiddleLeft;
            perfilButton.Location = new Point(0, 0);
            perfilButton.Margin = new Padding(3, 4, 3, 4);
            perfilButton.Name = "perfilButton";
            perfilButton.Padding = new Padding(17, 0, 0, 0);
            perfilButton.Size = new Size(215, 87);
            perfilButton.TabIndex = 1;
            perfilButton.Text = "Perfil";
            perfilButton.UseVisualStyleBackColor = true;
            perfilButton.Click += perfilButton_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(button1);
            panel3.Location = new Point(3, 240);
            panel3.Margin = new Padding(3, 4, 3, 4);
            panel3.Name = "panel3";
            panel3.Size = new Size(218, 83);
            panel3.TabIndex = 2;
            // 
            // button1
            // 
            button1.Cursor = Cursors.Hand;
            button1.Dock = DockStyle.Left;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(0, 0);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Padding = new Padding(17, 0, 0, 0);
            button1.Size = new Size(215, 83);
            button1.TabIndex = 1;
            button1.Text = "  Favoritos";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // panel4
            // 
            panel4.Controls.Add(catalogbtn);
            panel4.Location = new Point(3, 331);
            panel4.Margin = new Padding(3, 4, 3, 4);
            panel4.Name = "panel4";
            panel4.Size = new Size(218, 87);
            panel4.TabIndex = 3;
            // 
            // catalogbtn
            // 
            catalogbtn.Cursor = Cursors.Hand;
            catalogbtn.Dock = DockStyle.Left;
            catalogbtn.FlatAppearance.BorderSize = 0;
            catalogbtn.FlatStyle = FlatStyle.Flat;
            catalogbtn.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            catalogbtn.ForeColor = Color.White;
            catalogbtn.Image = (Image)resources.GetObject("catalogbtn.Image");
            catalogbtn.ImageAlign = ContentAlignment.MiddleLeft;
            catalogbtn.Location = new Point(0, 0);
            catalogbtn.Margin = new Padding(3, 4, 3, 4);
            catalogbtn.Name = "catalogbtn";
            catalogbtn.Padding = new Padding(17, 0, 0, 0);
            catalogbtn.Size = new Size(215, 87);
            catalogbtn.TabIndex = 1;
            catalogbtn.Text = " Catalogo";
            catalogbtn.UseVisualStyleBackColor = true;
            catalogbtn.Click += catalogbtn_Click;
            // 
            // panel5
            // 
            panel5.Controls.Add(cerrarSesionButton);
            panel5.Location = new Point(3, 426);
            panel5.Margin = new Padding(3, 4, 3, 4);
            panel5.Name = "panel5";
            panel5.Size = new Size(218, 87);
            panel5.TabIndex = 4;
            // 
            // cerrarSesionButton
            // 
            cerrarSesionButton.Cursor = Cursors.Hand;
            cerrarSesionButton.Dock = DockStyle.Left;
            cerrarSesionButton.FlatAppearance.BorderSize = 0;
            cerrarSesionButton.FlatStyle = FlatStyle.Flat;
            cerrarSesionButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cerrarSesionButton.ForeColor = Color.White;
            cerrarSesionButton.Image = (Image)resources.GetObject("cerrarSesionButton.Image");
            cerrarSesionButton.ImageAlign = ContentAlignment.MiddleLeft;
            cerrarSesionButton.Location = new Point(0, 0);
            cerrarSesionButton.Margin = new Padding(3, 4, 3, 4);
            cerrarSesionButton.Name = "cerrarSesionButton";
            cerrarSesionButton.Padding = new Padding(17, 0, 0, 0);
            cerrarSesionButton.Size = new Size(215, 87);
            cerrarSesionButton.TabIndex = 1;
            cerrarSesionButton.Text = "        Cerrar Sesión";
            cerrarSesionButton.UseVisualStyleBackColor = true;
            cerrarSesionButton.Click += cerrarSesionButton_Click;
            // 
            // slideBarTime
            // 
            slideBarTime.Interval = 10;
            slideBarTime.Tick += slideBarTime_Tick;
            // 
            // panelContenedor
            // 
            panelContenedor.AutoSize = true;
            panelContenedor.BackColor = Color.Transparent;
            panelContenedor.Dock = DockStyle.Fill;
            panelContenedor.Location = new Point(222, 0);
            panelContenedor.Margin = new Padding(3, 4, 3, 4);
            panelContenedor.Name = "panelContenedor";
            panelContenedor.Size = new Size(1026, 719);
            panelContenedor.TabIndex = 1;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1248, 719);
            Controls.Add(panelContenedor);
            Controls.Add(SlideBar);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main";
            SlideBar.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel SlideBar;
        private Panel panel1;
        private Panel panel2;
        private Button perfilButton;
        private Panel panel3;
        private Button button1;
        private Panel panel4;
        private Button catalogbtn;
        private Panel panel5;
        private Button cerrarSesionButton;
        private Panel panel6;
        private Button menuButton;
        private System.Windows.Forms.Timer slideBarTime;
        private Panel panelContenedor;
    }
}