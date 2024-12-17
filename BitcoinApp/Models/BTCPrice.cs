using System.ComponentModel.DataAnnotations;

namespace BitcoinApp.Models
{
    public class BTCPrice
    {
        [Key] public DateTime BtcTimeStamp { get; set; }
        public float Price { get; set; }

    }
}
