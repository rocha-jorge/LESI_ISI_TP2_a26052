namespace BitcoinApp.Models
{
    public class Transaction
    {
        public int IdTransaction { get; set; }
        public int IdUser { get; set; }
        public string TransactionType { get; set; }
        public int Units { get; set; }
        public DateTime BtcTimeStamp { get; set; }
    }
}
