namespace ProyectScrum.Forms
{
    partial class New_Password
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(New_Password));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            customControl1 = new customControl();
            label4 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonFace;
            label1.Location = new Point(240, 9);
            label1.Name = "label1";
            label1.Size = new Size(335, 45);
            label1.TabIndex = 0;
            label1.Text = "Cambia Tu Cotraseña";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(169, 105);
            label2.Name = "label2";
            label2.Size = new Size(261, 25);
            label2.TabIndex = 1;
            label2.Text = "Escribe tu Nueva Contraseña";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(169, 222);
            label3.Name = "label3";
            label3.Size = new Size(0, 15);
            label3.TabIndex = 2;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(169, 137);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(406, 23);
            textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(169, 252);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(406, 23);
            textBox2.TabIndex = 4;
            // 
            // customControl1
            // 
            customControl1.BackColor = Color.Transparent;
            customControl1.BackgroundImage = (Image)resources.GetObject("customControl1.BackgroundImage");
            customControl1.FlatAppearance.BorderSize = 0;
            customControl1.FlatStyle = FlatStyle.Flat;
            customControl1.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            customControl1.ForeColor = Color.White;
            customControl1.Location = new Point(169, 316);
            customControl1.Name = "customControl1";
            customControl1.Size = new Size(406, 40);
            customControl1.TabIndex = 5;
            customControl1.Text = "Confirmar Cambio";
            customControl1.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label4.ForeColor = SystemColors.ButtonHighlight;
            label4.Location = new Point(169, 214);
            label4.Name = "label4";
            label4.Size = new Size(221, 25);
            label4.TabIndex = 6;
            label4.Text = "Confirma La Contraseña";
            // 
            // New_Password
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(800, 450);
            Controls.Add(label4);
            Controls.Add(customControl1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "New_Password";
            Text = "New_Password";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBox1;
        private TextBox textBox2;
        private customControl customControl1;
        private Label label4;
    }
}