namespace ProyectScrum.Forms
{
    partial class PageTrial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PageTrial));
            SlideBar = new FlowLayoutPanel();
            panel1 = new Panel();
            panel6 = new Panel();
            menuButton = new Button();
            panel2 = new Panel();
            label1 = new Label();
            panel3 = new Panel();
            ISButton = new Button();
            slideBarTime = new System.Windows.Forms.Timer(components);
            SlideBar.SuspendLayout();
            panel1.SuspendLayout();
            panel6.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // SlideBar
            // 
            SlideBar.BackColor = Color.FromArgb(64, 64, 64);
            SlideBar.Controls.Add(panel1);
            SlideBar.Controls.Add(panel2);
            SlideBar.Controls.Add(panel3);
            SlideBar.Dock = DockStyle.Left;
            SlideBar.Location = new Point(0, 0);
            SlideBar.MaximumSize = new Size(194, 539);
            SlideBar.MinimumSize = new Size(77, 539);
            SlideBar.Name = "SlideBar";
            SlideBar.Size = new Size(194, 539);
            SlideBar.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel6);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(191, 100);
            panel1.TabIndex = 0;
            // 
            // panel6
            // 
            panel6.Controls.Add(menuButton);
            panel6.Location = new Point(0, 9);
            panel6.Name = "panel6";
            panel6.Size = new Size(191, 65);
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
            menuButton.ForeColor = SystemColors.ControlLightLight;
            menuButton.Image = (Image)resources.GetObject("menuButton.Image");
            menuButton.ImageAlign = ContentAlignment.MiddleLeft;
            menuButton.Location = new Point(0, 0);
            menuButton.Name = "menuButton";
            menuButton.Padding = new Padding(15, 0, 0, 0);
            menuButton.Size = new Size(188, 65);
            menuButton.TabIndex = 1;
            menuButton.Text = "Menu";
            menuButton.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(label1);
            panel2.Location = new Point(3, 109);
            panel2.Name = "panel2";
            panel2.Size = new Size(191, 65);
            panel2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(26, 12);
            label1.Name = "label1";
            label1.Size = new Size(128, 42);
            label1.TabIndex = 0;
            label1.Text = "Para Continuar \r\n  Inicia Sesion";
            // 
            // panel3
            // 
            panel3.Controls.Add(ISButton);
            panel3.Location = new Point(3, 180);
            panel3.Name = "panel3";
            panel3.Size = new Size(191, 62);
            panel3.TabIndex = 2;
            // 
            // ISButton
            // 
            ISButton.Cursor = Cursors.Hand;
            ISButton.Dock = DockStyle.Left;
            ISButton.FlatAppearance.BorderSize = 0;
            ISButton.FlatStyle = FlatStyle.Flat;
            ISButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ISButton.ForeColor = Color.White;
            ISButton.Image = (Image)resources.GetObject("ISButton.Image");
            ISButton.ImageAlign = ContentAlignment.MiddleLeft;
            ISButton.Location = new Point(0, 0);
            ISButton.Name = "ISButton";
            ISButton.Padding = new Padding(15, 0, 0, 0);
            ISButton.Size = new Size(188, 62);
            ISButton.TabIndex = 1;
            ISButton.Text = "  Iniciar Sesión";
            ISButton.UseVisualStyleBackColor = true;
            ISButton.Click += ISButton_Click;
            // 
            // slideBarTime
            // 
            slideBarTime.Interval = 10;
            // 
            // PageTrial
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1092, 539);
            Controls.Add(SlideBar);
            Name = "PageTrial";
            Text = "PageTrial";
            SlideBar.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel SlideBar;
        private Panel panel1;
        private Panel panel6;
        private Button menuButton;
        private Panel panel2;
        private Panel panel3;
        private Button ISButton;
        private Label label1;
        private System.Windows.Forms.Timer slideBarTime;
    }
}