using System;
using System.Windows.Input;
using Proyecto_A.Data;
using Proyecto_A.Models;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Proyecto_A.ViewModels;

public class RegisterViewModel
{
    private readonly DatabaseService _databaseService = new();

    public string NombreUsuario { get; set; }
    public string Email { get; set; }
    public string Contrasena { get; set; }

    public ICommand RegisterCommand => new Command(async () => await RegistrarUsuario());

    private async Task RegistrarUsuario()
    {
        if (string.IsNullOrWhiteSpace(NombreUsuario) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Contrasena))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
            return;
        }

        if (await _databaseService.VerificarEmailExistente(Email))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Este correo ya está registrado", "OK");
            return;
        }

        bool resultado = await _databaseService.InsertarUsuarioAsync(NombreUsuario, Email, Contrasena);

        if (resultado)
        {
            await Application.Current.MainPage.DisplayAlert("Éxito", "Usuario registrado correctamente", "OK");
            Application.Current.MainPage = new LoginPage();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No se pudo registrar el usuario", "OK");
        }
    }
}
