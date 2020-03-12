using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace prowizorkaLoginPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private async void dodajZnajomego(object sender, EventArgs e)
        {
            var znajomy = dodaj.Text;
        }

        private async void returnButton(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}