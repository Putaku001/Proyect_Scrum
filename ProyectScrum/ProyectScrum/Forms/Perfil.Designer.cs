namespace ProyectScrum.Forms
{
    partial class Perfil
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
            pictureBoxAvatar = new PictureBox();
            btnAvatarAnterior = new Button();
            btnAvatarSiguiente = new Button();
            label1 = new Label();
            txtNombre = new TextBox();
            label2 = new Label();
            txtEmail = new TextBox();
            labelEsPremium = new Label();
            btnGuardar = new Button();
            btnCambiarContraseña = new Button();
            btnEliminarCuenta = new Button();
            btnGestionarSuscripcion = new Button();
            label3 = new Label();
            panel1 = new Panel();
            lblFechaFinSuscripcion = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAvatar).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxAvatar
            // 
            pictureBoxAvatar.Anchor = AnchorStyles.None;
            pictureBoxAvatar.BackColor = Color.White;
            pictureBoxAvatar.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxAvatar.Location = new Point(162, 125);
            pictureBoxAvatar.Name = "pictureBoxAvatar";
            pictureBoxAvatar.Padding = new Padding(5);
            pictureBoxAvatar.Size = new Size(180, 180);
            pictureBoxAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxAvatar.TabIndex = 0;
            pictureBoxAvatar.TabStop = false;
            // 
            // btnAvatarAnterior
            // 
            btnAvatarAnterior.Anchor = AnchorStyles.None;
            btnAvatarAnterior.BackColor = Color.LightGray;
            btnAvatarAnterior.FlatStyle = FlatStyle.Flat;
            btnAvatarAnterior.Font = new Font("Segoe UI", 10F);
            btnAvatarAnterior.Location = new Point(112, 195);
            btnAvatarAnterior.Name = "btnAvatarAnterior";
            btnAvatarAnterior.Size = new Size(40, 40);
            btnAvatarAnterior.TabIndex = 1;
            btnAvatarAnterior.Text = "←";
            btnAvatarAnterior.UseVisualStyleBackColor = false;
            btnAvatarAnterior.Click += btnAvatarAnterior_Click;
            // 
            // btnAvatarSiguiente
            // 
            btnAvatarSiguiente.Anchor = AnchorStyles.None;
            btnAvatarSiguiente.BackColor = Color.LightGray;
            btnAvatarSiguiente.FlatStyle = FlatStyle.Flat;
            btnAvatarSiguiente.Font = new Font("Segoe UI", 10F);
            btnAvatarSiguiente.Location = new Point(352, 195);
            btnAvatarSiguiente.Name = "btnAvatarSiguiente";
            btnAvatarSiguiente.Size = new Size(40, 40);
            btnAvatarSiguiente.TabIndex = 2;
            btnAvatarSiguiente.Text = "→";
            btnAvatarSiguiente.UseVisualStyleBackColor = false;
            btnAvatarSiguiente.Click += btnAvatarSiguiente_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(482, 125);
            label1.Name = "label1";
            label1.Size = new Size(100, 25);
            label1.TabIndex = 3;
            label1.Text = "Nombre";
            // 
            // txtNombre
            // 
            txtNombre.Anchor = AnchorStyles.None;
            txtNombre.BackColor = Color.WhiteSmoke;
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Font = new Font("Segoe UI", 10F);
            txtNombre.Location = new Point(482, 155);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(280, 30);
            txtNombre.TabIndex = 4;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(482, 195);
            label2.Name = "label2";
            label2.Size = new Size(100, 25);
            label2.TabIndex = 5;
            label2.Text = "Email";
            // 
            // txtEmail
            // 
            txtEmail.Anchor = AnchorStyles.None;
            txtEmail.BackColor = Color.WhiteSmoke;
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.Location = new Point(482, 225);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(280, 30);
            txtEmail.TabIndex = 6;
            // 
            // labelEsPremium
            // 
            labelEsPremium.Anchor = AnchorStyles.None;
            labelEsPremium.AutoSize = true;
            labelEsPremium.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            labelEsPremium.ForeColor = Color.White;
            labelEsPremium.Location = new Point(191, 99);
            labelEsPremium.Name = "labelEsPremium";
            labelEsPremium.Size = new Size(137, 23);
            labelEsPremium.TabIndex = 11;
            labelEsPremium.Text = "labelEsPremium";
            // 
            // btnGuardar
            // 
            btnGuardar.Anchor = AnchorStyles.None;
            btnGuardar.BackColor = Color.DarkGreen;
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderColor = Color.Black;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 10F);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(482, 275);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(280, 45);
            btnGuardar.TabIndex = 7;
            btnGuardar.Text = "💾 Guardar Cambios";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCambiarContraseña
            // 
            btnCambiarContraseña.Anchor = AnchorStyles.None;
            btnCambiarContraseña.BackColor = Color.WhiteSmoke;
            btnCambiarContraseña.Cursor = Cursors.Hand;
            btnCambiarContraseña.FlatStyle = FlatStyle.Flat;
            btnCambiarContraseña.Font = new Font("Segoe UI", 10F);
            btnCambiarContraseña.Location = new Point(132, 347);
            btnCambiarContraseña.Name = "btnCambiarContraseña";
            btnCambiarContraseña.Size = new Size(240, 40);
            btnCambiarContraseña.TabIndex = 8;
            btnCambiarContraseña.Text = "🔒 Cambiar Contraseña";
            btnCambiarContraseña.UseVisualStyleBackColor = false;
            btnCambiarContraseña.Click += btnCambiarContraseña_Click;
            // 
            // btnEliminarCuenta
            // 
            btnEliminarCuenta.Anchor = AnchorStyles.None;
            btnEliminarCuenta.BackColor = Color.DarkRed;
            btnEliminarCuenta.Cursor = Cursors.Hand;
            btnEliminarCuenta.FlatAppearance.BorderColor = Color.Black;
            btnEliminarCuenta.FlatStyle = FlatStyle.Flat;
            btnEliminarCuenta.Font = new Font("Segoe UI", 10F);
            btnEliminarCuenta.ForeColor = Color.White;
            btnEliminarCuenta.Location = new Point(482, 335);
            btnEliminarCuenta.Name = "btnEliminarCuenta";
            btnEliminarCuenta.Size = new Size(280, 40);
            btnEliminarCuenta.TabIndex = 9;
            btnEliminarCuenta.Text = "🗑️ Eliminar Cuenta";
            btnEliminarCuenta.UseVisualStyleBackColor = false;
            btnEliminarCuenta.Click += btnEliminarCuenta_Click;
            // 
            // btnGestionarSuscripcion
            // 
            btnGestionarSuscripcion.Anchor = AnchorStyles.None;
            btnGestionarSuscripcion.BackColor = Color.Indigo;
            btnGestionarSuscripcion.Cursor = Cursors.Hand;
            btnGestionarSuscripcion.FlatStyle = FlatStyle.Flat;
            btnGestionarSuscripcion.Font = new Font("Segoe UI", 10F);
            btnGestionarSuscripcion.Location = new Point(132, 393);
            btnGestionarSuscripcion.Name = "btnGestionarSuscripcion";
            btnGestionarSuscripcion.Size = new Size(240, 40);
            btnGestionarSuscripcion.TabIndex = 10;
            btnGestionarSuscripcion.Text = "⭐ Suscripción";
            btnGestionarSuscripcion.UseVisualStyleBackColor = false;
            btnGestionarSuscripcion.Click += btnGestionarSuscripcion_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Black", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(12, 9);
            label3.Name = "label3";
            label3.Size = new Size(151, 31);
            label3.TabIndex = 12;
            label3.Text = "Mi perfil 👤";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.ForeColor = Color.White;
            panel1.Location = new Point(12, 47);
            panel1.Name = "panel1";
            panel1.Size = new Size(967, 10);
            panel1.TabIndex = 13;
            // 
            // lblFechaFinSuscripcion
            // 
            lblFechaFinSuscripcion.AutoSize = true;
            lblFechaFinSuscripcion.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblFechaFinSuscripcion.ForeColor = SystemColors.ButtonFace;
            lblFechaFinSuscripcion.Location = new Point(180, 436);
            lblFechaFinSuscripcion.Name = "lblFechaFinSuscripcion";
            lblFechaFinSuscripcion.Size = new Size(148, 20);
            lblFechaFinSuscripcion.TabIndex = 14;
            lblFechaFinSuscripcion.Text = "labelFinSuscripcion";
            // 
            // Perfil
            // 
            BackColor = Color.Black;
            ClientSize = new Size(991, 498);
            Controls.Add(lblFechaFinSuscripcion);
            Controls.Add(panel1);
            Controls.Add(label3);
            Controls.Add(pictureBoxAvatar);
            Controls.Add(btnAvatarAnterior);
            Controls.Add(btnAvatarSiguiente);
            Controls.Add(label1);
            Controls.Add(txtNombre);
            Controls.Add(label2);
            Controls.Add(txtEmail);
            Controls.Add(btnGuardar);
            Controls.Add(btnCambiarContraseña);
            Controls.Add(btnEliminarCuenta);
            Controls.Add(btnGestionarSuscripcion);
            Controls.Add(labelEsPremium);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Perfil";
            Text = "👤 Mi Perfil";
            Activated += Perfil_Activated;
            ((System.ComponentModel.ISupportInitialize)pictureBoxAvatar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxAvatar;
        private System.Windows.Forms.Button btnAvatarAnterior;
        private System.Windows.Forms.Button btnAvatarSiguiente;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCambiarContraseña;
        private System.Windows.Forms.Button btnEliminarCuenta;
        private System.Windows.Forms.Button btnGestionarSuscripcion;
        private Label labelEsPremium;
        private Label label3;
        private Panel panel1;
        private Label lblFechaFinSuscripcion;
    }
}