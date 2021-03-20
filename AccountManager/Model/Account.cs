using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
