using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitcoinApp.Models
{
    public class User
    {
        [Key] public int IdUser { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public int IdRole { get; set; }

        // Foreign keys
        [ForeignKey("IdRole")]
        public Role Role { get; set; }
    }
}
