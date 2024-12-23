namespace BitcoinApp.Models
{
    public class BTCPrice
    {
        public DateTime btcTimeStamp { get; set; } // Primary key managed in the database
        public float price { get; set; }           // The price of Bitcoin
    }
}
