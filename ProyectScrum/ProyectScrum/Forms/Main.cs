using ProyectScrum.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectScrum
{
    public partial class Main : Form
    {
        bool slideBarExpand;
        public Main()
        {
            InitializeComponent();
        }

        private void perfilButton_Click(object sender, EventArgs e)
        {

        }

        private void slideBarTime_Tick(object sender, EventArgs e)
        {
            if (slideBarExpand)
            {
                SlideBar.Width -= 10;
                if (SlideBar.Width == SlideBar.MinimumSize.Width)
                {
                    slideBarExpand = false;
                    slideBarTime.Stop();
                }
            }
            else
            {
                SlideBar.Width += 10;
                if (SlideBar.Width == SlideBar.MaximumSize.Width)
                {
                    slideBarExpand = true;
                    slideBarTime.Stop();
                }
            }
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            slideBarTime.Start();
        }

        private void catalogbtn_Click(object sender, EventArgs e)
        {
            Catalog catalogForm = new Catalog();
            catalogForm.Show();
            this.Hide();
        }
    }
}
