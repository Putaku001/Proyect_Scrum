namespace ProyectScrum.Forms
{
    partial class EditUserForm
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            chkPremium = new CheckBox();
            label5 = new Label();
            cmbRole = new ComboBox();
            btnSave = new Button();
            btnCancel = new Button();
            labelEsPremium = new Label();
            txtUsername = new Guna.UI2.WinForms.Guna2TextBox();
            txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold | FontStyle.Italic);
            label2.ForeColor = Color.White;
            label2.Location = new Point(24, 23);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(52, 15);
            label2.TabIndex = 1;
            label2.Text = "Usuario:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold | FontStyle.Italic);
            label3.ForeColor = Color.White;
            label3.Location = new Point(24, 58);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(40, 15);
            label3.TabIndex = 3;
            label3.Text = "Email:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold | FontStyle.Italic);
            label4.ForeColor = Color.White;
            label4.Location = new Point(24, 92);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(60, 15);
            label4.TabIndex = 5;
            label4.Text = "Premium:";
            // 
            // chkPremium
            // 
            chkPremium.AutoSize = true;
            chkPremium.Location = new Point(116, 92);
            chkPremium.Margin = new Padding(4, 4, 4, 4);
            chkPremium.Name = "chkPremium";
            chkPremium.Size = new Size(15, 14);
            chkPremium.TabIndex = 6;
            chkPremium.UseVisualStyleBackColor = true;
            chkPremium.CheckedChanged += chkPremium_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold | FontStyle.Italic);
            label5.ForeColor = Color.White;
            label5.Location = new Point(24, 127);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(27, 15);
            label5.TabIndex = 7;
            label5.Text = "Rol:";
            // 
            // cmbRole
            // 
            cmbRole.BackColor = Color.White;
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.FormattingEnabled = true;
            cmbRole.Items.AddRange(new object[] { "Usuario", "Administrador" });
            cmbRole.Location = new Point(116, 124);
            cmbRole.Margin = new Padding(4, 4, 4, 4);
            cmbRole.Name = "cmbRole";
            cmbRole.Size = new Size(232, 23);
            cmbRole.TabIndex = 8;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.DarkGreen;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(168, 173);
            btnSave.Margin = new Padding(4, 4, 4, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(88, 26);
            btnSave.TabIndex = 9;
            btnSave.Text = "Guardar";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.DarkRed;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(262, 173);
            btnCancel.Margin = new Padding(4, 4, 4, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 26);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Cancelar";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // labelEsPremium
            // 
            labelEsPremium.AutoSize = true;
            labelEsPremium.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            labelEsPremium.ForeColor = Color.Red;
            labelEsPremium.Location = new Point(138, 90);
            labelEsPremium.Name = "labelEsPremium";
            labelEsPremium.Size = new Size(72, 15);
            labelEsPremium.TabIndex = 11;
            labelEsPremium.Text = "No premium";
            // 
            // txtUsername
            // 
            txtUsername.BorderRadius = 5;
            txtUsername.CustomizableEdges = customizableEdges1;
            txtUsername.DefaultText = "";
            txtUsername.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtUsername.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtUsername.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtUsername.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtUsername.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtUsername.Font = new Font("Segoe UI", 9F);
            txtUsername.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtUsername.Location = new Point(116, 12);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "";
            txtUsername.SelectedText = "";
            txtUsername.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtUsername.Size = new Size(234, 26);
            txtUsername.TabIndex = 12;
            // 
            // txtEmail
            // 
            txtEmail.BorderRadius = 5;
            txtEmail.CustomizableEdges = customizableEdges3;
            txtEmail.DefaultText = "";
            txtEmail.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtEmail.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtEmail.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Font = new Font("Segoe UI", 9F);
            txtEmail.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Location = new Point(116, 47);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "";
            txtEmail.SelectedText = "";
            txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtEmail.Size = new Size(234, 26);
            txtEmail.TabIndex = 13;
            // 
            // EditUserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(2, 5, 20);
            ClientSize = new Size(378, 220);
            Controls.Add(txtEmail);
            Controls.Add(txtUsername);
            Controls.Add(labelEsPremium);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(cmbRole);
            Controls.Add(label5);
            Controls.Add(chkPremium);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 4, 4, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditUserForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Editar Usuario";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkPremium;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private Label labelEsPremium;
        private Guna.UI2.WinForms.Guna2TextBox txtUsername;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
    }
}