namespace ProyectScrum.Forms
{
    partial class mangaForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mangaForm));
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel1 = new Panel();
            labelGenero = new Label();
            labelGeneroInfo = new Label();
            picturePortada = new PictureBox();
            panel2 = new Panel();
            labelList = new Label();
            flowPanelVolumenes = new FlowLayoutPanel();
            txtTitulo = new TextBox();
            labelDescripcion = new TextBox();
            labelDescripcionInfo = new Label();
            labelFecha = new Label();
            labelFechaInfo = new Label();
            labelAutor = new Label();
            labelAutorInfo = new Label();
            labelTitulo = new Label();
            labelTituloInfo = new Label();
            pictureBox1 = new PictureBox();
            textTitle = new TextBox();
            label8 = new Label();
            label7 = new Label();
            labelCerrar = new Label();
            btnAgregarFavoritos = new Button();
            flowLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picturePortada).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(panel1);
            flowLayoutPanel1.Controls.Add(panel2);
            flowLayoutPanel1.Location = new Point(14, 72);
            flowLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(981, 579);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnAgregarFavoritos);
            panel1.Controls.Add(labelGenero);
            panel1.Controls.Add(labelGeneroInfo);
            panel1.Controls.Add(picturePortada);
            panel1.Location = new Point(3, 4);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(229, 575);
            panel1.TabIndex = 0;
            // 
            // labelGenero
            // 
            labelGenero.AutoSize = true;
            labelGenero.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelGenero.ForeColor = Color.White;
            labelGenero.Location = new Point(59, 370);
            labelGenero.Name = "labelGenero";
            labelGenero.Size = new Size(103, 23);
            labelGenero.TabIndex = 2;
            labelGenero.Text = "Genero-Info";
            // 
            // labelGeneroInfo
            // 
            labelGeneroInfo.AutoSize = true;
            labelGeneroInfo.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelGeneroInfo.ForeColor = Color.White;
            labelGeneroInfo.Location = new Point(77, 336);
            labelGeneroInfo.Name = "labelGeneroInfo";
            labelGeneroInfo.Size = new Size(66, 23);
            labelGeneroInfo.TabIndex = 1;
            labelGeneroInfo.Text = "Genero";
            // 
            // picturePortada
            // 
            picturePortada.Location = new Point(3, 4);
            picturePortada.Margin = new Padding(11, 13, 11, 13);
            picturePortada.Name = "picturePortada";
            picturePortada.Size = new Size(211, 320);
            picturePortada.SizeMode = PictureBoxSizeMode.Zoom;
            picturePortada.TabIndex = 0;
            picturePortada.TabStop = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(labelList);
            panel2.Controls.Add(flowPanelVolumenes);
            panel2.Controls.Add(txtTitulo);
            panel2.Controls.Add(labelDescripcion);
            panel2.Controls.Add(labelDescripcionInfo);
            panel2.Controls.Add(labelFecha);
            panel2.Controls.Add(labelFechaInfo);
            panel2.Controls.Add(labelAutor);
            panel2.Controls.Add(labelAutorInfo);
            panel2.Controls.Add(labelTitulo);
            panel2.Controls.Add(labelTituloInfo);
            panel2.Location = new Point(238, 4);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(733, 575);
            panel2.TabIndex = 1;
            // 
            // labelList
            // 
            labelList.AutoSize = true;
            labelList.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelList.ForeColor = Color.White;
            labelList.Location = new Point(293, 268);
            labelList.Name = "labelList";
            labelList.Size = new Size(152, 28);
            labelList.TabIndex = 27;
            labelList.Text = "Lista de Tomos";
            // 
            // flowPanelVolumenes
            // 
            flowPanelVolumenes.AutoScroll = true;
            flowPanelVolumenes.Location = new Point(3, 300);
            flowPanelVolumenes.Margin = new Padding(3, 4, 3, 4);
            flowPanelVolumenes.Name = "flowPanelVolumenes";
            flowPanelVolumenes.Size = new Size(726, 271);
            flowPanelVolumenes.TabIndex = 26;
            // 
            // txtTitulo
            // 
            txtTitulo.BackColor = Color.Black;
            txtTitulo.BorderStyle = BorderStyle.None;
            txtTitulo.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtTitulo.ForeColor = Color.White;
            txtTitulo.Location = new Point(-65, -61);
            txtTitulo.Margin = new Padding(3, 4, 3, 4);
            txtTitulo.Multiline = true;
            txtTitulo.Name = "txtTitulo";
            txtTitulo.ReadOnly = true;
            txtTitulo.ScrollBars = ScrollBars.Vertical;
            txtTitulo.Size = new Size(537, 48);
            txtTitulo.TabIndex = 25;
            // 
            // labelDescripcion
            // 
            labelDescripcion.BackColor = Color.Black;
            labelDescripcion.BorderStyle = BorderStyle.None;
            labelDescripcion.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelDescripcion.ForeColor = Color.White;
            labelDescripcion.Location = new Point(3, 148);
            labelDescripcion.Margin = new Padding(3, 4, 3, 4);
            labelDescripcion.Multiline = true;
            labelDescripcion.Name = "labelDescripcion";
            labelDescripcion.ReadOnly = true;
            labelDescripcion.ScrollBars = ScrollBars.Vertical;
            labelDescripcion.Size = new Size(726, 116);
            labelDescripcion.TabIndex = 7;
            // 
            // labelDescripcionInfo
            // 
            labelDescripcionInfo.AutoSize = true;
            labelDescripcionInfo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelDescripcionInfo.ForeColor = Color.White;
            labelDescripcionInfo.Location = new Point(3, 121);
            labelDescripcionInfo.Name = "labelDescripcionInfo";
            labelDescripcionInfo.Size = new Size(108, 23);
            labelDescripcionInfo.TabIndex = 6;
            labelDescripcionInfo.Text = "Descripción:";
            // 
            // labelFecha
            // 
            labelFecha.AutoSize = true;
            labelFecha.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelFecha.ForeColor = Color.White;
            labelFecha.Location = new Point(57, 84);
            labelFecha.Name = "labelFecha";
            labelFecha.Size = new Size(93, 23);
            labelFecha.TabIndex = 5;
            labelFecha.Text = "Fecha-Info";
            // 
            // labelFechaInfo
            // 
            labelFechaInfo.AutoSize = true;
            labelFechaInfo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelFechaInfo.ForeColor = Color.White;
            labelFechaInfo.Location = new Point(3, 84);
            labelFechaInfo.Name = "labelFechaInfo";
            labelFechaInfo.Size = new Size(60, 23);
            labelFechaInfo.TabIndex = 4;
            labelFechaInfo.Text = "Fecha:";
            // 
            // labelAutor
            // 
            labelAutor.AutoSize = true;
            labelAutor.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelAutor.ForeColor = Color.White;
            labelAutor.Location = new Point(57, 47);
            labelAutor.Name = "labelAutor";
            labelAutor.Size = new Size(90, 23);
            labelAutor.TabIndex = 3;
            labelAutor.Text = "Autor-Info";
            // 
            // labelAutorInfo
            // 
            labelAutorInfo.AutoSize = true;
            labelAutorInfo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelAutorInfo.ForeColor = Color.White;
            labelAutorInfo.Location = new Point(3, 47);
            labelAutorInfo.Name = "labelAutorInfo";
            labelAutorInfo.Size = new Size(62, 23);
            labelAutorInfo.TabIndex = 2;
            labelAutorInfo.Text = "Autor:";
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitulo.ForeColor = Color.White;
            labelTitulo.Location = new Point(57, 12);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(91, 23);
            labelTitulo.TabIndex = 1;
            labelTitulo.Text = "Titulo-Info";
            // 
            // labelTituloInfo
            // 
            labelTituloInfo.AutoSize = true;
            labelTituloInfo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTituloInfo.ForeColor = Color.White;
            labelTituloInfo.Location = new Point(3, 12);
            labelTituloInfo.Name = "labelTituloInfo";
            labelTituloInfo.Size = new Size(67, 23);
            labelTituloInfo.TabIndex = 0;
            labelTituloInfo.Text = "Titulo: ";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Black;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(110, 8);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(69, 49);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 24;
            pictureBox1.TabStop = false;
            // 
            // textTitle
            // 
            textTitle.BackColor = Color.Black;
            textTitle.BorderStyle = BorderStyle.None;
            textTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textTitle.ForeColor = Color.White;
            textTitle.Location = new Point(187, 20);
            textTitle.Margin = new Padding(3, 4, 3, 4);
            textTitle.Multiline = true;
            textTitle.Name = "textTitle";
            textTitle.Size = new Size(610, 48);
            textTitle.TabIndex = 25;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = SystemColors.ButtonFace;
            label8.Location = new Point(14, 29);
            label8.Name = "label8";
            label8.Size = new Size(117, 28);
            label8.TabIndex = 27;
            label8.Text = "COMPLETO";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.ForeColor = SystemColors.ButtonFace;
            label7.Location = new Point(14, 5);
            label7.Name = "label7";
            label7.Size = new Size(82, 25);
            label7.TabIndex = 26;
            label7.Text = "Catálago";
            // 
            // labelCerrar
            // 
            labelCerrar.AutoSize = true;
            labelCerrar.Cursor = Cursors.Hand;
            labelCerrar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelCerrar.ForeColor = Color.White;
            labelCerrar.Location = new Point(934, 20);
            labelCerrar.Name = "labelCerrar";
            labelCerrar.Size = new Size(60, 23);
            labelCerrar.TabIndex = 28;
            labelCerrar.Text = "Cerrar";
            labelCerrar.Click += labelCerrar_Click;
            // 
            // btnAgregarFavoritos
            // 
            btnAgregarFavoritos.BackColor = Color.Black;
            btnAgregarFavoritos.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            btnAgregarFavoritos.ForeColor = SystemColors.ButtonFace;
            btnAgregarFavoritos.Location = new Point(41, 396);
            btnAgregarFavoritos.Name = "btnAgregarFavoritos";
            btnAgregarFavoritos.Size = new Size(147, 57);
            btnAgregarFavoritos.TabIndex = 3;
            btnAgregarFavoritos.Text = "Agregar a Favoritos";
            btnAgregarFavoritos.UseVisualStyleBackColor = false;
            btnAgregarFavoritos.Click += btnAgregarFavoritos_Click;
            // 
            // mangaForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1008, 667);
            Controls.Add(labelCerrar);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(textTitle);
            Controls.Add(pictureBox1);
            Controls.Add(flowLayoutPanel1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "mangaForm";
            Text = "mangaForm";
            flowLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picturePortada).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel1;
        private Panel panel2;
        private Label labelGenero;
        private Label labelGeneroInfo;
        private PictureBox picturePortada;
        private Label labelTitulo;
        private Label labelTituloInfo;
        private Label labelFecha;
        private Label labelFechaInfo;
        private Label labelAutor;
        private Label labelAutorInfo;
        private Label labelDescripcionInfo;
        private TextBox labelDescripcion;
        private TextBox txtTitulo;
        private PictureBox pictureBox1;
        private TextBox textTitle;
        private Label label8;
        private Label label7;
        private FlowLayoutPanel flowPanelVolumenes;
        private Label labelList;
        private Label labelCerrar;
        private Button btnAgregarFavoritos;
    }
}