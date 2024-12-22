namespace BitcoinApp.Models
{
    public class Token
    {
        
        public int IdToken { get; set; }
        public int IdUser { get; set; }
        public string TokenString { get; set; }
        public DateTime Expiration { get; set; }
    }
}
