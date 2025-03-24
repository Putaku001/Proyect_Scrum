namespace ProyectScrum.Forms
{
    partial class Register
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Register));
            label1 = new Label();
            label2 = new Label();
            txtNombreUsuario = new TextBox();
            txtEmail = new TextBox();
            label3 = new Label();
            txtContrasena = new TextBox();
            label4 = new Label();
            Labela = new Label();
            inicioSLinkLabel = new LinkLabel();
            btnRegistrar = new customControl();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(575, 41);
            label1.Name = "label1";
            label1.Size = new Size(109, 28);
            label1.TabIndex = 0;
            label1.Text = "Registrate";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(466, 129);
            label2.Name = "label2";
            label2.Size = new Size(160, 23);
            label2.TabIndex = 1;
            label2.Text = "Nombre de Usuario\r\n";
            // 
            // txtNombreUsuario
            // 
            txtNombreUsuario.Location = new Point(466, 156);
            txtNombreUsuario.Margin = new Padding(3, 4, 3, 4);
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.Size = new Size(306, 27);
            txtNombreUsuario.TabIndex = 2;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(466, 253);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(306, 27);
            txtEmail.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(466, 227);
            label3.Name = "label3";
            label3.Size = new Size(62, 23);
            label3.TabIndex = 3;
            label3.Text = "Correo";
            // 
            // txtContrasena
            // 
            txtContrasena.Location = new Point(466, 348);
            txtContrasena.Margin = new Padding(3, 4, 3, 4);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.Size = new Size(306, 27);
            txtContrasena.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(466, 321);
            label4.Name = "label4";
            label4.Size = new Size(97, 23);
            label4.TabIndex = 5;
            label4.Text = "Contraseña";
            // 
            // Labela
            // 
            Labela.AutoSize = true;
            Labela.BackColor = Color.Transparent;
            Labela.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Labela.ForeColor = Color.Transparent;
            Labela.Location = new Point(517, 473);
            Labela.Name = "Labela";
            Labela.Size = new Size(149, 23);
            Labela.TabIndex = 8;
            Labela.Text = "¿Ya tienes cuenta?";
            // 
            // inicioSLinkLabel
            // 
            inicioSLinkLabel.AutoSize = true;
            inicioSLinkLabel.BackColor = Color.Transparent;
            inicioSLinkLabel.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            inicioSLinkLabel.LinkColor = Color.LightBlue;
            inicioSLinkLabel.Location = new Point(647, 473);
            inicioSLinkLabel.Name = "inicioSLinkLabel";
            inicioSLinkLabel.Size = new Size(104, 23);
            inicioSLinkLabel.TabIndex = 9;
            inicioSLinkLabel.TabStop = true;
            inicioSLinkLabel.Text = "Inicia Sesion";
            inicioSLinkLabel.LinkClicked += inicioSLinkLabel_LinkClicked_1;
            // 
            // btnRegistrar
            // 
            btnRegistrar.BackColor = Color.MediumSlateBlue;
            btnRegistrar.BackgroundImage = (Image)resources.GetObject("btnRegistrar.BackgroundImage");
            btnRegistrar.Cursor = Cursors.Hand;
            btnRegistrar.FlatAppearance.BorderSize = 0;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRegistrar.ForeColor = Color.White;
            btnRegistrar.Location = new Point(537, 400);
            btnRegistrar.Margin = new Padding(3, 4, 3, 4);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(171, 53);
            btnRegistrar.TabIndex = 10;
            btnRegistrar.Text = "Registrarse";
            btnRegistrar.UseVisualStyleBackColor = false;
            btnRegistrar.Click += btnRegistrar_Click;
            // 
            // Register
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1248, 719);
            Controls.Add(btnRegistrar);
            Controls.Add(inicioSLinkLabel);
            Controls.Add(Labela);
            Controls.Add(txtContrasena);
            Controls.Add(label4);
            Controls.Add(txtEmail);
            Controls.Add(label3);
            Controls.Add(txtNombreUsuario);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Register";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Register";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtNombreUsuario;
        private TextBox txtEmail;
        private Label label3;
        private TextBox txtContrasena;
        private Label label4;
        private Label Labela;
        private LinkLabel inicioSLinkLabel;
        private customControl btnRegistrar;
    }
}