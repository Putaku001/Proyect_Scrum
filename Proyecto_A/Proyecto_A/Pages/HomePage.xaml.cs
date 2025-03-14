using Microsoft.Maui.Controls;
using Proyecto_A.ViewModels;

namespace Proyecto_A.Pages;

public partial class HomePage : ContentPage
{
    private bool isMenuOpen = true; // Estado del men� lateral

    public HomePage()
    {
        InitializeComponent();
        BindingContext = new HomeViewModel();
    }

    private void ToggleMenu(object sender, EventArgs e)
    {
        isMenuOpen = !isMenuOpen;

        if (!isMenuOpen)
        {
            SideMenu.IsVisible = false; // Oculta el men�
            MenuColumn.Width = new GridLength(0, GridUnitType.Absolute); // Cierra el men�
            MenuTab.IsVisible = true; // Muestra la pesta�a para reabrir
        }
        else
        {
            SideMenu.IsVisible = true; // Muestra el men�
            MenuColumn.Width = new GridLength(250, GridUnitType.Absolute); // Restaura el ancho del men�
            MenuTab.IsVisible = false; // Oculta la pesta�a porque ya est� abierto
        }
    }
}
