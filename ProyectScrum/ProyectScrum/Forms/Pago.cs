using ProyectScrum.Data;
using ProyectScrum.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectScrum.Forms
{
    public partial class Pago : Form
    {
        private int usuarioId;
        private string tipo;
        private int diasDuracion;
        public Pago(int idUsuario, string tipoSuscripcion, int duracionDias)
        {
            InitializeComponent();
            this.usuarioId = idUsuario;
            this.tipo = tipoSuscripcion;
            this.diasDuracion = duracionDias;
            lblResumen.Text = $"Suscripción: {tipo} - Duración: {diasDuracion} días";
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtNumero.Text) ||
                string.IsNullOrWhiteSpace(txtFechaExp.Text) ||
                string.IsNullOrWhiteSpace(txtCVV.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos.");
                return;
            }

            DateTime fechaFin = DateTime.Now.AddDays(diasDuracion);

            SqlDataAccess dataAccess = new SqlDataAccess();
            using (SqlConnection conn = dataAccess.GetConnection())
            {
                try
                {
                    conn.Open();

                    SqlCommand updateSuscripcion = new SqlCommand("UPDATE Usuarios SET EsPremium = 1, FechaFinSuscripcion = @fechaFin WHERE UsuarioID = @usuarioID", conn);
                    updateSuscripcion.Parameters.AddWithValue("@usuarioID", usuarioId);
                    updateSuscripcion.Parameters.AddWithValue("@fechaFin", fechaFin);
                    updateSuscripcion.ExecuteNonQuery();

                    // ACTUALIZA CapturedData
                    CapturedData.EsPremium = true;
                    CapturedData.FechaFinSuscripcion = fechaFin;

                    MessageBox.Show("¡Suscripción activada!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al activar la suscripción: {ex.Message}");
                }
            }
        }
    }
}
