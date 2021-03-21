using Microsoft.EntityFrameworkCore;

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
