namespace BitcoinApp.Models
{
    public class BTCPrice
    {
        public DateTime BtcTimeStamp { get; set; } // Primary key managed in the database
        public float Price { get; set; }           // The price of Bitcoin
    }
}
