namespace ProyectScrum.Forms
{
    partial class visorForm
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
            btnCerrar = new Button();
            pnlContenedorPdf = new Panel();
            btnHerramientas = new Button();
            panelHerramientas = new Panel();
            btnManga = new Button();
            btnLibro = new Button();
            btnCascada = new Button();
            labelTitle = new Label();
            btnMaximizar = new Button();
            btnSiguiente = new Button();
            btnAnterior = new Button();
            lblContadorPaginas = new Label();
            panelHerramientas.SuspendLayout();
            SuspendLayout();
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(806, 1);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(75, 32);
            btnCerrar.TabIndex = 0;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // pnlContenedorPdf
            // 
            pnlContenedorPdf.Location = new Point(111, 32);
            pnlContenedorPdf.Name = "pnlContenedorPdf";
            pnlContenedorPdf.Size = new Size(665, 448);
            pnlContenedorPdf.TabIndex = 1;
            // 
            // btnHerramientas
            // 
            btnHerramientas.Cursor = Cursors.Hand;
            btnHerramientas.FlatAppearance.BorderSize = 0;
            btnHerramientas.FlatStyle = FlatStyle.Flat;
            btnHerramientas.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHerramientas.ForeColor = Color.White;
            btnHerramientas.Location = new Point(738, 1);
            btnHerramientas.Name = "btnHerramientas";
            btnHerramientas.Size = new Size(75, 30);
            btnHerramientas.TabIndex = 2;
            btnHerramientas.Text = "⚙";
            btnHerramientas.UseVisualStyleBackColor = true;
            btnHerramientas.Click += btnHerramientas_Click;
            // 
            // panelHerramientas
            // 
            panelHerramientas.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panelHerramientas.BackColor = Color.FromArgb(64, 64, 64);
            panelHerramientas.Controls.Add(btnManga);
            panelHerramientas.Controls.Add(btnLibro);
            panelHerramientas.Controls.Add(btnCascada);
            panelHerramientas.Controls.Add(labelTitle);
            panelHerramientas.Location = new Point(558, 12);
            panelHerramientas.Name = "panelHerramientas";
            panelHerramientas.Size = new Size(218, 176);
            panelHerramientas.TabIndex = 0;
            // 
            // btnManga
            // 
            btnManga.FlatStyle = FlatStyle.Flat;
            btnManga.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnManga.ForeColor = Color.White;
            btnManga.Location = new Point(71, 124);
            btnManga.Name = "btnManga";
            btnManga.Size = new Size(75, 29);
            btnManga.TabIndex = 3;
            btnManga.Text = "Manga";
            btnManga.UseVisualStyleBackColor = true;
            btnManga.Click += btnManga_Click;
            // 
            // btnLibro
            // 
            btnLibro.FlatStyle = FlatStyle.Flat;
            btnLibro.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLibro.ForeColor = Color.White;
            btnLibro.Location = new Point(71, 79);
            btnLibro.Name = "btnLibro";
            btnLibro.Size = new Size(75, 29);
            btnLibro.TabIndex = 2;
            btnLibro.Text = "Libro";
            btnLibro.UseVisualStyleBackColor = true;
            btnLibro.Click += btnLibro_Click;
            // 
            // btnCascada
            // 
            btnCascada.FlatStyle = FlatStyle.Flat;
            btnCascada.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCascada.ForeColor = Color.White;
            btnCascada.Location = new Point(71, 33);
            btnCascada.Name = "btnCascada";
            btnCascada.Size = new Size(75, 29);
            btnCascada.TabIndex = 1;
            btnCascada.Text = "Cascada";
            btnCascada.UseVisualStyleBackColor = true;
            btnCascada.Click += btnCascada_Click;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitle.ForeColor = Color.White;
            labelTitle.Location = new Point(47, 1);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(127, 20);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Modo de Lectura";
            // 
            // btnMaximizar
            // 
            btnMaximizar.Cursor = Cursors.Hand;
            btnMaximizar.FlatAppearance.BorderSize = 0;
            btnMaximizar.FlatStyle = FlatStyle.Flat;
            btnMaximizar.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnMaximizar.ForeColor = Color.White;
            btnMaximizar.Location = new Point(642, 1);
            btnMaximizar.Name = "btnMaximizar";
            btnMaximizar.Size = new Size(75, 30);
            btnMaximizar.TabIndex = 6;
            btnMaximizar.Text = "⛶";
            btnMaximizar.UseVisualStyleBackColor = true;
            btnMaximizar.Click += btnMaximizar_Click;
            // 
            // btnSiguiente
            // 
            btnSiguiente.BackColor = Color.Transparent;
            btnSiguiente.FlatAppearance.BorderSize = 0;
            btnSiguiente.FlatStyle = FlatStyle.Flat;
            btnSiguiente.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSiguiente.ForeColor = Color.White;
            btnSiguiente.Location = new Point(782, 91);
            btnSiguiente.Name = "btnSiguiente";
            btnSiguiente.Size = new Size(75, 322);
            btnSiguiente.TabIndex = 4;
            btnSiguiente.Text = ">";
            btnSiguiente.UseVisualStyleBackColor = false;
            btnSiguiente.Click += btnSiguiente_Click;
            // 
            // btnAnterior
            // 
            btnAnterior.BackColor = Color.Transparent;
            btnAnterior.FlatAppearance.BorderSize = 0;
            btnAnterior.FlatStyle = FlatStyle.Flat;
            btnAnterior.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAnterior.ForeColor = Color.White;
            btnAnterior.Location = new Point(30, 91);
            btnAnterior.Name = "btnAnterior";
            btnAnterior.Size = new Size(75, 322);
            btnAnterior.TabIndex = 3;
            btnAnterior.Text = "<";
            btnAnterior.UseVisualStyleBackColor = false;
            btnAnterior.Click += btnAnterior_Click;
            // 
            // lblContadorPaginas
            // 
            lblContadorPaginas.AutoSize = true;
            lblContadorPaginas.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblContadorPaginas.ForeColor = Color.White;
            lblContadorPaginas.Location = new Point(393, 483);
            lblContadorPaginas.Name = "lblContadorPaginas";
            lblContadorPaginas.Size = new Size(36, 17);
            lblContadorPaginas.TabIndex = 5;
            lblContadorPaginas.Text = "num";
            lblContadorPaginas.Visible = false;
            // 
            // visorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(882, 500);
            Controls.Add(panelHerramientas);
            Controls.Add(btnMaximizar);
            Controls.Add(lblContadorPaginas);
            Controls.Add(btnSiguiente);
            Controls.Add(btnAnterior);
            Controls.Add(btnHerramientas);
            Controls.Add(pnlContenedorPdf);
            Controls.Add(btnCerrar);
            Name = "visorForm";
            Text = "visor";
            panelHerramientas.ResumeLayout(false);
            panelHerramientas.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCerrar;
        private Panel pnlContenedorPdf;
        private Button btnHerramientas;
        private Panel panelHerramientas;
        private Button btnManga;
        private Button btnLibro;
        private Button btnCascada;
        private Label labelTitle;
        private Button btnSiguiente;
        private Button btnAnterior;
        private Label lblContadorPaginas;
        private Button btnMaximizar;
    }
}