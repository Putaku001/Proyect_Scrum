using Microsoft.Maui.Controls;
using Proyecto_A.ViewModels;

namespace Proyecto_A.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly RegisterViewModel _viewModel;

    public RegisterPage()
    {
        InitializeComponent();
        _viewModel = new RegisterViewModel();
        BindingContext = _viewModel;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new LoginPage();
    }
}
