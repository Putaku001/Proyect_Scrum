namespace ProyectScrum.Forms
{
    partial class New_Password
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(New_Password));
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBox1 = new Guna.UI2.WinForms.Guna2TextBox();
            textBox2 = new Guna.UI2.WinForms.Guna2TextBox();
            customControl1 = new Guna.UI2.WinForms.Guna2GradientButton();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel2.Controls.Add(label2, 0, 1);
            tableLayoutPanel2.Controls.Add(label3, 0, 3);
            tableLayoutPanel2.Controls.Add(textBox1, 0, 2);
            tableLayoutPanel2.Controls.Add(textBox2, 0, 4);
            tableLayoutPanel2.Controls.Add(customControl1, 0, 5);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(163, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 6;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7.882883F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 17.1171169F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 16.44144F));
            tableLayoutPanel2.Size = new Size(474, 444);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(label1, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(468, 105);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(3, 52);
            label1.Name = "label1";
            label1.Size = new Size(462, 37);
            label1.TabIndex = 0;
            label1.Text = "Cambia tu Contraseña";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Bottom;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(3, 155);
            label2.Name = "label2";
            label2.Size = new Size(468, 30);
            label2.TabIndex = 2;
            label2.Text = "Escribe tu Nueva Contraseña:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Bottom;
            label3.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(3, 264);
            label3.Name = "label3";
            label3.Size = new Size(468, 30);
            label3.TabIndex = 3;
            label3.Text = "Confirma la Contraseña:";
            // 
            // textBox1
            // 
            textBox1.BorderRadius = 5;
            textBox1.CustomizableEdges = customizableEdges1;
            textBox1.DefaultText = "";
            textBox1.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            textBox1.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            textBox1.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            textBox1.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            textBox1.Dock = DockStyle.Top;
            textBox1.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            textBox1.Font = new Font("Segoe UI", 9F);
            textBox1.ForeColor = Color.Black;
            textBox1.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            textBox1.Location = new Point(3, 188);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "";
            textBox1.SelectedText = "";
            textBox1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            textBox1.Size = new Size(468, 36);
            textBox1.TabIndex = 4;
            // 
            // textBox2
            // 
            textBox2.BorderRadius = 5;
            textBox2.CustomizableEdges = customizableEdges3;
            textBox2.DefaultText = "";
            textBox2.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            textBox2.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            textBox2.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            textBox2.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            textBox2.Dock = DockStyle.Top;
            textBox2.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            textBox2.Font = new Font("Segoe UI", 9F);
            textBox2.ForeColor = Color.Black;
            textBox2.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            textBox2.Location = new Point(3, 297);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "";
            textBox2.SelectedText = "";
            textBox2.ShadowDecoration.CustomizableEdges = customizableEdges4;
            textBox2.Size = new Size(468, 35);
            textBox2.TabIndex = 5;
            // 
            // customControl1
            // 
            customControl1.Animated = true;
            customControl1.AnimatedGIF = true;
            customControl1.BorderRadius = 5;
            customControl1.CustomizableEdges = customizableEdges5;
            customControl1.DisabledState.BorderColor = Color.DarkGray;
            customControl1.DisabledState.CustomBorderColor = Color.DarkGray;
            customControl1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            customControl1.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
            customControl1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            customControl1.Dock = DockStyle.Top;
            customControl1.FillColor = Color.DarkBlue;
            customControl1.FillColor2 = Color.LightSeaGreen;
            customControl1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            customControl1.ForeColor = Color.White;
            customControl1.Location = new Point(3, 373);
            customControl1.Name = "customControl1";
            customControl1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            customControl1.Size = new Size(468, 45);
            customControl1.TabIndex = 6;
            customControl1.Text = "Confirmar Cambio";
            customControl1.Click += customControl1_Click;
            // 
            // New_Password
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "New_Password";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "New_Password";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label1;
        private Label label2;
        private Label label3;
        private Guna.UI2.WinForms.Guna2TextBox textBox1;
        private Guna.UI2.WinForms.Guna2TextBox textBox2;
        private Guna.UI2.WinForms.Guna2GradientButton customControl1;
    }
}