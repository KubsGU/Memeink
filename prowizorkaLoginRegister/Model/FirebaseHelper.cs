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
        public async Task<List<Friend>> GetAllFriends(string userId,int requestStatus)
        {
            string myKey = await GetKey(userId);
            return (await firebase
              .Child("Users")
              .Child(myKey)
              .Child("FriendList")
              .OnceAsync<Friend>()).Select(item => new Friend
              {
                  FriendCustomName = item.Object.FriendCustomName,
                  FriendNick = item.Object.FriendNick,
                  FriendUserName = item.Object.FriendUserName,
                  RequestStatus = item.Object.RequestStatus,
              }).Where(a=>(a.RequestStatus==requestStatus)&&(a.FriendUserName!=null)&&(a.FriendNick!=null))
              .ToList();
        }
        public async Task<List<User>> GetAllUsers()
        {

        return (await firebase
              .Child("Users")
              .OnceAsync<User>()).Select(item => new User
              {
                  UserId=item.Object.UserId,
                  Password=item.Object.Password,
                  UserNickName=item.Object.UserNickName,
                  Email=item.Object.Email,
                  Score=item.Object.Score,
                  UserGlobalRank=item.Object.UserGlobalRank,
              }).ToList();
        }
        public async Task<List<User>> GetAllMatchingUsers(string friendName)
        {
            List<User> list = (await firebase
              .Child("Users")
              .OnceAsync<User>()).Select(item => new User
              {
                  UserId = item.Object.UserId,
                  UserNickName = item.Object.UserNickName,

              }).Where(c => (c.UserId.StartsWith(friendName)) || (c.UserNickName.StartsWith(friendName))).ToList();
           // if (list.Count != 0)
                return list;
           // else return null;
            
        }
        public async Task AddUser(string userId, string password,string nickname, string email)
        {
            await firebase
              .Child("Users")
              .PostAsync(new User() { UserId = userId, Password = password,UserNickName = nickname, Email=email, Score=0,UserGlobalRank=0 });//ToDo enum
        }
        public async Task SendFriendRequest(string friendUserName,string friendNick,string myUserId,string myUserNick)
        {
            //User
            //
            string myKey = await GetKey(myUserId);
            string friendKey = await GetKey(friendUserName);
            await firebase
              .Child("Users").Child(myKey).Child("FriendList")
              .PostAsync(new Friend() { FriendNick = friendNick, FriendUserName = friendUserName, FriendCustomName = null, RequestStatus = 0 });

           await firebase
            .Child("Friends").Child(myKey)
            .PostAsync(new DbFriend() { FriendId = friendUserName, IsFriend = false });
            //
            //Friend
            //
            await firebase
              .Child("Users").Child(friendKey).Child("FriendList")
              .PostAsync(new Friend() { FriendNick = myUserNick, FriendUserName = myUserId, FriendCustomName = null, RequestStatus = 1 });

            await firebase
             .Child("Friends").Child(friendKey)
             .PostAsync(new DbFriend() { FriendId = myUserId, IsFriend = false }); 
            //
        }
        public async Task AcceptFriendRequest(string friendUserName, string friendNick, string myUserId)
        {
            string myKey = await GetKey(myUserId);
            string friendKey = await GetKey(friendUserName);
            // uptading User.friendList
            string date = DateTime.Today.ToString();
            var toUpdateUserFriendList = (await firebase
              .Child("Users").Child(myKey).Child("FriendList")
              .OnceAsync<Friend>()).Where(a => a.Object.FriendUserName == friendUserName).FirstOrDefault().Key;
            //
            var toUpdateFriendFriendList = (await firebase
              .Child("Users").Child(friendKey).Child("FriendList")
              .OnceAsync<Friend>()).Where(a => a.Object.FriendUserName == myUserId).FirstOrDefault().Key;
            //
            await firebase
              .Child("Users").Child(myKey).Child("FriendList").Child(toUpdateUserFriendList)
              .PutAsync(new Friend() { RequestStatus = 2, AcceptDate = date, UserPrivateRank = 0, DayStreak = 0 });
            //
            await firebase
              .Child("Users").Child(friendKey).Child("FriendList").Child(toUpdateFriendFriendList)
              .PutAsync(new Friend() { RequestStatus = 2, AcceptDate = date, UserPrivateRank = 0, DayStreak = 0 });
            /////////////////////
            // uptading Friends
           // string friendsMyKey = await GetFriendsKey(myUserId);
           // string friendsFriendKey = await GetFriendsKey(friendUserName);
            var toUpdateFriendsUser = (await firebase
             .Child("Friends").Child(myKey)
             .OnceAsync<DbFriend>()).Where(a => a.Object.FriendId == friendUserName).FirstOrDefault().Key;
            //
            var toUpdateFriendsFriend = (await firebase
             .Child("Friends").Child(friendKey)
             .OnceAsync<DbFriend>()).Where(a => a.Object.FriendId == myUserId).FirstOrDefault().Key;
            //
            await firebase
             .Child("Friends").Child(myKey).Child(friendKey)
             .PutAsync(new DbFriend() {IsFriend = true });
            //
            await firebase
                .Child("Friends").Child(friendKey).Child(myKey)
                .PutAsync(new DbFriend() { IsFriend = true });
        }
        public async Task DeclineFriendRequest(string friendUserName, string friendNick, string myUserId)
        {
            string myKey = await GetKey(myUserId);
            string friendKey = await GetKey(friendUserName);
            // uptading User.friendList
            string date = DateTime.Today.ToString();
            var toUpdateUserFriendList = (await firebase
              .Child("Users").Child(myKey).Child("FriendList")
              .OnceAsync<Friend>()).Where(a => a.Object.FriendUserName == friendUserName).FirstOrDefault().Key;
            //
            var toUpdateFriendFriendList = (await firebase
              .Child("Users").Child(friendKey).Child("FriendList")
              .OnceAsync<Friend>()).Where(a => a.Object.FriendUserName == myUserId).FirstOrDefault().Key;
            //
            await firebase
              .Child("Users").Child(myKey).Child("FriendList").Child(toUpdateUserFriendList)
              .DeleteAsync();

            //
            await firebase
              .Child("Users").Child(friendKey).Child("FriendList").Child(toUpdateFriendFriendList)
              .DeleteAsync();
            /////////////////////
            // uptading Friends
            string friendsMyKey = await GetFriendsKey(myUserId);
            string friendsFriendKey = await GetFriendsKey(friendUserName);
            var toUpdateFriendsUser = (await firebase
             .Child("Friends").Child(friendsMyKey)
             .OnceAsync<DbFriend>()).Where(a => a.Object.FriendId == friendUserName).FirstOrDefault().Key;
            //
            var toUpdateFriendsFriend = (await firebase
             .Child("Friends").Child(friendsFriendKey)
             .OnceAsync<DbFriend>()).Where(a => a.Object.FriendId == myUserId).FirstOrDefault().Key;
            //
            await firebase
             .Child("Friends").Child(friendsMyKey).Child(toUpdateFriendsUser)
             .DeleteAsync();
            //
            await firebase
                .Child("Friends").Child(friendsFriendKey).Child(toUpdateFriendsFriend)
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
        public async Task <string> GetKey(string userId)
        {
            return (await firebase
             .Child("Users")
             .OnceAsync<User>()).Where(a => a.Object.UserId == userId).FirstOrDefault().Key;
        }
        public async Task<string> GetFriendsKey(string userId)
        {
            return (await firebase
             .Child("Friends")
             .OnceAsync<User>()).Where(a => a.Object.UserId == userId).FirstOrDefault().Key;
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