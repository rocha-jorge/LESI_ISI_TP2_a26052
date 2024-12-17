using System.ComponentModel.DataAnnotations;

namespace BitcoinApp.Models
{
    public class Role
    {
        [Key] public int IdRole { get; set; }
        public string RoleDescription { get; set; }
    }
}
