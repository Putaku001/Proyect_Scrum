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
            label1 = new Label();
            panel1 = new Panel();
            label2 = new Label();
            label3 = new Label();
            txtUsuario = new TextBox();
            txtContrasena = new TextBox();
            label4 = new Label();
            registerLinkLabel = new LinkLabel();
            btnIniciarSesion = new customControl();
            label5 = new Label();
            linkLabel1 = new LinkLabel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(790, 68);
            label1.Name = "label1";
            label1.Size = new Size(92, 21);
            label1.TabIndex = 0;
            label1.Text = "Bienvenido!";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Location = new Point(576, 68);
            panel1.Name = "panel1";
            panel1.Size = new Size(10, 400);
            panel1.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(672, 120);
            label2.Name = "label2";
            label2.Size = new Size(55, 17);
            label2.TabIndex = 2;
            label2.Text = "Usuario";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(672, 192);
            label3.Name = "label3";
            label3.Size = new Size(77, 17);
            label3.TabIndex = 3;
            label3.Text = "Contraseña";
            // 
            // txtUsuario
            // 
            txtUsuario.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsuario.Location = new Point(672, 140);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(323, 25);
            txtUsuario.TabIndex = 4;
            // 
            // txtContrasena
            // 
            txtContrasena.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtContrasena.Location = new Point(672, 212);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.Size = new Size(323, 25);
            txtContrasena.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(748, 390);
            label4.Name = "label4";
            label4.Size = new Size(118, 17);
            label4.TabIndex = 7;
            label4.Text = "¿No tienes cuenta?\r\n";
            // 
            // registerLinkLabel
            // 
            registerLinkLabel.AutoSize = true;
            registerLinkLabel.BackColor = Color.Transparent;
            registerLinkLabel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            registerLinkLabel.LinkColor = Color.LightBlue;
            registerLinkLabel.Location = new Point(861, 390);
            registerLinkLabel.Name = "registerLinkLabel";
            registerLinkLabel.Size = new Size(70, 17);
            registerLinkLabel.TabIndex = 8;
            registerLinkLabel.TabStop = true;
            registerLinkLabel.Text = "Registrate";
            registerLinkLabel.LinkClicked += registerLinkLabel_LinkClicked;
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
            btnIniciarSesion.Location = new Point(763, 313);
            btnIniciarSesion.Name = "btnIniciarSesion";
            btnIniciarSesion.Size = new Size(150, 40);
            btnIniciarSesion.TabIndex = 9;
            btnIniciarSesion.Text = "Iniciar Sesion";
            btnIniciarSesion.UseVisualStyleBackColor = false;
            btnIniciarSesion.Click += btnIniciarSesion_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(624, 293);
            label5.Name = "label5";
            label5.Size = new Size(0, 17);
            label5.TabIndex = 10;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.BackColor = Color.Transparent;
            linkLabel1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkLabel1.LinkColor = Color.LightBlue;
            linkLabel1.Location = new Point(672, 250);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(168, 17);
            linkLabel1.TabIndex = 11;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "¿Olvidaste Tu contraseña?";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // Login
            // 
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
            Controls.Add(panel1);
            Controls.Add(label1);
            Name = "Login";
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel panel1;
        private Label label2;
        private Label label3;
        private TextBox txtUsuario;
        private TextBox txtContrasena;
        private Label label4;
        private LinkLabel registerLinkLabel;
        private customControl btnIniciarSesion;
        private Label label5;
        private LinkLabel linkLabel1;
    }
}