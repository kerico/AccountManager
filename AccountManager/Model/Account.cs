using System.ComponentModel.DataAnnotations;

namespace AccountManager.Model
{
    public class Account
    {
        public int ID { get; set; }
        [Required]
        public string DomainName { get; set; }
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
