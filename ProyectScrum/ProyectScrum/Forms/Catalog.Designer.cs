namespace ProyectScrum.Forms
{
    partial class Catalog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Catalog));
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            pictureBox1 = new PictureBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            anteriorButton = new Label();
            siguienteButton = new Label();
            panelFiltro = new Panel();
            btnAplicarFiltro = new Button();
            flowCheckBoxGeneros = new FlowLayoutPanel();
            btnFiltro = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panelFiltro.SuspendLayout();
            SuspendLayout();
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.ForeColor = SystemColors.ButtonFace;
            label7.Location = new Point(156, 4);
            label7.Name = "label7";
            label7.Size = new Size(69, 20);
            label7.TabIndex = 17;
            label7.Text = "Catálago";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = SystemColors.ButtonFace;
            label8.Location = new Point(156, 22);
            label8.Name = "label8";
            label8.Size = new Size(94, 21);
            label8.TabIndex = 18;
            label8.Text = "COMPLETO";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.ForeColor = SystemColors.ButtonHighlight;
            label9.Location = new Point(16, 13);
            label9.Name = "label9";
            label9.Size = new Size(80, 21);
            label9.TabIndex = 19;
            label9.Text = "MANGAS";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Black;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(100, 4);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(60, 37);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 20;
            pictureBox1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Location = new Point(59, 62);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(774, 437);
            flowLayoutPanel1.TabIndex = 21;
            // 
            // anteriorButton
            // 
            anteriorButton.AutoSize = true;
            anteriorButton.BackColor = Color.Transparent;
            anteriorButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            anteriorButton.ForeColor = Color.White;
            anteriorButton.Location = new Point(425, 476);
            anteriorButton.Name = "anteriorButton";
            anteriorButton.Size = new Size(15, 15);
            anteriorButton.TabIndex = 22;
            anteriorButton.Text = "<";
            anteriorButton.Click += anteriorButton_Click;
            // 
            // siguienteButton
            // 
            siguienteButton.AutoSize = true;
            siguienteButton.BackColor = Color.Transparent;
            siguienteButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            siguienteButton.ForeColor = Color.White;
            siguienteButton.Location = new Point(462, 476);
            siguienteButton.Name = "siguienteButton";
            siguienteButton.Size = new Size(15, 15);
            siguienteButton.TabIndex = 23;
            siguienteButton.Text = ">";
            siguienteButton.Click += siguienteButton_Click;
            // 
            // panelFiltro
            // 
            panelFiltro.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panelFiltro.BackColor = Color.FromArgb(220, 30, 30, 30);
            panelFiltro.BorderStyle = BorderStyle.FixedSingle;
            panelFiltro.Controls.Add(btnAplicarFiltro);
            panelFiltro.Controls.Add(flowCheckBoxGeneros);
            panelFiltro.Location = new Point(633, 22);
            panelFiltro.Name = "panelFiltro";
            panelFiltro.Size = new Size(200, 300);
            panelFiltro.TabIndex = 24;
            panelFiltro.Visible = false;
            // 
            // btnAplicarFiltro
            // 
            btnAplicarFiltro.Dock = DockStyle.Bottom;
            btnAplicarFiltro.Location = new Point(0, 275);
            btnAplicarFiltro.Name = "btnAplicarFiltro";
            btnAplicarFiltro.Size = new Size(198, 23);
            btnAplicarFiltro.TabIndex = 1;
            btnAplicarFiltro.Text = "aplicar";
            btnAplicarFiltro.UseVisualStyleBackColor = true;
            btnAplicarFiltro.Click += btnAplicarFiltro_Click;
            // 
            // flowCheckBoxGeneros
            // 
            flowCheckBoxGeneros.AutoScroll = true;
            flowCheckBoxGeneros.Dock = DockStyle.Top;
            flowCheckBoxGeneros.Location = new Point(0, 0);
            flowCheckBoxGeneros.Name = "flowCheckBoxGeneros";
            flowCheckBoxGeneros.Size = new Size(198, 200);
            flowCheckBoxGeneros.TabIndex = 0;
            // 
            // btnFiltro
            // 
            btnFiltro.FlatStyle = FlatStyle.Flat;
            btnFiltro.ForeColor = Color.White;
            btnFiltro.Location = new Point(797, 13);
            btnFiltro.Name = "btnFiltro";
            btnFiltro.Size = new Size(62, 33);
            btnFiltro.TabIndex = 25;
            btnFiltro.Text = "filtrar";
            btnFiltro.UseVisualStyleBackColor = true;
            btnFiltro.Click += btnFiltro_Click;
            // 
            // Catalog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(882, 500);
            Controls.Add(btnFiltro);
            Controls.Add(panelFiltro);
            Controls.Add(siguienteButton);
            Controls.Add(anteriorButton);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(pictureBox1);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Catalog";
            Text = "Catalog";
            Load += Catalog_Load;
            MouseDown += Catalog_MouseDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panelFiltro.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label7;
        private Label label8;
        private Label label9;
        private PictureBox pictureBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label anteriorButton;
        private Label siguienteButton;
        private Panel panelFiltro;
        private Button btnAplicarFiltro;
        private FlowLayoutPanel flowCheckBoxGeneros;
        private Button btnFiltro;
    }
}