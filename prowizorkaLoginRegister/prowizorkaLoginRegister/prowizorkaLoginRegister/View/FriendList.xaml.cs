using prowizorkaLoginRegister.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prowizorkaLoginPage;
using prowizorkaLoginRegister;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace prowizorkaLoginPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FriendList : ContentPage
    {
        public FriendList()
        {
            InitializeComponent();
            _friends = new ObservableCollection<Friend> { };
        }
                //new Friends { Login = "Rafau", Rank = "MasterMeme", Nick="Mistrz"},
                //new Friends { Login = "Kuba Rozruba", Rank = "MasterMeme", Nick="Ten drugi"},
                //new Friends { Login = "Asia Basia", Rank = "MiniMeme", Nick="Tego drugiego"},
                //new Friends { Login = "Marta Grzechu Warta", Rank = "MiniMeme", Nick="Mistrza"}
            
            
        

        private ObservableCollection<Friend> _friends;

        private void FriendSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var contact = e.SelectedItem as User;
            var visable = e.SelectedItem as Label;
        }
        async void BackButton(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void DeleteFriendButton(object sender, EventArgs e)
        {
            var selectedFriend = (sender as MenuItem).CommandParameter as Friend;
            _friends.Remove(selectedFriend);
        }
        async void GoToSearchFriendsPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchFriends());
        }

        async void GoToFriendRequests(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AcceptFriendRequest());
        }
    }
}