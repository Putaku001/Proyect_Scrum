using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Proyecto_A.Pages
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
            AnimateDots();
            NavigateToMainPage();
        }

        private async void AnimateDots()
        {
            while (true)
            {
                await Dot1.FadeTo(1, 500);
                await Dot2.FadeTo(1, 500);
                await Dot3.FadeTo(1, 500);
                await Task.Delay(500);

                await Dot1.FadeTo(0.2, 500);
                await Dot2.FadeTo(0.5, 500);
                await Dot3.FadeTo(1, 500);
                await Task.Delay(500);
            }
        }

        private async void NavigateToMainPage()
        {
            await Task.Delay(4000);
            Application.Current.MainPage = new LoginPage();
        }
    }
}
