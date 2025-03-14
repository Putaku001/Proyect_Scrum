using System;
using Proyecto_A.ViewModels;

namespace Proyecto_A.Pages;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public LoginPage()
    {
        InitializeComponent();
        _viewModel = new LoginViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await WelcomeLabel.FadeTo(1, 1000, Easing.CubicIn);
    }
    private void OnRegisterClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new RegisterPage();
    }
    private void OnSkipClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new HomePage(); // Ir a la página principal sin iniciar sesión
    }
}
