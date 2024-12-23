namespace BitcoinApp.Models
{
    public class Transaction
    {
        public int idTransaction { get; set; }
        public int idUser { get; set; }
        public string transactionType { get; set; }
        public int units { get; set; }
        public DateTime btcTimeStamp { get; set; }
    }
}
