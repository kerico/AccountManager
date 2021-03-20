using AccountManager.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return await _context.Account.ToListAsync();
        }
        public async Task<Account> GetAccount(int accountID)
        {
            return await _context.Account.FirstOrDefaultAsync(a => a.ID == accountID);
        }

        public async Task<Account> AddAccount(Account account)
        {
            var result = await _context.Account.AddAsync(account);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Account> UpdateAccount(Account account)
        {
            var result = await _context.Account.FirstOrDefaultAsync(a => a.ID == account.ID);
            if (result != null)
            {
                result.Password = account.Password;
                await _context.SaveChangesAsync();
            }

            return result;
        }

        public async Task DeleteAccount(int accountID)
        {
            var result = await _context.Account.FirstOrDefaultAsync(a => a.ID == accountID);
            if (result != null)
                return;

            _context.Account.Remove(result);
            await _context.SaveChangesAsync();
        }

    }
}
