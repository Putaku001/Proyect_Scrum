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
            flowPanelVolumenes = new FlowLayoutPanel();
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
            flowLayoutPanel1.Location = new Point(12, 54);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(858, 434);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(labelGenero);
            panel1.Controls.Add(labelGeneroInfo);
            panel1.Controls.Add(picturePortada);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 431);
            panel1.TabIndex = 0;
            // 
            // labelGenero
            // 
            labelGenero.AutoSize = true;
            labelGenero.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelGenero.ForeColor = Color.White;
            labelGenero.Location = new Point(3, 281);
            labelGenero.Name = "labelGenero";
            labelGenero.Size = new Size(78, 17);
            labelGenero.TabIndex = 2;
            labelGenero.Text = "Genero-Info";
            // 
            // labelGeneroInfo
            // 
            labelGeneroInfo.AutoSize = true;
            labelGeneroInfo.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelGeneroInfo.ForeColor = Color.White;
            labelGeneroInfo.Location = new Point(67, 252);
            labelGeneroInfo.Name = "labelGeneroInfo";
            labelGeneroInfo.Size = new Size(51, 17);
            labelGeneroInfo.TabIndex = 1;
            labelGeneroInfo.Text = "Genero";
            // 
            // picturePortada
            // 
            picturePortada.Location = new Point(3, 3);
            picturePortada.Margin = new Padding(10);
            picturePortada.Name = "picturePortada";
            picturePortada.Size = new Size(185, 240);
            picturePortada.SizeMode = PictureBoxSizeMode.Zoom;
            picturePortada.TabIndex = 0;
            picturePortada.TabStop = false;
            // 
            // panel2
            // 
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
            panel2.Location = new Point(209, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(641, 431);
            panel2.TabIndex = 1;
            // 
            // txtTitulo
            // 
            txtTitulo.BackColor = Color.Black;
            txtTitulo.BorderStyle = BorderStyle.None;
            txtTitulo.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtTitulo.ForeColor = Color.White;
            txtTitulo.Location = new Point(-57, -46);
            txtTitulo.Multiline = true;
            txtTitulo.Name = "txtTitulo";
            txtTitulo.ReadOnly = true;
            txtTitulo.ScrollBars = ScrollBars.Vertical;
            txtTitulo.Size = new Size(470, 36);
            txtTitulo.TabIndex = 25;
            // 
            // labelDescripcion
            // 
            labelDescripcion.BackColor = Color.Black;
            labelDescripcion.BorderStyle = BorderStyle.None;
            labelDescripcion.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelDescripcion.ForeColor = Color.White;
            labelDescripcion.Location = new Point(3, 111);
            labelDescripcion.Multiline = true;
            labelDescripcion.Name = "labelDescripcion";
            labelDescripcion.ReadOnly = true;
            labelDescripcion.ScrollBars = ScrollBars.Vertical;
            labelDescripcion.Size = new Size(635, 87);
            labelDescripcion.TabIndex = 7;
            // 
            // labelDescripcionInfo
            // 
            labelDescripcionInfo.AutoSize = true;
            labelDescripcionInfo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelDescripcionInfo.ForeColor = Color.White;
            labelDescripcionInfo.Location = new Point(3, 91);
            labelDescripcionInfo.Name = "labelDescripcionInfo";
            labelDescripcionInfo.Size = new Size(84, 17);
            labelDescripcionInfo.TabIndex = 6;
            labelDescripcionInfo.Text = "Descripción:";
            // 
            // labelFecha
            // 
            labelFecha.AutoSize = true;
            labelFecha.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelFecha.ForeColor = Color.White;
            labelFecha.Location = new Point(50, 63);
            labelFecha.Name = "labelFecha";
            labelFecha.Size = new Size(72, 17);
            labelFecha.TabIndex = 5;
            labelFecha.Text = "Fecha-Info";
            // 
            // labelFechaInfo
            // 
            labelFechaInfo.AutoSize = true;
            labelFechaInfo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelFechaInfo.ForeColor = Color.White;
            labelFechaInfo.Location = new Point(3, 63);
            labelFechaInfo.Name = "labelFechaInfo";
            labelFechaInfo.Size = new Size(47, 17);
            labelFechaInfo.TabIndex = 4;
            labelFechaInfo.Text = "Fecha:";
            // 
            // labelAutor
            // 
            labelAutor.AutoSize = true;
            labelAutor.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelAutor.ForeColor = Color.White;
            labelAutor.Location = new Point(50, 35);
            labelAutor.Name = "labelAutor";
            labelAutor.Size = new Size(71, 17);
            labelAutor.TabIndex = 3;
            labelAutor.Text = "Autor-Info";
            // 
            // labelAutorInfo
            // 
            labelAutorInfo.AutoSize = true;
            labelAutorInfo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelAutorInfo.ForeColor = Color.White;
            labelAutorInfo.Location = new Point(3, 35);
            labelAutorInfo.Name = "labelAutorInfo";
            labelAutorInfo.Size = new Size(48, 17);
            labelAutorInfo.TabIndex = 2;
            labelAutorInfo.Text = "Autor:";
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitulo.ForeColor = Color.White;
            labelTitulo.Location = new Point(50, 9);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(71, 17);
            labelTitulo.TabIndex = 1;
            labelTitulo.Text = "Titulo-Info";
            // 
            // labelTituloInfo
            // 
            labelTituloInfo.AutoSize = true;
            labelTituloInfo.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTituloInfo.ForeColor = Color.White;
            labelTituloInfo.Location = new Point(3, 9);
            labelTituloInfo.Name = "labelTituloInfo";
            labelTituloInfo.Size = new Size(53, 17);
            labelTituloInfo.TabIndex = 0;
            labelTituloInfo.Text = "Titulo: ";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Black;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(96, 6);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(60, 37);
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
            textTitle.Location = new Point(164, 15);
            textTitle.Multiline = true;
            textTitle.Name = "textTitle";
            textTitle.Size = new Size(534, 36);
            textTitle.TabIndex = 25;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = SystemColors.ButtonFace;
            label8.Location = new Point(12, 22);
            label8.Name = "label8";
            label8.Size = new Size(94, 21);
            label8.TabIndex = 27;
            label8.Text = "COMPLETO";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.ForeColor = SystemColors.ButtonFace;
            label7.Location = new Point(12, 4);
            label7.Name = "label7";
            label7.Size = new Size(69, 20);
            label7.TabIndex = 26;
            label7.Text = "Catálago";
            // 
            // flowPanelVolumenes
            // 
            flowPanelVolumenes.AutoScroll = true;
            flowPanelVolumenes.Location = new Point(3, 204);
            flowPanelVolumenes.Name = "flowPanelVolumenes";
            flowPanelVolumenes.Size = new Size(635, 224);
            flowPanelVolumenes.TabIndex = 26;
            // 
            // mangaForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(882, 500);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(textTitle);
            Controls.Add(pictureBox1);
            Controls.Add(flowLayoutPanel1);
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
    }
}