using AccountManager.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AccountManager.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountManagerContext _context;
        public AccountRepository(AccountManagerContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await _context.Account.OrderBy(a => a.DomainName).ThenBy(a => a.AccountName).ToListAsync();
        }
        public async Task<IEnumerable<Account>> GetAccountsFromDomain(string domainName)
        {
            return await _context.Account.Where(a=> a.DomainName.ToLower().Equals(domainName.ToLower())).OrderBy(a => a.DomainName).ThenBy(a => a.AccountName).ToListAsync();
        }
        public async Task<Account> GetAccount(int accountID)
        {
            return await _context.Account.FirstOrDefaultAsync(a => a.ID == accountID);
        }
        public async Task<Account> GetAccount(Account account)
        {
            var result = await _context.Account.FirstOrDefaultAsync(a =>
            a.DomainName.ToLower().Equals(account.DomainName.ToLower())
            && a.AccountName.Equals(account.AccountName));

            return result;
        }
        public async Task<Account> AddAccount(Account account)
        {
            account.DomainName = account.DomainName.ToLower();
            var result = await _context.Account.AddAsync(account);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
        public async Task<Account> UpdateAccount(int accountID, string newPassword)
        {
            var result = await _context.Account.FirstOrDefaultAsync(a => a.ID == accountID);
            if (result != null)
            {
                result.Password = newPassword;
                await _context.SaveChangesAsync();
            }

            return result;
        }
        public async Task DeleteAccount(int accountID)
        {
            var result = await _context.Account.FirstOrDefaultAsync(a => a.ID == accountID);
            if (result == null)
                return;

            _context.Account.Remove(result);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> AccountExists(int accountID)
        {
            var result = await _context.Account.AnyAsync(a => a.ID == accountID);

            return result;
        }
    }
}
