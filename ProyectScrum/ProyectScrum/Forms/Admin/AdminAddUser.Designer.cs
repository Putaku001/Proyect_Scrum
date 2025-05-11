namespace ProyectScrum.Forms
{
    partial class AdminAddUser
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
            panel1 = new Panel();
            labelEsPremium = new Label();
            label1 = new Label();
            btnAdd = new Button();
            cmbRole = new ComboBox();
            label7 = new Label();
            chkPremium = new CheckBox();
            label6 = new Label();
            txtConfirmPassword = new TextBox();
            label5 = new Label();
            txtPassword = new TextBox();
            label4 = new Label();
            txtEmail = new TextBox();
            label3 = new Label();
            txtUsername = new TextBox();
            label2 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.None;
            panel1.BackColor = Color.Snow;
            panel1.Controls.Add(labelEsPremium);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnAdd);
            panel1.Controls.Add(cmbRole);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(chkPremium);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(txtConfirmPassword);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(txtPassword);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(txtEmail);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(txtUsername);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(109, 81);
            panel1.Name = "panel1";
            panel1.Size = new Size(483, 574);
            panel1.TabIndex = 0;
            // 
            // labelEsPremium
            // 
            labelEsPremium.AutoSize = true;
            labelEsPremium.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            labelEsPremium.ForeColor = Color.Red;
            labelEsPremium.Location = new Point(219, 309);
            labelEsPremium.Name = "labelEsPremium";
            labelEsPremium.Size = new Size(90, 20);
            labelEsPremium.TabIndex = 30;
            labelEsPremium.Text = "No premium";
            // 
            // label1
            // 
            label1.BackColor = Color.DarkSlateGray;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI Black", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(483, 68);
            label1.TabIndex = 29;
            label1.Text = "Datos del usuario";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.None;
            btnAdd.BackColor = Color.ForestGreen;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(21, 447);
            btnAdd.Margin = new Padding(4, 5, 4, 5);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(438, 46);
            btnAdd.TabIndex = 27;
            btnAdd.Text = "Agregar";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click_1;
            // 
            // cmbRole
            // 
            cmbRole.Anchor = AnchorStyles.None;
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.FormattingEnabled = true;
            cmbRole.Items.AddRange(new object[] { "Usuario", "Administrador" });
            cmbRole.Location = new Point(194, 354);
            cmbRole.Margin = new Padding(4, 5, 4, 5);
            cmbRole.Name = "cmbRole";
            cmbRole.Size = new Size(265, 28);
            cmbRole.TabIndex = 26;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.None;
            label7.AutoSize = true;
            label7.BackColor = Color.White;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Italic);
            label7.ForeColor = Color.Black;
            label7.Location = new Point(21, 359);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(37, 20);
            label7.TabIndex = 25;
            label7.Text = "Rol:";
            // 
            // chkPremium
            // 
            chkPremium.Anchor = AnchorStyles.None;
            chkPremium.AutoSize = true;
            chkPremium.Location = new Point(194, 312);
            chkPremium.Margin = new Padding(4, 5, 4, 5);
            chkPremium.Name = "chkPremium";
            chkPremium.Size = new Size(18, 17);
            chkPremium.TabIndex = 24;
            chkPremium.UseVisualStyleBackColor = true;
            chkPremium.CheckedChanged += chkPremium_CheckedChanged;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.None;
            label6.AutoSize = true;
            label6.BackColor = Color.White;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Italic);
            label6.ForeColor = Color.Black;
            label6.Location = new Point(21, 312);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(78, 20);
            label6.TabIndex = 23;
            label6.Text = "Premium:";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Anchor = AnchorStyles.None;
            txtConfirmPassword.Location = new Point(194, 262);
            txtConfirmPassword.Margin = new Padding(4, 5, 4, 5);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '*';
            txtConfirmPassword.Size = new Size(265, 27);
            txtConfirmPassword.TabIndex = 22;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.None;
            label5.AutoSize = true;
            label5.BackColor = Color.White;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Italic);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(21, 266);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(173, 20);
            label5.TabIndex = 21;
            label5.Text = "Confirmar Contraseña:";
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.None;
            txtPassword.Location = new Point(194, 215);
            txtPassword.Margin = new Padding(4, 5, 4, 5);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(265, 27);
            txtPassword.TabIndex = 20;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.None;
            label4.AutoSize = true;
            label4.BackColor = Color.White;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Italic);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(21, 220);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(95, 20);
            label4.TabIndex = 19;
            label4.Text = "Contraseña:";
            // 
            // txtEmail
            // 
            txtEmail.Anchor = AnchorStyles.None;
            txtEmail.Location = new Point(194, 169);
            txtEmail.Margin = new Padding(4, 5, 4, 5);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(265, 27);
            txtEmail.TabIndex = 18;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.BackColor = Color.White;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Italic);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(21, 174);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(54, 20);
            label3.TabIndex = 17;
            label3.Text = "Email:";
            // 
            // txtUsername
            // 
            txtUsername.Anchor = AnchorStyles.None;
            txtUsername.Location = new Point(194, 123);
            txtUsername.Margin = new Padding(4, 5, 4, 5);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(265, 27);
            txtUsername.TabIndex = 16;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.BackColor = Color.White;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Italic);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(21, 128);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(152, 20);
            label2.TabIndex = 15;
            label2.Text = "Nombre de Usuario:";
            // 
            // AdminAddUser
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(710, 731);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AdminAddUser";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Agregar Nuevo Usuario";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnAdd;
        private ComboBox cmbRole;
        private Label label7;
        private CheckBox chkPremium;
        private Label label6;
        private TextBox txtConfirmPassword;
        private Label label5;
        private TextBox txtPassword;
        private Label label4;
        private TextBox txtEmail;
        private Label label3;
        private TextBox txtUsername;
        private Label label2;
        private Label label1;
        private Label labelEsPremium;
    }
}