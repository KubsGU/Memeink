using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace prowizorkaLoginPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcceptFriendRequest : ContentPage
    {
        public AcceptFriendRequest()
        {
            InitializeComponent();

            var users = new List<Users>{
                new Users { Login = "Rafau", Rank = "MasterMeme", Nick="Mistrz"},
                new Users { Login = "Kuba Rozruba", Rank = "MasterMeme", Nick="Mistrz"}
            };

            SearchFriend_List.ItemsSource = users;
        }

        async void BackButton(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private void AcceptRequest(object sender, System.EventArgs e)
        {
            var selectedUser = (sender as Button).CommandParameter as Users;
            DisplayAlert("add", selectedUser.Login + "dodany", "ok");
        }

        private void UserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var contact = e.SelectedItem as Users;
        }
    }
}