namespace WebsupplyHHemo.API.Models
{
    public class UserModel
    {
        public string LOGIN { get; set; }
        public string CGC { get; set; }
        public string CCUSTO { get; set; }
        public string REQUISIT { get; set; }
        public string NOME { get; set; }
        public string EMAIL { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public int NUMTENTATIVAS { get; set; }
    }
}
