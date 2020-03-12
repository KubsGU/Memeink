using Firebase.Database;
using Firebase.Database.Query;
using prowizorkaLoginRegister.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace prowizorkaLoginRegister.Helper
{
    public class FirebaseHelper
    {
        private FirebaseClient firebase = new FirebaseClient("https://memechat-5c36a.firebaseio.com/Tb_Users/apgIN02i958fYerf8Uat");

        public async Task<List<User>> GetAllUsers()
        {
            return (await firebase
              .Child("Users")
              .OnceAsync<User>()).Select(item => new User
              {
                  Password = item.Object.Password,
                  UserId = item.Object.UserId
              }).ToList();
        }
        public async Task<List<User>> GetAllMatchingUsers(string friendName)
        {
            return (await firebase
              .Child("Users")
              .OnceAsync<User>()).Select(item => new User
              {
                  UserId = item.Object.UserId,
                  UserNickName = item.Object.UserNickName,

              }).Where(a => (a.UserId==friendName )||( a.UserNickName==friendName  )).ToList();
        }

        public async Task AddUser(string userId, string password)
        {
            await firebase
              .Child("Users")
              .PostAsync(new User() { UserId = userId, Password = password });
        }
        public async Task SendFriendRequest(string friendUserName,string friendNick,string myUserId,string myUserNick)
        {
            //User
            //
            await firebase
              .Child("Users").Child(myUserId).Child("FriendList")
              .PostAsync(new Friend() { FriendNick = friendNick, FriendUserName = friendUserName, FriendCustomName = null, RequestStatus = 0 });

           await firebase
            .Child("Friends").Child(myUserId)
            .PostAsync(new DbFriend() { FriendId = friendUserName, IsFriend = false });
            //
            //Friend
            //
            await firebase
              .Child("Users").Child(friendUserName).Child("FriendList")
              .PostAsync(new Friend() { FriendNick = myUserNick, FriendUserName = myUserId, FriendCustomName = null, RequestStatus = 1 });

            await firebase
             .Child("Friends").Child(friendUserName)
             .PostAsync(new DbFriend() { FriendId = myUserId, IsFriend = false }); 
            //
        }
        public async Task AcceptFriendRequest(string friendUserName, string friendNick, string myUserId)
        {
            // uptading User.friendList
            string date = DateTime.Today.ToString();
            var toUpdateUserFriendList = (await firebase
              .Child("Users").Child(myUserId).Child("FriendList")
              .OnceAsync<Friend>()).Where(a => a.Object.FriendUserName == friendUserName).FirstOrDefault().Key;
            //
            var toUpdateFriendFriendList = (await firebase
              .Child("Users").Child(friendUserName).Child("FriendList")
              .OnceAsync<Friend>()).Where(a => a.Object.FriendUserName == myUserId).FirstOrDefault().Key;
            //
            await firebase
              .Child("Users").Child(myUserId).Child("FriendList").Child(toUpdateUserFriendList)
              .PutAsync(new Friend() { RequestStatus = 2, AcceptDate=date });
            //
            await firebase
              .Child("Users").Child(friendUserName).Child("FriendList").Child(toUpdateFriendFriendList)
              .PutAsync(new Friend() { RequestStatus = 2, AcceptDate=date });
            /////////////////////
            // uptading Friends
            var toUpdateFriendsUser = (await firebase
             .Child("Friends").Child(myUserId)
             .OnceAsync<DbFriend>()).Where(a => a.Object.FriendId == friendUserName).FirstOrDefault().Key;
            //
            var toUpdateFriendsFriend = (await firebase
             .Child("Friends").Child(friendUserName)
             .OnceAsync<DbFriend>()).Where(a => a.Object.FriendId == myUserId).FirstOrDefault().Key;
            //
            await firebase
             .Child("Friends").Child(myUserId).Child(toUpdateFriendsUser)
             .PutAsync(new DbFriend() {IsFriend = true });
            //
            await firebase
                .Child("Friends").Child(friendUserName).Child(toUpdateFriendsFriend)
                .PutAsync(new DbFriend() { IsFriend = true });
        }
        public async Task DeclineFriendRequest(string friendUserName, string friendNick, string myUserId)
        {
            // uptading User.friendList
            string date = DateTime.Today.ToString();
            var toUpdateUserFriendList = (await firebase
              .Child("Users").Child(myUserId).Child("FriendList")
              .OnceAsync<Friend>()).Where(a => a.Object.FriendUserName == friendUserName).FirstOrDefault().Key;
            //
            var toUpdateFriendFriendList = (await firebase
              .Child("Users").Child(friendUserName).Child("FriendList")
              .OnceAsync<Friend>()).Where(a => a.Object.FriendUserName == myUserId).FirstOrDefault().Key;
            //
            await firebase
              .Child("Users").Child(myUserId).Child("FriendList").Child(toUpdateUserFriendList)
              .DeleteAsync();

            //
            await firebase
              .Child("Users").Child(friendUserName).Child("FriendList").Child(toUpdateFriendFriendList)
              .DeleteAsync();
            /////////////////////
            // uptading Friends
            var toUpdateFriendsUser = (await firebase
             .Child("Friends").Child(myUserId)
             .OnceAsync<DbFriend>()).Where(a => a.Object.FriendId == friendUserName).FirstOrDefault().Key;
            //
            var toUpdateFriendsFriend = (await firebase
             .Child("Friends").Child(friendUserName)
             .OnceAsync<DbFriend>()).Where(a => a.Object.FriendId == myUserId).FirstOrDefault().Key;
            //
            await firebase
             .Child("Friends").Child(myUserId).Child(toUpdateFriendsUser)
             .DeleteAsync();
            //
            await firebase
                .Child("Friends").Child(friendUserName).Child(toUpdateFriendsFriend)
                .DeleteAsync();
        }


        public async Task<User> GetUser(string userId)
        {
            var allUsers = await GetAllUsers();
            await firebase
              .Child("Users")
              .OnceAsync<User>();
            return allUsers.Where(a => a.UserId == userId).FirstOrDefault();
        }

        public async Task UpdateUser(string userId, string password)
        {
            var toUpdateUser = (await firebase
              .Child("Users")
              .OnceAsync<User>()).Where(a => a.Object.UserId == userId).FirstOrDefault();

            await firebase
              .Child("Users")
              .Child(toUpdateUser.Key)
              .PutAsync(new User() { UserId = userId, Password = password });
        }
        public async Task UptadeFriend(string userId, string friendId)
        {
             var toUpdateUserFriendList = (await firebase
              .Child("Users").Child(userId).Child("Friends")
              .OnceAsync<Friend>()).Where(a => a.Object.FriendUserName == friendId).FirstOrDefault();
            //return toUpdateUserFriendList;
            //await firebase
            //  .Child("Users").Child(userId).Child("Friends")
            //  .Child(toUpdateUserFriendList.Key)
            //  .PutAsync(new Friend() { AcceptDate = DateTime.Now, RequestStatus=2  });
        }

        public async Task DeleteUser(string userId)
        {
            var toDeleteUser = (await firebase
              .Child("Users")
              .OnceAsync<User>()).Where(a => a.Object.UserId == userId).FirstOrDefault();
            await firebase.Child("Users").Child(toDeleteUser.Key).DeleteAsync();
        }
    }
}