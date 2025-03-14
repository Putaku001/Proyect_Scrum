using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Proyecto_A.Models;
using System.Security.Cryptography;
using System.Text;


namespace Proyecto_A.Data
{
    public class DatabaseService
    {
        private readonly string connectionString = "Data Source=PUTAKU\\SQLEXPRESS;Initial Catalog=proyectoDBS;Integrated Security=True;Trust Server Certificate=True";

        // Probar conexión a la base de datos
        public async Task<bool> TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    Console.WriteLine("✅ Conexión exitosa a la base de datos.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al conectar con la base de datos: {ex.Message}");
                return false;
            }
        }

        // Obtener todos los usuarios de la BD
        public async Task<List<UsuarioModel>> ObtenerUsuariosAsync()
        {
            var usuarios = new List<UsuarioModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Usuarios", connection))
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        usuarios.Add(new UsuarioModel
                        {
                            UsuarioID = reader.GetInt32(0),
                            NombreUsuario = reader.GetString(1),
                            Email = reader.GetString(2),
                            ContrasenaHash = reader.GetString(3),
                            FechaRegistro = reader.GetDateTime(4),
                            RolID = reader.GetInt32(5)
                        });
                    }
                }
            }
            return usuarios;
        }

        // Obtener un usuario por Email
        public async Task<UsuarioModel?> ObtenerUsuarioPorEmailAsync(string email)
        {
            UsuarioModel? usuario = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Usuarios WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usuario = new UsuarioModel
                            {
                                UsuarioID = reader.GetInt32(0),
                                NombreUsuario = reader.GetString(1),
                                Email = reader.GetString(2),
                                ContrasenaHash = reader.GetString(3),
                                FechaRegistro = reader.GetDateTime(4),
                                RolID = reader.GetInt32(5)
                            };
                        }
                    }
                }
            }
            return usuario;
        }

        // Insertar un nuevo usuario
        public async Task<bool> InsertarUsuarioAsync(string nombreUsuario, string email, string contrasena)
        {
            string contrasenaHash = HashPassword(contrasena);
            int rolUsuario = 1; // Siempre será "Usuario"

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Usuarios (NombreUsuario, Email, ContrasenaHash, RolID) VALUES (@NombreUsuario, @Email, @ContrasenaHash, @RolID)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@ContrasenaHash", contrasenaHash);
                    command.Parameters.AddWithValue("@RolID", rolUsuario);

                    int result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }

        public async Task<bool> VerificarEmailExistente(string email)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Email = @Email";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    int count = (int)await command.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //obtener contenidos
        public async Task<List<ContenidoModel>> ObtenerContenidosAsync()
        {
            var contenidos = new List<ContenidoModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Contenidos", connection))
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        contenidos.Add(new ContenidoModel
                        {
                            ContenidoID = reader.GetInt32(0),
                            Titulo = reader.GetString(1),
                            Descripcion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            CategoriaID = reader.GetInt32(3),
                            FechaPublicacion = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4)
                        });
                    }
                }
            }
            return contenidos;
        }

    }
}
