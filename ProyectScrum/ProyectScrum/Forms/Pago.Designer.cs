namespace ProyectScrum.Forms
{
    partial class Pago
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pago));
            label1 = new Label();
            txtNombre = new TextBox();
            txtCVV = new TextBox();
            txtNumero = new TextBox();
            txtFechaExp = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            btnConfirmar = new customControl();
            lblResumen = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonFace;
            label1.Location = new Point(498, 58);
            label1.Name = "label1";
            label1.Size = new Size(322, 28);
            label1.TabIndex = 0;
            label1.Text = "Introduce los datos de tu tarjeta ";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(498, 152);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(316, 27);
            txtNombre.TabIndex = 1;
            // 
            // txtCVV
            // 
            txtCVV.Location = new Point(498, 391);
            txtCVV.Name = "txtCVV";
            txtCVV.Size = new Size(316, 27);
            txtCVV.TabIndex = 2;
            // 
            // txtNumero
            // 
            txtNumero.Location = new Point(498, 231);
            txtNumero.Name = "txtNumero";
            txtNumero.Size = new Size(316, 27);
            txtNumero.TabIndex = 3;
            // 
            // txtFechaExp
            // 
            txtFechaExp.Location = new Point(498, 314);
            txtFechaExp.Name = "txtFechaExp";
            txtFechaExp.Size = new Size(316, 27);
            txtFechaExp.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonFace;
            label2.Location = new Point(492, 360);
            label2.Name = "label2";
            label2.Size = new Size(51, 28);
            label2.TabIndex = 6;
            label2.Text = "CVV";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.ButtonFace;
            label3.Location = new Point(498, 200);
            label3.Name = "label3";
            label3.Size = new Size(185, 28);
            label3.TabIndex = 7;
            label3.Text = "Numero de tarjeta";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label4.ForeColor = SystemColors.ButtonFace;
            label4.Location = new Point(498, 121);
            label4.Name = "label4";
            label4.Size = new Size(209, 28);
            label4.TabIndex = 8;
            label4.Text = "Nombre de la tarjeta";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label5.ForeColor = SystemColors.ButtonFace;
            label5.Location = new Point(498, 283);
            label5.Name = "label5";
            label5.Size = new Size(83, 28);
            label5.TabIndex = 9;
            label5.Text = "MM/AA";
            // 
            // btnConfirmar
            // 
            btnConfirmar.BackColor = Color.MediumSlateBlue;
            btnConfirmar.BackgroundImage = (Image)resources.GetObject("btnConfirmar.BackgroundImage");
            btnConfirmar.Cursor = Cursors.Hand;
            btnConfirmar.FlatAppearance.BorderSize = 0;
            btnConfirmar.FlatStyle = FlatStyle.Flat;
            btnConfirmar.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.Location = new Point(577, 441);
            btnConfirmar.Margin = new Padding(3, 4, 3, 4);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(171, 53);
            btnConfirmar.TabIndex = 11;
            btnConfirmar.Text = "Confirmar Pago";
            btnConfirmar.UseVisualStyleBackColor = false;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // lblResumen
            // 
            lblResumen.AutoSize = true;
            lblResumen.BackColor = Color.Transparent;
            lblResumen.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblResumen.ForeColor = SystemColors.ButtonFace;
            lblResumen.Location = new Point(460, 524);
            lblResumen.Name = "lblResumen";
            lblResumen.Size = new Size(96, 28);
            lblResumen.TabIndex = 12;
            lblResumen.Text = "Resumen";
            // 
            // Pago
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.menu;
            ClientSize = new Size(1283, 727);
            Controls.Add(lblResumen);
            Controls.Add(btnConfirmar);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtFechaExp);
            Controls.Add(txtNumero);
            Controls.Add(txtCVV);
            Controls.Add(txtNombre);
            Controls.Add(label1);
            Name = "Pago";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pago";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtNombre;
        private TextBox txtCVV;
        private TextBox txtNumero;
        private TextBox txtFechaExp;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private customControl btnConfirmar;
        private Label lblResumen;
    }
}