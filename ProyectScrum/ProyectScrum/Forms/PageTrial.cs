using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectScrum.Forms
{
    public partial class PageTrial : Form
    {
        public PageTrial()
        {
            InitializeComponent();
        }

        private void ISButton_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.FormClosed += LoginForm_FormClosed;
            loginForm.Show();
            this.Hide();
        }
        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
    
