namespace BitcoinApp.Models
{
    public class User
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string? Email { get; set; }
        public int IdRole { get; set; }
}
