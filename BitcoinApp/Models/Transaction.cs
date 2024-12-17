using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitcoinApp.Models
{
    public class Transaction
    {
        [Key] public int IdTransaction { get; set; }
        public int IdUser { get; set; }
        public string TransactionType { get; set; }
        public int Units { get; set; }
        public DateTime BtcTimeStamp { get; set; }

        // Foreign keys
        [ForeignKey("IdUser")]
        public User User { get; set; }

        [ForeignKey("BtcTimeStamp")]
        public BTCPrice BTCPrice { get; set; }
    }
}
