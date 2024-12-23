namespace BitcoinApp.Models
{
    public class Token
    {
        
        public int idToken { get; set; }
        public int idUser { get; set; }
        public string tokenString { get; set; }
        public DateTime expiration { get; set; }
        public bool revoked { get; set; }
    }
}
