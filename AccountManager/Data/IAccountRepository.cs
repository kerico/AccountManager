using AccountManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManager.Data
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccounts();
        Task<Account> GetAccount(int accountID);
        Task<Account> AddAccount(Account account);
        Task<Account> UpdateAccount(Account account);
        Task DeleteAccount(int accointID);
    }
}
