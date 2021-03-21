using AccountManager;
using AccountManager.Controllers;
using AccountManager.Data;
using AccountManager.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Controllers
{
    [TestFixture]
    public class TestAccountsController
    {
        private DbContextOptions<AccountManagerContext> _options = new DbContextOptionsBuilder<AccountManagerContext>()
                            .UseInMemoryDatabase(databaseName: "AcountManagerTestDB").Options;
        private AccountsController _controller;
        private IMapper _mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            SeedDB();
            SetupMapper();
            _controller = new AccountsController(new AccountRepository(new AccountManagerContext(_options)), _mapper);
        }

        [Test]
        public async Task GetAccounts_ShouldReturnOkObjectResult()
        {
            var result = (await _controller.GetAccounts(null)).Result;

            Assert.IsTrue(result is OkObjectResult);
        }
        [Test]
        public async Task GetAccounts_ShouldReturnAllAccountsIfDomainNameNotSpecified()
        {
            var accounts = ((await _controller.GetAccounts(null)).Result as OkObjectResult).Value as IEnumerable<object>;

            Assert.AreEqual(accounts.Count(), 4);
            Assert.IsFalse(accounts.Any(a => a == null));
        }
        [Test]
        public async Task GetAccounts_ShouldReturnAccountsWithSpecifiedDomain()
        {
            string domainName = "domain1";
            var accounts = ((await _controller.GetAccounts(domainName)).Result as OkObjectResult).Value as IEnumerable<AccountDTO>;

            Assert.AreEqual(accounts.Count(), 2);
            Assert.IsFalse(accounts.Any(a => a == null));
            Assert.IsFalse(accounts.Any(a => !a.DomainName.Equals(domainName.ToLower())));
        }
        [Test]
        public async Task CreateAccount_ShouldReturn400MessageWhenAddingAccountDublicate()
        {
            var accountDTO = new AccountDTO
            {
                DomainName = "domain3",
                AccountName = "Account1",
                Password = "Password1"
            };
            await _controller.CreateAccount(accountDTO);
            var response = (await _controller.CreateAccount(accountDTO)).Result;

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
        }
        [Test]
        public async Task CreateAccount_ShouldReturnReadableTextWhenAddingAccountDublicate()
        {
            var accountDTO = new AccountDTO
            {
                DomainName = "domain3",
                AccountName = "Account1",
                Password = "Password1"
            };
            await _controller.CreateAccount(accountDTO);
            var response = (await _controller.CreateAccount(accountDTO)).Result as BadRequestObjectResult;

            Assert.IsInstanceOf<string>(response.Value);
            Assert.IsNotEmpty((string)response.Value);
        }
        private void SetupMapper()
        {
            var mapperConfig = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()));
            _mapper = mapperConfig.CreateMapper();
        }
        private void SeedDB()
        {
            using AccountManagerContext context = new AccountManagerContext(_options);
            var accounts = new List<Account>
                {
                    new Account
                    {
                        ID = 1,
                        DomainName = "domain1",
                        AccountName = "Account1",
                        Password = "Password1"
                    },
                    new Account
                    {
                        ID = 2,
                        DomainName = "domain1",
                        AccountName = "Account2",
                        Password = "Password1"
                    },
                    new Account
                    {
                        ID = 3,
                        DomainName = "domain2",
                        AccountName = "Account1",
                        Password = "Password1"
                    },
                    new Account
                    {
                        ID = 4,
                        DomainName = "domain2",
                        AccountName = "Account3",
                        Password = "Password1"
                    },
                };
            context.Account.AddRange(accounts);
            context.SaveChanges();
        }
    }
}
