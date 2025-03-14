using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using Proyecto_A.Data;
using Proyecto_A.Models;

namespace Proyecto_A.ViewModels;

public class LoginViewModel
{
    private readonly DatabaseService _databaseService = new();

    public string Email { get; set; }
    public string Contrasena { get; set; }

    public ICommand LoginCommand => new Command(async () => await IniciarSesion());

    private async Task IniciarSesion()
    {
        var usuario = await _databaseService.ObtenerUsuarioPorEmailAsync(Email);

        if (usuario != null && VerificarContraseña(Contrasena, usuario.ContrasenaHash))
        {
            Application.Current.MainPage = new HomePage(); // Si la contraseña es correcta, redirige a Home
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Correo o contraseña incorrectos", "OK");
        }
    }

    // Método para verificar si la contraseña ingresada coincide con la almacenada (hasheada)
    private bool VerificarContraseña(string contraseñaIngresada, string contraseñaAlmacenada)
    {
        string hashIngresado = HashPassword(contraseñaIngresada);
        return hashIngresado == contraseñaAlmacenada;
    }

    // Método para hashear la contraseña ingresada por el usuario
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
}
