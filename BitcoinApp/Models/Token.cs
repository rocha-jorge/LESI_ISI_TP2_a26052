using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitcoinApp.Models
{
    public class Token
    {
        
        [Key] public int IdToken { get; set; }
        public int IdUser { get; set; }
        public string TokenString { get; set; }
        public DateTime Expiration { get; set; }

        // Foreign keys
        [ForeignKey("IdUser")]
        public User User { get; set; }
    }
}
