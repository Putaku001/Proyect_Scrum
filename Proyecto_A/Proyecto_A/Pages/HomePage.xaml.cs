using Microsoft.Maui.Controls;
using Proyecto_A.ViewModels;

namespace Proyecto_A.Pages;

public partial class HomePage : ContentPage
{
    private bool isMenuOpen = true; // Estado del menú lateral

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
            SideMenu.IsVisible = false; // Oculta el menú
            MenuColumn.Width = new GridLength(0, GridUnitType.Absolute); // Cierra el menú
            MenuTab.IsVisible = true; // Muestra la pestaña para reabrir
        }
        else
        {
            SideMenu.IsVisible = true; // Muestra el menú
            MenuColumn.Width = new GridLength(250, GridUnitType.Absolute); // Restaura el ancho del menú
            MenuTab.IsVisible = false; // Oculta la pestaña porque ya está abierto
        }
    }
}
