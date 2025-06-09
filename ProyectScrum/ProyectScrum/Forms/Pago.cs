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
using System.Text.RegularExpressions;
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

            // Configurar máscaras y validaciones iniciales
            txtNumero.KeyPress += TxtNumero_KeyPress;
            txtNumero.Leave += TxtNumero_Leave;
            txtCVV.KeyPress += TxtCVV_KeyPress;
            txtFechaExp.KeyPress += TxtFechaExp_KeyPress;
            txtFechaExp.Leave += TxtFechaExp_Leave;
            txtNombre.KeyPress += TxtNombre_KeyPress;
        }

        #region Eventos de Validación

        private void TxtNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permitir números y teclas de control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtNumero_Leave(object sender, EventArgs e)
        {
            // Formatear número de tarjeta como XXXX XXXX XXXX XXXX
            string cleaned = Regex.Replace(txtNumero.Text, @"[^\d]", "");
            if (cleaned.Length >= 16)
            {
                txtNumero.Text = Regex.Replace(cleaned, @"(\d{4})(\d{4})(\d{4})(\d{4})", "$1 $2 $3 $4");
            }
            else if (cleaned.Length > 0)
            {
                MessageBox.Show("El número de tarjeta debe tener 16 dígitos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNumero.Focus();
            }
        }

        private void TxtCVV_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permitir números y teclas de control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtFechaExp_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permitir números, '/' y teclas de control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '/')
            {
                e.Handled = true;
            }
        }

        private void TxtFechaExp_Leave(object sender, EventArgs e)
        {
            // Primero validar que tenga el formato correcto con /
            if (!txtFechaExp.Text.Contains('/'))
            {
                MessageBox.Show("Formato de fecha inválido. Use MM/AA", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaExp.Focus();
                return;
            }

            // Dividir la fecha en partes
            var parts = txtFechaExp.Text.Split('/');

            // Validar que tengamos exactamente 2 partes (mes y año)
            if (parts.Length != 2)
            {
                MessageBox.Show("Formato de fecha inválido. Use MM/AA", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaExp.Focus();
                return;
            }

            // Validar que ambas partes sean numéricas
            if (!int.TryParse(parts[0], out int month) || !int.TryParse(parts[1], out int year))
            {
                MessageBox.Show("Solo se permiten números en MM/AA", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaExp.Focus();
                return;
            }

            // Validar mes (1-12)
            if (month < 1 || month > 12)
            {
                MessageBox.Show("Mes inválido. Debe ser entre 01 y 12", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaExp.Focus();
                return;
            }

            // Validar año (2 dígitos)
            if (parts[1].Length != 2)
            {
                MessageBox.Show("Año inválido. Use 2 dígitos (ej: 25 para 2025)", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaExp.Focus();
                return;
            }

            // Convertir año a 4 dígitos (asumimos siglo 21)
            year += 2000;

            // Validar que la fecha no esté expirada
            var expirationDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            if (expirationDate < DateTime.Now)
            {
                MessageBox.Show("La tarjeta ha expirado", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFechaExp.Focus();
            }
        }

        private void TxtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permitir letras, espacios y teclas de control
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            // Validación básica de campos vacíos
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtNumero.Text) ||
                string.IsNullOrWhiteSpace(txtFechaExp.Text) ||
                string.IsNullOrWhiteSpace(txtCVV.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validaciones específicas
            if (txtNombre.Text.Trim().Length < 3)
            {
                MessageBox.Show("El nombre del titular debe tener al menos 3 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombre.Focus();
                return;
            }

            string cleanedCardNumber = Regex.Replace(txtNumero.Text, @"[^\d]", "");
            if (cleanedCardNumber.Length != 16)
            {
                MessageBox.Show("El número de tarjeta debe tener exactamente 16 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNumero.Focus();
                return;
            }

            if (txtCVV.Text.Length < 3 || txtCVV.Text.Length > 4 || !int.TryParse(txtCVV.Text, out _))
            {
                MessageBox.Show("CVV inválido. Debe ser un número de 3 o 4 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCVV.Focus();
                return;
            }

            // Procesar el pago si todas las validaciones pasan
            DateTime fechaFin = DateTime.Now.AddDays(diasDuracion);

            SqlDataAccess dataAccess = new SqlDataAccess();
            using (SqlConnection conn = dataAccess.GetConnection())
            {
                try
                {
                    conn.Open();

                    SqlCommand updateSuscripcion = new SqlCommand(
                        "UPDATE Usuarios SET EsPremium = 1, FechaFinSuscripcion = @fechaFin WHERE UsuarioID = @usuarioID",
                        conn);
                    updateSuscripcion.Parameters.AddWithValue("@usuarioID", usuarioId);
                    updateSuscripcion.Parameters.AddWithValue("@fechaFin", fechaFin);
                    updateSuscripcion.ExecuteNonQuery();

                    // Actualizar CapturedData
                    CapturedData.EsPremium = true;
                    CapturedData.FechaFinSuscripcion = fechaFin;

                    MessageBox.Show("¡Suscripción activada con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al activar la suscripción: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
