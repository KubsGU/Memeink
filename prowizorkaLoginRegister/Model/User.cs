namespace prowizorkaLoginRegister.Model
{
    public class User
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserNickName { get; set; }
        public string Password { get; set; }
        public Ranks.UserGlobalRank UserGlobalRank { get; set; }
        public int Score { get; set; }
    }
}