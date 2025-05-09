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
    public partial class Suscripcion : Form
    {
        private int usuarioId;
        private Perfil perfilForm;
        public Suscripcion(int idUsuario, Perfil perfil)
        {
            InitializeComponent();
            this.usuarioId = idUsuario;
            this.perfilForm = perfil;
        }

        private void btnMensual_Click(object sender, EventArgs e)
        {
            var pagoForm = new Pago(usuarioId, "Mensual", 30);
            if (pagoForm.ShowDialog() == DialogResult.OK)
            {
                perfilForm.CargarDatosUsuario(); // ACTUALIZA la vista
            }
            this.Hide();
        }

        private void btnAnual_Click(object sender, EventArgs e)
        {
            var pagoForm = new Pago(usuarioId, "Anual", 306);
            if (pagoForm.ShowDialog() == DialogResult.OK)
            {
                perfilForm.CargarDatosUsuario();
            }
            this.Hide();
        }
    }
}
