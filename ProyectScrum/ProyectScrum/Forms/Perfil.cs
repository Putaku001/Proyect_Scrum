using ProyectScrum.Data;
using ProyectScrum.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectScrum.Forms
{
    public partial class Perfil : Form
    {
        private List<byte[]> avataresDisponibles;
        private int avatarSeleccionadoIndex = 0;
        private byte[] _avatarSeleccionadoBytes;
        EmailSettings _emailSettings;
        public Suscripcion suscripcionForm;

        public Perfil(EmailSettings emailSettings)
        {
            InitializeComponent();
            CargarAvatares();
            CargarDatosUsuario();
            AplicarEstilo();
            _emailSettings = emailSettings;

            RedondearControl(btnGuardar, 15);
            RedondearControl(btnAvatarAnterior, 15);
            RedondearControl(btnAvatarSiguiente, 15);
            RedondearControl(btnCambiarContraseña, 15);
            RedondearControl(btnEliminarCuenta, 15);
            RedondearControl(btnGestionarSuscripcion, 15);

            RedondearControl(pictureBoxAvatar, 30);
        }

        private void CargarAvatares()
        {
            avataresDisponibles = new List<byte[]>
            {
                Properties.Resources.Avatar4,
                Properties.Resources.Avatar3,
                Properties.Resources.Avatar2,
                Properties.Resources.Avatar1
            };

            CargarAvatarUsuario();
        }

        private void Perfil_Activated(object sender, EventArgs e)
        {
            CargarDatosUsuario();
        }

        private void CargarAvatarUsuario()
        {
            var dataAccess = new SqlDataAccess();
            using (var conn = dataAccess.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT Avatar FROM Usuarios WHERE UsuarioID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", CapturedData.UsuarioID);

                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    byte[] avatarBytes = (byte[])result;
                    _avatarSeleccionadoBytes = avatarBytes;

                    using (var ms = new MemoryStream(avatarBytes))
                    {
                        pictureBoxAvatar.Image = Image.FromStream(ms);
                        pictureBoxAvatar.AccessibleName = "AvatarUsuario";
                    }
                }
                else
                {
                    MostrarAvatar(0);
                }
            }
        }

        private void MostrarAvatar(int index)
        {
            if (index >= 0 && index < avataresDisponibles.Count)
            {
                avatarSeleccionadoIndex = index;
                _avatarSeleccionadoBytes = avataresDisponibles[index];

                using (var ms = new MemoryStream(avataresDisponibles[index]))
                {
                    pictureBoxAvatar.Image = Image.FromStream(ms);
                }
            }
        }

        public void CargarDatosUsuario()
        {
            txtNombre.Text = CapturedData.NombreUsuario;
            txtEmail.Text = CapturedData.Email;

            btnGestionarSuscripcion.Visible = (CapturedData.RolID != 1);
            btnCancelarSuscripcion.Visible = (CapturedData.EsPremium && CapturedData.RolID == 2);
            labelEsPremium.Visible = (CapturedData.RolID != 1);

            if (CapturedData.EsPremium && CapturedData.RolID == 2)
            {
                labelEsPremium.Text = "Es Premium";
                labelEsPremium.ForeColor = Color.Green;

                if (CapturedData.FechaFinSuscripcion.HasValue)
                {
                    lblFechaFinSuscripcion.Text = $"Vence el: {CapturedData.FechaFinSuscripcion.Value.ToShortDateString()}";
                    lblFechaFinSuscripcion.ForeColor = Color.Green;
                }
                else
                {
                    lblFechaFinSuscripcion.Text = "Fecha de vencimiento no disponible";
                    lblFechaFinSuscripcion.ForeColor = Color.Orange;
                }
            }
            else
            {
                labelEsPremium.Text = "No es Premium";
                labelEsPremium.ForeColor = Color.Red;
                lblFechaFinSuscripcion.Text = "";
            }
        }

        private void AplicarEstilo()
        {
            this.BackColor = Color.FromArgb(2, 5, 20);

        }

        private void btnAvatarAnterior_Click(object sender, EventArgs e)
        {
            int nuevoIndex = avatarSeleccionadoIndex - 1;
            if (nuevoIndex < 0) nuevoIndex = avataresDisponibles.Count - 1;
            MostrarAvatar(nuevoIndex);
        }

        private void btnAvatarSiguiente_Click(object sender, EventArgs e)
        {
            int nuevoIndex = (avatarSeleccionadoIndex + 1) % avataresDisponibles.Count;
            MostrarAvatar(nuevoIndex);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("El nombre de usuario es requerido");
                return;
            }

            if (ActualizarUsuario())
            {
                MessageBox.Show("Perfil actualizado correctamente");
            }
            else
            {
                MessageBox.Show("Error al actualizar el perfil");
            }
        }

        private bool ActualizarUsuario()
        {
            var dataAccess = new SqlDataAccess();
            using (var conn = dataAccess.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE Usuarios SET NombreUsuario = @Nombre, Email = @Email, Avatar = @Avatar WHERE UsuarioID = @ID",
                    conn);

                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.Add("@Avatar", SqlDbType.VarBinary).Value = _avatarSeleccionadoBytes ?? (object)DBNull.Value;
                cmd.Parameters.AddWithValue("@ID", CapturedData.UsuarioID);

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    CapturedData.NombreUsuario = txtNombre.Text;
                    CapturedData.Email = txtEmail.Text;

                    if (_avatarSeleccionadoBytes != null)
                    {
                        CapturedData.Avatar = Convert.ToBase64String(_avatarSeleccionadoBytes);
                    }

                    return true;
                }
                return false;
            }
        }

        private void btnCambiarContraseña_Click(object sender, EventArgs e)
        {
            New_Password cambiarContrasena = new New_Password(_emailSettings);
            cambiarContrasena.ShowDialog();
        }

        private void btnEliminarCuenta_Click(object sender, EventArgs e)
        {
            var confirmar = MessageBox.Show("¿Estás seguro de que quieres eliminar tu cuenta? Esta acción no se puede deshacer.",
                                          "Confirmar eliminación",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Warning);

            if (confirmar == DialogResult.Yes)
            {
                if (EliminarCuenta())
                {
                    MessageBox.Show("Cuenta eliminada correctamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
            }
        }

        public bool EliminarCuenta()
        {
            var dataAccess = new SqlDataAccess();
            using (var conn = dataAccess.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Usuarios WHERE UsuarioID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", CapturedData.UsuarioID);
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        private void btnGestionarSuscripcion_Click(object sender, EventArgs e)
        {
            if (CapturedData.EsPremium && CapturedData.FechaFinSuscripcion.HasValue && CapturedData.FechaFinSuscripcion.Value > DateTime.Now)
            {
                MessageBox.Show("Usted ya posee una suscripción activa hasta el " + CapturedData.FechaFinSuscripcion.Value.ToString("dd/MM/yyyy"), "Suscripción activa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var suscripcionForm = new Suscripcion(CapturedData.UsuarioID, this);
            suscripcionForm.ShowDialog();
        }


        private void RedondearControl(Control control, int radio)
        {
            Rectangle bounds = control.ClientRectangle;
            GraphicsPath path = new GraphicsPath();
            int diameter = radio * 2;

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90); // Top-left
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90); // Top-right
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90); // Bottom-right
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90); // Bottom-left
            path.CloseAllFigures();

            Region region = new Region(path);
            control.Region = region;
        }

        private void btnCancelarSuscripcion_Click(object sender, EventArgs e)
        {
            var confirmar = MessageBox.Show("¿Estás seguro que quieres cancelar tu suscripción? Perderás acceso Premium.",
                                    "Confirmar cancelación",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Warning);

            if (confirmar == DialogResult.Yes)
            {
                if (CancelarSuscripcion())
                {
                    MessageBox.Show("Suscripción cancelada correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatosUsuario(); // Refrescamos la vista
                }
                else
                {
                    MessageBox.Show("Error al cancelar la suscripción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool CancelarSuscripcion()
        {
            var dataAccess = new SqlDataAccess();
            using (var conn = dataAccess.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Usuarios SET EsPremium = 0, FechaFinSuscripcion = NULL WHERE UsuarioID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", CapturedData.UsuarioID);

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    // Actualizamos CapturedData también
                    CapturedData.EsPremium = false;
                    CapturedData.FechaFinSuscripcion = null;

                    return true;
                }
                return false;
            }
        }
    }
}