namespace BitcoinApp.Models
{
    public class User
    {
        public int idUser { get; set; }
        public string username { get; set; }
        public string passwordHash { get; set; }
        public string? email { get; set; }
        public int idRole { get; set; }
    }
}
