using System;
namespace prowizorkaLoginRegister.Model
{
    public class Friend
    {
        public string FriendUserName { get; set; }
        public string FriendNick { get; set; }
        public int RequestStatus { get; set; }//sent= 0 received =1 accepted =2
        public int DayStreak { get; set;}
        public string AcceptDate { get; set; }
        public string FriendCustomName { get; set; }
    }
}