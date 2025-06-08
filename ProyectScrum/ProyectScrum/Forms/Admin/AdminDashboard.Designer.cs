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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
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
            tableLayoutPanel3 = new TableLayoutPanel();
            panelRegistrations = new Panel();
            label5 = new Label();
            listRegistrations = new ListBox();
            label4 = new Label();
            panelRecentUsers = new Panel();
            dataGridRecentUsers = new DataGridView();
            label2 = new Label();
            panelDistribution = new Panel();
            listRoles = new ListBox();
            label3 = new Label();
            tableLayoutPanel4 = new TableLayoutPanel();
            btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            btnGenerarPDF = new Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panelStats.SuspendLayout();
            panelAdminUsers.SuspendLayout();
            panelPremiumUsers.SuspendLayout();
            panelTotalUsers.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panelRegistrations.SuspendLayout();
            panelRecentUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridRecentUsers).BeginInit();
            panelDistribution.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.9999924F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 971F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0000076F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 867F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1040, 923);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panelStats, 0, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(37, 32);
            tableLayoutPanel2.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 200F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel2.Size = new Size(965, 859);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // panelStats
            // 
            panelStats.BackColor = Color.White;
            panelStats.Controls.Add(panelAdminUsers);
            panelStats.Controls.Add(panelPremiumUsers);
            panelStats.Controls.Add(panelTotalUsers);
            panelStats.Controls.Add(label1);
            panelStats.Dock = DockStyle.Fill;
            panelStats.Location = new Point(5, 5);
            panelStats.Margin = new Padding(5, 5, 5, 5);
            panelStats.Name = "panelStats";
            panelStats.Size = new Size(955, 190);
            panelStats.TabIndex = 20;
            // 
            // panelAdminUsers
            // 
            panelAdminUsers.BorderStyle = BorderStyle.Fixed3D;
            panelAdminUsers.Controls.Add(lblAdminUsers);
            panelAdminUsers.Controls.Add(label10);
            panelAdminUsers.Location = new Point(667, 61);
            panelAdminUsers.Margin = new Padding(5, 5, 5, 5);
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
            lblAdminUsers.Margin = new Padding(5, 0, 5, 0);
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
            label10.Margin = new Padding(5, 0, 5, 0);
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
            panelPremiumUsers.Location = new Point(333, 61);
            panelPremiumUsers.Margin = new Padding(5, 5, 5, 5);
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
            lblPremiumUsers.Margin = new Padding(5, 0, 5, 0);
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
            label8.Margin = new Padding(5, 0, 5, 0);
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
            panelTotalUsers.Location = new Point(27, 61);
            panelTotalUsers.Margin = new Padding(5, 5, 5, 5);
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
            lblTotalUsers.Margin = new Padding(5, 0, 5, 0);
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
            label6.Margin = new Padding(5, 0, 5, 0);
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
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(187, 20);
            label1.TabIndex = 0;
            label1.Text = "Estadísticas Rápidas";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(panelRegistrations, 1, 0);
            tableLayoutPanel3.Controls.Add(panelRecentUsers, 0, 1);
            tableLayoutPanel3.Controls.Add(panelDistribution, 0, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 1, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 204);
            tableLayoutPanel3.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel3.Size = new Size(959, 651);
            tableLayoutPanel3.TabIndex = 21;
            // 
            // panelRegistrations
            // 
            panelRegistrations.BackColor = Color.White;
            panelRegistrations.Controls.Add(label5);
            panelRegistrations.Controls.Add(listRegistrations);
            panelRegistrations.Controls.Add(label4);
            panelRegistrations.Dock = DockStyle.Fill;
            panelRegistrations.Location = new Point(484, 5);
            panelRegistrations.Margin = new Padding(5, 5, 5, 5);
            panelRegistrations.Name = "panelRegistrations";
            panelRegistrations.Size = new Size(470, 315);
            panelRegistrations.TabIndex = 30;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = SystemColors.MenuText;
            label5.Location = new Point(165, 15);
            label5.Margin = new Padding(5, 0, 5, 0);
            label5.Name = "label5";
            label5.Size = new Size(160, 20);
            label5.TabIndex = 2;
            label5.Text = "Últimos Registros";
            // 
            // listRegistrations
            // 
            listRegistrations.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listRegistrations.FormattingEnabled = true;
            listRegistrations.ItemHeight = 18;
            listRegistrations.Location = new Point(13, 61);
            listRegistrations.Margin = new Padding(5, 5, 5, 5);
            listRegistrations.Name = "listRegistrations";
            listRegistrations.Size = new Size(453, 184);
            listRegistrations.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = SystemColors.MenuText;
            label4.Location = new Point(483, 0);
            label4.Margin = new Padding(5, 0, 5, 0);
            label4.Name = "label4";
            label4.Size = new Size(164, 20);
            label4.TabIndex = 0;
            label4.Text = "Registros por Mes";
            // 
            // panelRecentUsers
            // 
            panelRecentUsers.BackColor = Color.White;
            panelRecentUsers.Controls.Add(dataGridRecentUsers);
            panelRecentUsers.Controls.Add(label2);
            panelRecentUsers.Dock = DockStyle.Fill;
            panelRecentUsers.Location = new Point(5, 330);
            panelRecentUsers.Margin = new Padding(5, 5, 5, 5);
            panelRecentUsers.Name = "panelRecentUsers";
            panelRecentUsers.Size = new Size(469, 316);
            panelRecentUsers.TabIndex = 29;
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
            dataGridRecentUsers.Location = new Point(13, 61);
            dataGridRecentUsers.Margin = new Padding(5, 5, 5, 5);
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
            label2.Margin = new Padding(5, 0, 5, 0);
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
            panelDistribution.Dock = DockStyle.Fill;
            panelDistribution.Location = new Point(5, 5);
            panelDistribution.Margin = new Padding(5, 5, 5, 5);
            panelDistribution.Name = "panelDistribution";
            panelDistribution.Size = new Size(469, 315);
            panelDistribution.TabIndex = 26;
            // 
            // listRoles
            // 
            listRoles.BackColor = SystemColors.MenuBar;
            listRoles.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listRoles.FormattingEnabled = true;
            listRoles.ItemHeight = 18;
            listRoles.Location = new Point(13, 61);
            listRoles.Margin = new Padding(5, 5, 5, 5);
            listRoles.Name = "listRoles";
            listRoles.Size = new Size(453, 184);
            listRoles.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.MenuText;
            label3.Location = new Point(187, 15);
            label3.Margin = new Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new Size(151, 20);
            label3.TabIndex = 0;
            label3.Text = "Usuarios por Rol";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 229F));
            tableLayoutPanel4.Controls.Add(btnRefresh, 1, 1);
            tableLayoutPanel4.Controls.Add(btnGenerarPDF, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(482, 329);
            tableLayoutPanel4.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 3;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 93F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel4.Size = new Size(474, 318);
            tableLayoutPanel4.TabIndex = 31;
            // 
            // btnRefresh
            // 
            btnRefresh.Animated = true;
            btnRefresh.BorderColor = Color.White;
            btnRefresh.BorderRadius = 5;
            btnRefresh.BorderThickness = 2;
            btnRefresh.CustomizableEdges = customizableEdges3;
            btnRefresh.DisabledState.BorderColor = Color.DarkGray;
            btnRefresh.DisabledState.CustomBorderColor = Color.DarkGray;
            btnRefresh.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnRefresh.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnRefresh.Dock = DockStyle.Fill;
            btnRefresh.FillColor = Color.Transparent;
            btnRefresh.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(248, 202);
            btnRefresh.Margin = new Padding(3, 4, 3, 4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnRefresh.Size = new Size(223, 85);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "Actualizar";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnGenerarPDF
            // 
            btnGenerarPDF.BackColor = Color.Black;
            btnGenerarPDF.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGenerarPDF.ForeColor = SystemColors.ButtonFace;
            btnGenerarPDF.Location = new Point(3, 201);
            btnGenerarPDF.Name = "btnGenerarPDF";
            btnGenerarPDF.Size = new Size(239, 87);
            btnGenerarPDF.TabIndex = 1;
            btnGenerarPDF.Text = "Reportes";
            btnGenerarPDF.UseVisualStyleBackColor = false;
            btnGenerarPDF.Click += btnGenerarPDF_Click;
            // 
            // AdminDashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(2, 5, 20);
            ClientSize = new Size(1040, 923);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(5, 5, 5, 5);
            Name = "AdminDashboard";
            Text = "Panel de Control";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panelStats.ResumeLayout(false);
            panelStats.PerformLayout();
            panelAdminUsers.ResumeLayout(false);
            panelAdminUsers.PerformLayout();
            panelPremiumUsers.ResumeLayout(false);
            panelPremiumUsers.PerformLayout();
            panelTotalUsers.ResumeLayout(false);
            panelTotalUsers.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            panelRegistrations.ResumeLayout(false);
            panelRegistrations.PerformLayout();
            panelRecentUsers.ResumeLayout(false);
            panelRecentUsers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridRecentUsers).EndInit();
            panelDistribution.ResumeLayout(false);
            panelDistribution.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panelStats;
        private Panel panelAdminUsers;
        private Label lblAdminUsers;
        private Label label10;
        private Panel panelPremiumUsers;
        private Label lblPremiumUsers;
        private Label label8;
        private Panel panelTotalUsers;
        private Label lblTotalUsers;
        private Label label6;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel panelRegistrations;
        private ListBox listRegistrations;
        private Label label4;
        private Panel panelRecentUsers;
        private DataGridView dataGridRecentUsers;
        private Label label2;
        private Panel panelDistribution;
        private ListBox listRoles;
        private Label label3;
        private Label label5;
        private TableLayoutPanel tableLayoutPanel4;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private Button btnGenerarPDF;
    }
}