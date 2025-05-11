namespace ProyectScrum.Forms
{
    partial class AdminDashboard
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
            panelStats = new Panel();
            panelAdminUsers = new Panel();
            lblAdminUsers = new Label();
            label10 = new Label();
            panelPremiumUsers = new Panel();
            lblPremiumUsers = new Label();
            label8 = new Label();
            panelTotalUsers = new Panel();
            lblTotalUsers = new Label();
            label6 = new Label();
            label1 = new Label();
            panelRecentUsers = new Panel();
            dataGridRecentUsers = new DataGridView();
            label2 = new Label();
            panelDistribution = new Panel();
            listRoles = new ListBox();
            label3 = new Label();
            panelRegistrations = new Panel();
            listRegistrations = new ListBox();
            label4 = new Label();
            btnRefresh = new Button();
            panelStats.SuspendLayout();
            panelAdminUsers.SuspendLayout();
            panelPremiumUsers.SuspendLayout();
            panelTotalUsers.SuspendLayout();
            panelRecentUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridRecentUsers).BeginInit();
            panelDistribution.SuspendLayout();
            panelRegistrations.SuspendLayout();
            SuspendLayout();
            // 
            // panelStats
            // 
            panelStats.BackColor = Color.White;
            panelStats.Controls.Add(panelAdminUsers);
            panelStats.Controls.Add(panelPremiumUsers);
            panelStats.Controls.Add(panelTotalUsers);
            panelStats.Controls.Add(label1);
            panelStats.Location = new Point(27, 31);
            panelStats.Margin = new Padding(4, 5, 4, 5);
            panelStats.Name = "panelStats";
            panelStats.Size = new Size(987, 185);
            panelStats.TabIndex = 0;
            // 
            // panelAdminUsers
            // 
            panelAdminUsers.BorderStyle = BorderStyle.Fixed3D;
            panelAdminUsers.Controls.Add(lblAdminUsers);
            panelAdminUsers.Controls.Add(label10);
            panelAdminUsers.Location = new Point(667, 62);
            panelAdminUsers.Margin = new Padding(4, 5, 4, 5);
            panelAdminUsers.Name = "panelAdminUsers";
            panelAdminUsers.Size = new Size(267, 92);
            panelAdminUsers.TabIndex = 7;
            // 
            // lblAdminUsers
            // 
            lblAdminUsers.AutoSize = true;
            lblAdminUsers.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAdminUsers.ForeColor = SystemColors.MenuText;
            lblAdminUsers.Location = new Point(107, 31);
            lblAdminUsers.Margin = new Padding(4, 0, 4, 0);
            lblAdminUsers.Name = "lblAdminUsers";
            lblAdminUsers.Size = new Size(27, 29);
            lblAdminUsers.TabIndex = 6;
            lblAdminUsers.Text = "0";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ForeColor = SystemColors.MenuText;
            label10.Location = new Point(27, 15);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(113, 20);
            label10.TabIndex = 5;
            label10.Text = "Usuarios Admin";
            // 
            // panelPremiumUsers
            // 
            panelPremiumUsers.BorderStyle = BorderStyle.Fixed3D;
            panelPremiumUsers.Controls.Add(lblPremiumUsers);
            panelPremiumUsers.Controls.Add(label8);
            panelPremiumUsers.Location = new Point(333, 62);
            panelPremiumUsers.Margin = new Padding(4, 5, 4, 5);
            panelPremiumUsers.Name = "panelPremiumUsers";
            panelPremiumUsers.Size = new Size(267, 92);
            panelPremiumUsers.TabIndex = 4;
            // 
            // lblPremiumUsers
            // 
            lblPremiumUsers.AutoSize = true;
            lblPremiumUsers.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPremiumUsers.ForeColor = SystemColors.MenuText;
            lblPremiumUsers.Location = new Point(107, 31);
            lblPremiumUsers.Margin = new Padding(4, 0, 4, 0);
            lblPremiumUsers.Name = "lblPremiumUsers";
            lblPremiumUsers.Size = new Size(27, 29);
            lblPremiumUsers.TabIndex = 4;
            lblPremiumUsers.Text = "0";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = SystemColors.MenuText;
            label8.Location = new Point(27, 15);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(128, 20);
            label8.TabIndex = 3;
            label8.Text = "Usuarios Premium";
            // 
            // panelTotalUsers
            // 
            panelTotalUsers.BorderStyle = BorderStyle.Fixed3D;
            panelTotalUsers.Controls.Add(lblTotalUsers);
            panelTotalUsers.Controls.Add(label6);
            panelTotalUsers.Location = new Point(27, 62);
            panelTotalUsers.Margin = new Padding(4, 5, 4, 5);
            panelTotalUsers.Name = "panelTotalUsers";
            panelTotalUsers.Size = new Size(267, 92);
            panelTotalUsers.TabIndex = 2;
            // 
            // lblTotalUsers
            // 
            lblTotalUsers.AutoSize = true;
            lblTotalUsers.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalUsers.ForeColor = SystemColors.MenuText;
            lblTotalUsers.Location = new Point(107, 31);
            lblTotalUsers.Margin = new Padding(4, 0, 4, 0);
            lblTotalUsers.Name = "lblTotalUsers";
            lblTotalUsers.Size = new Size(27, 29);
            lblTotalUsers.TabIndex = 2;
            lblTotalUsers.Text = "0";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = SystemColors.MenuText;
            label6.Location = new Point(27, 15);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(102, 20);
            label6.TabIndex = 1;
            label6.Text = "Total Usuarios";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.MenuText;
            label1.Location = new Point(413, 12);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(187, 20);
            label1.TabIndex = 0;
            label1.Text = "Estadísticas Rápidas";
            // 
            // panelRecentUsers
            // 
            panelRecentUsers.BackColor = Color.White;
            panelRecentUsers.Controls.Add(dataGridRecentUsers);
            panelRecentUsers.Controls.Add(label2);
            panelRecentUsers.Location = new Point(27, 246);
            panelRecentUsers.Margin = new Padding(4, 5, 4, 5);
            panelRecentUsers.Name = "panelRecentUsers";
            panelRecentUsers.Size = new Size(480, 308);
            panelRecentUsers.TabIndex = 1;
            // 
            // dataGridRecentUsers
            // 
            dataGridRecentUsers.AllowUserToAddRows = false;
            dataGridRecentUsers.AllowUserToDeleteRows = false;
            dataGridRecentUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridRecentUsers.BackgroundColor = SystemColors.MenuBar;
            dataGridRecentUsers.BorderStyle = BorderStyle.Fixed3D;
            dataGridRecentUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridRecentUsers.GridColor = Color.FromArgb(64, 64, 64);
            dataGridRecentUsers.Location = new Point(13, 62);
            dataGridRecentUsers.Margin = new Padding(4, 5, 4, 5);
            dataGridRecentUsers.Name = "dataGridRecentUsers";
            dataGridRecentUsers.ReadOnly = true;
            dataGridRecentUsers.RowHeadersWidth = 51;
            dataGridRecentUsers.Size = new Size(453, 231);
            dataGridRecentUsers.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.MenuText;
            label2.Location = new Point(151, 15);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(160, 20);
            label2.TabIndex = 0;
            label2.Text = "Últimos Registros";
            // 
            // panelDistribution
            // 
            panelDistribution.BackColor = Color.White;
            panelDistribution.Controls.Add(listRoles);
            panelDistribution.Controls.Add(label3);
            panelDistribution.Location = new Point(533, 246);
            panelDistribution.Margin = new Padding(4, 5, 4, 5);
            panelDistribution.Name = "panelDistribution";
            panelDistribution.Size = new Size(480, 308);
            panelDistribution.TabIndex = 2;
            // 
            // listRoles
            // 
            listRoles.BackColor = SystemColors.MenuBar;
            listRoles.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listRoles.FormattingEnabled = true;
            listRoles.ItemHeight = 18;
            listRoles.Location = new Point(13, 62);
            listRoles.Margin = new Padding(4, 5, 4, 5);
            listRoles.Name = "listRoles";
            listRoles.Size = new Size(453, 202);
            listRoles.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.MenuText;
            label3.Location = new Point(188, 15);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(151, 20);
            label3.TabIndex = 0;
            label3.Text = "Usuarios por Rol";
            // 
            // panelRegistrations
            // 
            panelRegistrations.BackColor = Color.White;
            panelRegistrations.Controls.Add(listRegistrations);
            panelRegistrations.Controls.Add(label4);
            panelRegistrations.Location = new Point(27, 585);
            panelRegistrations.Margin = new Padding(4, 5, 4, 5);
            panelRegistrations.Name = "panelRegistrations";
            panelRegistrations.Size = new Size(480, 308);
            panelRegistrations.TabIndex = 3;
            // 
            // listRegistrations
            // 
            listRegistrations.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listRegistrations.FormattingEnabled = true;
            listRegistrations.ItemHeight = 18;
            listRegistrations.Location = new Point(13, 62);
            listRegistrations.Margin = new Padding(4, 5, 4, 5);
            listRegistrations.Name = "listRegistrations";
            listRegistrations.Size = new Size(453, 202);
            listRegistrations.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = SystemColors.MenuText;
            label4.Location = new Point(147, 14);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(164, 20);
            label4.TabIndex = 0;
            label4.Text = "Registros por Mes";
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRefresh.BackColor = SystemColors.HotTrack;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(801, 862);
            btnRefresh.Margin = new Padding(4, 5, 4, 5);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(213, 46);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "Actualizar";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // AdminDashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1040, 923);
            Controls.Add(btnRefresh);
            Controls.Add(panelRegistrations);
            Controls.Add(panelDistribution);
            Controls.Add(panelRecentUsers);
            Controls.Add(panelStats);
            Margin = new Padding(4, 5, 4, 5);
            Name = "AdminDashboard";
            Text = "Panel de Control";
            panelStats.ResumeLayout(false);
            panelStats.PerformLayout();
            panelAdminUsers.ResumeLayout(false);
            panelAdminUsers.PerformLayout();
            panelPremiumUsers.ResumeLayout(false);
            panelPremiumUsers.PerformLayout();
            panelTotalUsers.ResumeLayout(false);
            panelTotalUsers.PerformLayout();
            panelRecentUsers.ResumeLayout(false);
            panelRecentUsers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridRecentUsers).EndInit();
            panelDistribution.ResumeLayout(false);
            panelDistribution.PerformLayout();
            panelRegistrations.ResumeLayout(false);
            panelRegistrations.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Panel panelAdminUsers;
        private System.Windows.Forms.Label lblAdminUsers;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panelPremiumUsers;
        private System.Windows.Forms.Label lblPremiumUsers;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panelTotalUsers;
        private System.Windows.Forms.Label lblTotalUsers;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelRecentUsers;
        private System.Windows.Forms.DataGridView dataGridRecentUsers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelDistribution;
        private System.Windows.Forms.ListBox listRoles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelRegistrations;
        private System.Windows.Forms.ListBox listRegistrations;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRefresh;
    }
}