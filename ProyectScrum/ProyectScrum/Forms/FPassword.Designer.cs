namespace ProyectScrum.Forms
{
    partial class FPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPassword));
            label1 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            customControl1 = new customControl();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(334, 18);
            label1.Name = "label1";
            label1.Size = new Size(381, 45);
            label1.TabIndex = 0;
            label1.Text = "Olvidaste Tu Contraseña";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(391, 215);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(264, 23);
            textBox1.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(301, 151);
            label2.Name = "label2";
            label2.Size = new Size(517, 30);
            label2.TabIndex = 2;
            label2.Text = "Se ha enviado un correo con un codigo de verificacion";
            // 
            // customControl1
            // 
            customControl1.BackColor = Color.Transparent;
            customControl1.BackgroundImage = (Image)resources.GetObject("customControl1.BackgroundImage");
            customControl1.FlatAppearance.BorderSize = 0;
            customControl1.FlatStyle = FlatStyle.Flat;
            customControl1.ForeColor = Color.White;
            customControl1.Location = new Point(449, 266);
            customControl1.Name = "customControl1";
            customControl1.Size = new Size(150, 40);
            customControl1.TabIndex = 4;
            customControl1.Text = "Verificar";
            customControl1.UseVisualStyleBackColor = false;
            // 
            // FPassword
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1092, 539);
            Controls.Add(customControl1);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Name = "FPassword";
            Text = "FPassword";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private customControl customControl1;
    }
}