namespace ProyectScrum.Forms
{
    partial class SubidaMangaForm
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
            label1 = new Label();
            pictureBoxPortada = new PictureBox();
            txtTitulo = new TextBox();
            label2 = new Label();
            txtTituloAlt = new TextBox();
            label3 = new Label();
            txtAutor = new TextBox();
            label4 = new Label();
            txtDescripcion = new TextBox();
            label5 = new Label();
            cmbEstado = new ComboBox();
            label6 = new Label();
            dtpFecha = new DateTimePicker();
            label7 = new Label();
            cmbGenero = new ComboBox();
            btnSeleccionarPortada = new Button();
            btnSeleccionarPDFs = new Button();
            panelTomos = new Panel();
            btnModificar = new Button();
            panelSeleccionManga = new Panel();
            btnGuardar = new Button();
            btnSubir = new Button();
            BtnCancelar = new Button();
            btnEliminar = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPortada).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(312, 9);
            label1.Name = "label1";
            label1.Size = new Size(58, 20);
            label1.TabIndex = 0;
            label1.Text = "Titulo: ";
            // 
            // pictureBoxPortada
            // 
            pictureBoxPortada.Location = new Point(53, 9);
            pictureBoxPortada.Name = "pictureBoxPortada";
            pictureBoxPortada.Size = new Size(213, 311);
            pictureBoxPortada.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPortada.TabIndex = 1;
            pictureBoxPortada.TabStop = false;
            // 
            // txtTitulo
            // 
            txtTitulo.Location = new Point(312, 32);
            txtTitulo.Name = "txtTitulo";
            txtTitulo.Size = new Size(443, 23);
            txtTitulo.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(312, 70);
            label2.Name = "label2";
            label2.Size = new Size(216, 20);
            label2.TabIndex = 3;
            label2.Text = "Titulo Alternativo (opcional): ";
            // 
            // txtTituloAlt
            // 
            txtTituloAlt.Location = new Point(312, 93);
            txtTituloAlt.Name = "txtTituloAlt";
            txtTituloAlt.Size = new Size(443, 23);
            txtTituloAlt.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(312, 134);
            label3.Name = "label3";
            label3.Size = new Size(55, 20);
            label3.TabIndex = 5;
            label3.Text = "Autor:";
            // 
            // txtAutor
            // 
            txtAutor.Location = new Point(312, 157);
            txtAutor.Name = "txtAutor";
            txtAutor.Size = new Size(443, 23);
            txtAutor.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(312, 199);
            label4.Name = "label4";
            label4.Size = new Size(94, 20);
            label4.TabIndex = 7;
            label4.Text = "Descripcíon:";
            // 
            // txtDescripcion
            // 
            txtDescripcion.Location = new Point(312, 222);
            txtDescripcion.Multiline = true;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(443, 60);
            txtDescripcion.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(310, 306);
            label5.Name = "label5";
            label5.Size = new Size(60, 20);
            label5.TabIndex = 9;
            label5.Text = "Estado:";
            // 
            // cmbEstado
            // 
            cmbEstado.FormattingEnabled = true;
            cmbEstado.Items.AddRange(new object[] { "En publicación", "Finalizado", "Pausado" });
            cmbEstado.Location = new Point(312, 326);
            cmbEstado.Name = "cmbEstado";
            cmbEstado.Size = new Size(132, 23);
            cmbEstado.TabIndex = 10;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.White;
            label6.Location = new Point(459, 303);
            label6.Name = "label6";
            label6.Size = new Size(157, 20);
            label6.TabIndex = 11;
            label6.Text = "Fecha de Publicacion:";
            // 
            // dtpFecha
            // 
            dtpFecha.Location = new Point(459, 326);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(157, 23);
            dtpFecha.TabIndex = 12;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.White;
            label7.Location = new Point(637, 303);
            label7.Name = "label7";
            label7.Size = new Size(64, 20);
            label7.TabIndex = 13;
            label7.Text = "Genero:";
            // 
            // cmbGenero
            // 
            cmbGenero.FormattingEnabled = true;
            cmbGenero.Location = new Point(634, 326);
            cmbGenero.Name = "cmbGenero";
            cmbGenero.Size = new Size(121, 23);
            cmbGenero.TabIndex = 14;
            // 
            // btnSeleccionarPortada
            // 
            btnSeleccionarPortada.Cursor = Cursors.Hand;
            btnSeleccionarPortada.FlatStyle = FlatStyle.Flat;
            btnSeleccionarPortada.ForeColor = Color.White;
            btnSeleccionarPortada.Location = new Point(53, 326);
            btnSeleccionarPortada.Name = "btnSeleccionarPortada";
            btnSeleccionarPortada.Size = new Size(213, 32);
            btnSeleccionarPortada.TabIndex = 15;
            btnSeleccionarPortada.Text = "Elegir Imagen";
            btnSeleccionarPortada.UseVisualStyleBackColor = true;
            btnSeleccionarPortada.Click += btnElegirImagen_Click;
            // 
            // btnSeleccionarPDFs
            // 
            btnSeleccionarPDFs.Cursor = Cursors.Hand;
            btnSeleccionarPDFs.FlatStyle = FlatStyle.Flat;
            btnSeleccionarPDFs.ForeColor = Color.White;
            btnSeleccionarPDFs.Location = new Point(53, 456);
            btnSeleccionarPDFs.Name = "btnSeleccionarPDFs";
            btnSeleccionarPDFs.Size = new Size(213, 32);
            btnSeleccionarPDFs.TabIndex = 17;
            btnSeleccionarPDFs.Text = "Elegir Tomos";
            btnSeleccionarPDFs.UseVisualStyleBackColor = true;
            btnSeleccionarPDFs.Click += btnElegirTomos_Click;
            // 
            // panelTomos
            // 
            panelTomos.Location = new Point(53, 374);
            panelTomos.Name = "panelTomos";
            panelTomos.Size = new Size(475, 76);
            panelTomos.TabIndex = 19;
            // 
            // btnModificar
            // 
            btnModificar.BackColor = Color.FromArgb(0, 0, 192);
            btnModificar.Cursor = Cursors.Hand;
            btnModificar.FlatStyle = FlatStyle.Flat;
            btnModificar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnModificar.ForeColor = Color.White;
            btnModificar.Location = new Point(542, 412);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(213, 32);
            btnModificar.TabIndex = 20;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = false;
            btnModificar.Click += btnModificar_Click;
            // 
            // panelSeleccionManga
            // 
            panelSeleccionManga.AutoScroll = true;
            panelSeleccionManga.BackColor = Color.Black;
            panelSeleccionManga.Location = new Point(459, 101);
            panelSeleccionManga.Name = "panelSeleccionManga";
            panelSeleccionManga.Size = new Size(363, 387);
            panelSeleccionManga.TabIndex = 21;
            panelSeleccionManga.Visible = false;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.Black;
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderColor = Color.LawnGreen;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGuardar.ForeColor = Color.LawnGreen;
            btnGuardar.Location = new Point(542, 374);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(213, 32);
            btnGuardar.TabIndex = 22;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Visible = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnSubir
            // 
            btnSubir.BackColor = Color.Green;
            btnSubir.Cursor = Cursors.Hand;
            btnSubir.FlatStyle = FlatStyle.Flat;
            btnSubir.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSubir.ForeColor = Color.White;
            btnSubir.Location = new Point(542, 374);
            btnSubir.Name = "btnSubir";
            btnSubir.Size = new Size(213, 32);
            btnSubir.TabIndex = 18;
            btnSubir.Text = "Agregar";
            btnSubir.UseVisualStyleBackColor = false;
            btnSubir.Click += btnSubir_Click;
            // 
            // BtnCancelar
            // 
            BtnCancelar.BackColor = Color.Black;
            BtnCancelar.Cursor = Cursors.Hand;
            BtnCancelar.FlatAppearance.BorderColor = Color.LightGray;
            BtnCancelar.FlatStyle = FlatStyle.Flat;
            BtnCancelar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnCancelar.ForeColor = Color.LightGray;
            BtnCancelar.Location = new Point(542, 412);
            BtnCancelar.Name = "BtnCancelar";
            BtnCancelar.Size = new Size(213, 32);
            BtnCancelar.TabIndex = 23;
            BtnCancelar.Text = "Cancelar";
            BtnCancelar.UseVisualStyleBackColor = false;
            BtnCancelar.Visible = false;
            BtnCancelar.Click += BtnCancelar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.Black;
            btnEliminar.Cursor = Cursors.Hand;
            btnEliminar.FlatAppearance.BorderColor = Color.IndianRed;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEliminar.ForeColor = Color.IndianRed;
            btnEliminar.Location = new Point(542, 450);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(213, 32);
            btnEliminar.TabIndex = 24;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Visible = false;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // SubidaMangaForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(882, 500);
            Controls.Add(btnEliminar);
            Controls.Add(BtnCancelar);
            Controls.Add(btnGuardar);
            Controls.Add(panelSeleccionManga);
            Controls.Add(btnModificar);
            Controls.Add(panelTomos);
            Controls.Add(btnSubir);
            Controls.Add(btnSeleccionarPDFs);
            Controls.Add(btnSeleccionarPortada);
            Controls.Add(cmbGenero);
            Controls.Add(label7);
            Controls.Add(dtpFecha);
            Controls.Add(label6);
            Controls.Add(cmbEstado);
            Controls.Add(label5);
            Controls.Add(txtDescripcion);
            Controls.Add(label4);
            Controls.Add(txtAutor);
            Controls.Add(label3);
            Controls.Add(txtTituloAlt);
            Controls.Add(label2);
            Controls.Add(txtTitulo);
            Controls.Add(pictureBoxPortada);
            Controls.Add(label1);
            Name = "SubidaMangaForm";
            Text = "SubidaMangaForm";
            ((System.ComponentModel.ISupportInitialize)pictureBoxPortada).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private PictureBox pictureBoxPortada;
        private TextBox txtTitulo;
        private Label label2;
        private TextBox txtTituloAlt;
        private Label label3;
        private TextBox txtAutor;
        private Label label4;
        private TextBox txtDescripcion;
        private Label label5;
        private ComboBox cmbEstado;
        private Label label6;
        private DateTimePicker dtpFecha;
        private Label label7;
        private ComboBox cmbGenero;
        private Button btnSeleccionarPortada;
        private Button btnSeleccionarPDFs;
        private Panel panelTomos;
        private Button btnModificar;
        private Panel panelSeleccionManga;
        private Button btnSubir;
        private Button btnGuardar;
        private Button BtnCancelar;
        private Button btnEliminar;
    }
}