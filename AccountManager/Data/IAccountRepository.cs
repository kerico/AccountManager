using AccountManager.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountManager.Data
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccounts();
        Task<IEnumerable<Account>> GetAccountsFromDomain(string domainName);
        Task<Account> GetAccount(int accountID);
        Task<Account> GetAccount(Account account);
        Task<Account> AddAccount(Account account);
        Task<Account> UpdateAccount(int accountID, string newPassword);
        Task DeleteAccount(int accountID);
        Task<bool> AccountExists(int accountID);
    }
}
