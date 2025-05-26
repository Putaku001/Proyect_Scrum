namespace ProyectScrum.Forms
{
    partial class inicioForm
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
            label2 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            flowLayoutPanelMangas = new FlowLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblBienvenida = new Label();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonFace;
            label2.Location = new Point(71, -31);
            label2.Name = "label2";
            label2.Size = new Size(258, 25);
            label2.TabIndex = 12;
            label2.Text = "MANGAS RECOMENDADOS";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(2, 5, 20);
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.Controls.Add(flowLayoutPanelMangas, 1, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(882, 511);
            tableLayoutPanel1.TabIndex = 17;
            // 
            // flowLayoutPanelMangas
            // 
            flowLayoutPanelMangas.AutoScroll = true;
            flowLayoutPanelMangas.Dock = DockStyle.Fill;
            flowLayoutPanelMangas.Location = new Point(13, 102);
            flowLayoutPanelMangas.Margin = new Padding(3, 2, 3, 2);
            flowLayoutPanelMangas.Name = "flowLayoutPanelMangas";
            flowLayoutPanelMangas.Size = new Size(856, 407);
            flowLayoutPanelMangas.TabIndex = 17;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(lblBienvenida, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(13, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(856, 94);
            tableLayoutPanel2.TabIndex = 18;
            // 
            // lblBienvenida
            // 
            lblBienvenida.AutoSize = true;
            lblBienvenida.Dock = DockStyle.Fill;
            lblBienvenida.Font = new Font("Segoe UI Black", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBienvenida.ForeColor = SystemColors.ButtonFace;
            lblBienvenida.Location = new Point(231, 0);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(394, 50);
            lblBienvenida.TabIndex = 15;
            lblBienvenida.Text = "¡Bienvenido, Usuario! 👋";
            lblBienvenida.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // inicioForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(882, 511);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(label2);
            Name = "inicioForm";
            Text = "inicioForm";
            Load += inicioForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanelMangas;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblBienvenida;
    }
}