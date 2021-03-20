using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AccountManager.Model;

namespace AccountManager.Data
{
    public class AccountManagerContext : DbContext
    {
        public AccountManagerContext (DbContextOptions<AccountManagerContext> options)
            : base(options)
        {
        }

        public DbSet<AccountManager.Model.Account> Account { get; set; }
    }
}
