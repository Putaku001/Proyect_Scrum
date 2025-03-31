namespace ProyectScrum.Forms
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            pictureBox1 = new PictureBox();
            linkLabel1 = new LinkLabel();
            label5 = new Label();
            btnIniciarSesion = new customControl();
            registerLinkLabel = new LinkLabel();
            label4 = new Label();
            txtContrasena = new TextBox();
            txtUsuario = new TextBox();
            label3 = new Label();
            label2 = new Label();
            panel2 = new Panel();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(500, 539);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.BackColor = Color.Transparent;
            linkLabel1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkLabel1.LinkColor = Color.LightBlue;
            linkLabel1.Location = new Point(658, 261);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(168, 17);
            linkLabel1.TabIndex = 33;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "¿Olvidaste Tu contraseña?";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(610, 304);
            label5.Name = "label5";
            label5.Size = new Size(0, 17);
            label5.TabIndex = 32;
            // 
            // btnIniciarSesion
            // 
            btnIniciarSesion.BackColor = Color.MediumSlateBlue;
            btnIniciarSesion.BackgroundImage = (Image)resources.GetObject("btnIniciarSesion.BackgroundImage");
            btnIniciarSesion.Cursor = Cursors.Hand;
            btnIniciarSesion.FlatAppearance.BorderSize = 0;
            btnIniciarSesion.FlatStyle = FlatStyle.Flat;
            btnIniciarSesion.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnIniciarSesion.ForeColor = Color.White;
            btnIniciarSesion.Location = new Point(749, 324);
            btnIniciarSesion.Name = "btnIniciarSesion";
            btnIniciarSesion.Size = new Size(150, 40);
            btnIniciarSesion.TabIndex = 31;
            btnIniciarSesion.Text = "Iniciar Sesion";
            btnIniciarSesion.UseVisualStyleBackColor = false;
            btnIniciarSesion.Click += btnIniciarSesion_Click;
            // 
            // registerLinkLabel
            // 
            registerLinkLabel.AutoSize = true;
            registerLinkLabel.BackColor = Color.Transparent;
            registerLinkLabel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            registerLinkLabel.LinkColor = Color.LightBlue;
            registerLinkLabel.Location = new Point(847, 401);
            registerLinkLabel.Name = "registerLinkLabel";
            registerLinkLabel.Size = new Size(70, 17);
            registerLinkLabel.TabIndex = 30;
            registerLinkLabel.TabStop = true;
            registerLinkLabel.Text = "Registrate";
            registerLinkLabel.LinkClicked += registerLinkLabel_LinkClicked;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(723, 401);
            label4.Name = "label4";
            label4.Size = new Size(118, 17);
            label4.TabIndex = 29;
            label4.Text = "¿No tienes cuenta?\r\n";
            // 
            // txtContrasena
            // 
            txtContrasena.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtContrasena.Location = new Point(658, 223);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.PasswordChar = '*';
            txtContrasena.Size = new Size(323, 25);
            txtContrasena.TabIndex = 28;
            // 
            // txtUsuario
            // 
            txtUsuario.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsuario.Location = new Point(658, 151);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(323, 25);
            txtUsuario.TabIndex = 27;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(658, 203);
            label3.Name = "label3";
            label3.Size = new Size(77, 17);
            label3.TabIndex = 26;
            label3.Text = "Contraseña";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(658, 131);
            label2.Name = "label2";
            label2.Size = new Size(55, 17);
            label2.TabIndex = 25;
            label2.Text = "Usuario";
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Location = new Point(562, 79);
            panel2.Name = "panel2";
            panel2.Size = new Size(10, 400);
            panel2.TabIndex = 24;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(776, 79);
            label1.Name = "label1";
            label1.Size = new Size(92, 21);
            label1.TabIndex = 23;
            label1.Text = "Bienvenido!";
            // 
            // Login
            // 
            AcceptButton = btnIniciarSesion;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(159, 51, 255);
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1092, 539);
            Controls.Add(linkLabel1);
            Controls.Add(label5);
            Controls.Add(btnIniciarSesion);
            Controls.Add(registerLinkLabel);
            Controls.Add(label4);
            Controls.Add(txtContrasena);
            Controls.Add(txtUsuario);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(panel2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox pictureBox1;
        private LinkLabel linkLabel1;
        private Label label5;
        private customControl btnIniciarSesion;
        private LinkLabel registerLinkLabel;
        private Label label4;
        private TextBox txtContrasena;
        private TextBox txtUsuario;
        private Label label3;
        private Label label2;
        private Panel panel2;
        private Label label1;
    }
}