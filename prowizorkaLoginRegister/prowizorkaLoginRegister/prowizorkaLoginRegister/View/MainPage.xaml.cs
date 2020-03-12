using prowizorkaLoginPage;
using prowizorkaLoginRegister.Helper;
using prowizorkaLoginRegister.Model;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace prowizorkaLoginRegister
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private FirebaseHelper firebaseHelper = new FirebaseHelper();
        public User ThisUser;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void LoginButtonClicked(object sender, EventArgs e)
        {
            ThisUser = await firebaseHelper.GetUser(loginB.Text);
            if (ThisUser == null)
            {
                await DisplayAlert("Failed", "No user found", "OK");
            }
            else
            {
                if (ThisUser.Password == passB.Text)
                {
                    //ThisUser = user;
                    
                    await DisplayAlert("Success", "Connected", "OK");
                    await Navigation.PushAsync(new Page1());
                }
                else
                    await DisplayAlert("Failed", "Invalid login or password", "OK");
            }
        }

        private async void RegisterButtonClicked(object sender, EventArgs e)
        {
            User user = await firebaseHelper.GetUser(loginB.Text);
            if (user == null)
            {
                await firebaseHelper.AddUser((loginB.Text), passB.Text);
                loginB.Text = string.Empty;
                passB.Text = string.Empty;
                await DisplayAlert("Success", "User Added Successfully", "OK");
            }
            else
                await DisplayAlert("Failed", "User already registered", "OK");
        }
    }
}