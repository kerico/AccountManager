using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountManager.Data;
using AccountManager.Model;
using AutoMapper;

namespace AccountManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper _mapper;
        public AccountsController(IAccountRepository repository, IMapper mapper)
        {
            //TODO: unit tests
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAccounts(string domainName)
        {
            IEnumerable<Account> result;
            if (string.IsNullOrWhiteSpace(domainName))
                result = await _repository.GetAccounts();
            else
                result = await _repository.GetAccountsFromDomain(domainName);

            if (!result.Any())
                return NotFound("No accounts have been found");
            return Ok(result.Select(a => _mapper.Map<AccountDTO>(a)));
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> GetAccount(int id)
        {
            var account = await _repository.GetAccount(id);

            if (account == null)
                return NotFound($"Account(id:{id}) was not found");

            return _mapper.Map<AccountDTO>(account);
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, string newPassword)
        {
            //NOTE: if allowed updating not just password, needs validation for unique account
            if (string.IsNullOrEmpty(newPassword))
                return BadRequest("Account password cannot be empty.");

            try
            {
                await _repository.UpdateAccount(id, newPassword.Encrypt());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                    return NotFound($"Account(id:{id}) was not found");
                else
                    throw;
            }

            return Ok("Password has been changed");
        }

        // POST: api/Accounts
        [HttpPost]
        public async Task<ActionResult<Account>> CreateAccount(AccountDTO accountDTO)
        {
            if (string.IsNullOrEmpty(accountDTO.Password))
                return BadRequest("Account password cannot be empty.");

            var account = _mapper.Map<Account>(accountDTO);

            var existingAccount = await _repository.GetAccount(account);

            if (existingAccount != null && account.AccountName.Equals(existingAccount.AccountName))
                return BadRequest($"Account {account.DomainName}\\{account.AccountName} already exists");

            account = await _repository.AddAccount(account);
            return CreatedAtAction("GetAccount", new { id = account.ID }, _mapper.Map<AccountDetailedDTO>(account));
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            if (!AccountExists(id))
                return NotFound($"Account(id:{id}) was not found");

            await _repository.DeleteAccount(id);

            return Ok($"Account(id:{id}) has been deleted");
        }

        private bool AccountExists(int id)
        {
            var accountExistsTask = _repository.AccountExists(id);
            return accountExistsTask.Result;
        }
    }
}
