using prowizorkaLoginRegister.Model;
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
    public partial class SearchFriends : ContentPage
    {
        public SearchFriends()
        {
            InitializeComponent();

            SearchFriend_List.ItemsSource = GetUsers();
        }

        IEnumerable<User> GetUsers(string searchText = null)
        {
            var users = new List<User>{
      
            };

            if (String.IsNullOrWhiteSpace(searchText))
                return users;

            return users.Where(c => c.UserId.StartsWith(searchText));
        }
        async void BackButton(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private void AddFriendButton(object sender, System.EventArgs e)
        {
            var selectedUser = (sender as Button).CommandParameter as User;
            DisplayAlert("add", selectedUser.UserId + "dodany", "ok");
        }
        private void UserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var contact = e.SelectedItem as User;
        }

        private void SearchForFriend(object sender, TextChangedEventArgs e)
        {
            SearchFriend_List.ItemsSource = GetUsers(e.NewTextValue);
        }
    }
}