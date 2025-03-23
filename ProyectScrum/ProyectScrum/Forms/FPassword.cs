using ProyectScrum.Entities;
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
    public partial class FPassword : Form
    {
        private readonly EmailSettings _emailSettings;
        public FPassword(EmailSettings email)
        {
            InitializeComponent();
            _emailSettings = email;
        }

        private void customControl1_Click(object sender, EventArgs e)
        {
            if (DateTime.Now - _emailSettings.time > TimeSpan.FromMinutes(10))
            {
                MessageBox.Show("El codigo ha expirado, ha excedido el limite de 10 minutos establecido, mande una nueva solicitud");
            }
            else if (textBox1.Text == _emailSettings.codigoVerificacion.ToString())
            {
                New_Password new_Password = new New_Password(_emailSettings);
                new_Password.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Código incorrecto. Inténtalo de nuevo.");
            }
        }
    }
}

