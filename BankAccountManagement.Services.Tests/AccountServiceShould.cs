using AutoFixture;
using BankAccountManagement.Entities;
using BankAccountManagement.Repositories.Interfaces;
using BankAccountManagement.Services.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Services.Tests
{
    public class AccountServiceShould : TestBase
    {
        private AccountService _accountService;
        private Mock<IAccountRepository> _accountRepository;

        public AccountServiceShould()
        {
            InitialiseServices();    
        }

        private void InitialiseServices()
        {
            _accountRepository = new Mock<IAccountRepository>();
            _accountService = new AccountService(_accountRepository.Object);
        }

        [Fact]
        public async Task GetUserAccountsAsync_Retruns_Accounts()
        {
            await SeedData();
            await SetupRepos();

            var accounts = await _accountService.GetUserAccountsAsync(1);

            accounts.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateLoanAccount_ShouldCreateLoan_And_UpdateLinkedAccountBalance()
        {
            // Arrange
            await SeedData();
            var loanService = new AccountService(_accountRepository.Object);

            var loan = new Models.Loan
            {
                LinkedAccountId = 1,
                Value = 1000,
                InterestRate = 5
            };

            var user = new Models.User
            {
                UserId = 123
            };

            var linkedAccount = new Account
            {
                AccountId = loan.LinkedAccountId,
                Balance = 500
            };

            var createdAccount = new Account
            {
                AccountId = 2,
                AccountType = Enums.AccountType.Loan,
                Balance = loan.Value
            };

            var createdLoan = new Loan
            {
                LoanId = 1,
                AccountId = createdAccount.AccountId,
                Duration = loan.Duration,
                InterestRate = loan.InterestRate,
                Value = loan.Value,
                LinkedAccountId = loan.LinkedAccountId
            };

            SetupLoanCreation(loan, linkedAccount, createdAccount, createdLoan);

            // Act
            var result = await loanService.CreateLoanAccount(loan, user);

            // Assert
            result.Should().NotBeNull();
            createdLoan.LoanId.Should().Be(result.LoanId);
            createdLoan.AccountId.Should().Be(result.AccountId);
            loan.Value.Should().Be(result.Value);
            createdAccount.Balance.Should().Be(createdLoan.Value);

            // Verify interactions with repository methods
            _accountRepository.Verify(repo => repo.GetAccount(loan.LinkedAccountId), Times.Once);
            _accountRepository.Verify(repo => repo.CreateAccount(It.Is<Account>(a => a.Balance == loan.Value && a.AccountType == Enums.AccountType.Loan)), Times.Once);
            _accountRepository.Verify(repo => repo.CreateUserAccount(It.Is<UserAccount>(ua => ua.UserId == user.UserId && ua.AccountId == createdAccount.AccountId)), Times.Once);
            _accountRepository.Verify(repo => repo.CreateLoan(It.Is<Loan>(l => l.LinkedAccountId == loan.LinkedAccountId && l.Value == loan.Value)), Times.Once);
        }

        [Fact]
        public async Task TransferMoney_ShouldUpdateBalancesForNormalAccounts()
        {
            // Arrange
            var loanService = new AccountService(_accountRepository.Object);

            var fromAccount = new Account { AccountId = 1, Balance = 2000, AccountType = Enums.AccountType.Savings };
            var toAccount = new Account { AccountId = 2, Balance = 1000, AccountType = Enums.AccountType.Savings };
            var transferAmount = 500;

            _accountRepository
                .Setup(repo => repo.GetAccount(fromAccount.AccountId))
                .ReturnsAsync(fromAccount);

            _accountRepository
                .Setup(repo => repo.GetAccount(toAccount.AccountId))
                .ReturnsAsync(toAccount);

            _accountRepository
                .Setup(repo => repo.UpdateAccount(fromAccount))
                .ReturnsAsync(fromAccount);

            var initialFromAccountBalance = fromAccount.Balance;
            var initialToAccountBalance = toAccount.Balance;

            // Act
            var result = await loanService.TransferMoney(
                new Models.Account
                {
                    AccountId = fromAccount.AccountId,
                    AccountType = fromAccount.AccountType,
                    Balance = fromAccount.Balance
                },
                new Models.Account
                {
                    AccountId = toAccount.AccountId,
                    AccountType = toAccount.AccountType,
                    Balance = toAccount.Balance
                },
                transferAmount);

            // Assert
            result.Should().BeTrue();
            fromAccount.Balance.Should().Be(initialFromAccountBalance - transferAmount);
            toAccount.Balance.Should().Be(initialToAccountBalance + transferAmount);


            _accountRepository.Verify(repo => repo.GetAccount(fromAccount.AccountId), Times.Once);
            _accountRepository.Verify(repo => repo.GetAccount(toAccount.AccountId), Times.Once);

            _accountRepository.Verify(repo => repo.UpdateAccount(fromAccount), Times.Once);
            _accountRepository.Verify(repo => repo.UpdateAccount(toAccount), Times.Once);
        }

        private void SetupLoanCreation(Models.Loan loan, Account linkedAccount, Account createdAccount, Loan createdLoan)
        {
            _accountRepository
                            .Setup(repo => repo.GetAccount(loan.LinkedAccountId))
                            .ReturnsAsync(linkedAccount);

            _accountRepository
                .Setup(repo => repo.CreateAccount(It.IsAny<Account>()))
                .ReturnsAsync(createdAccount);

            _accountRepository
                .Setup(repo => repo.CreateUserAccount(It.IsAny<UserAccount>()))
                .ReturnsAsync(new UserAccount()
                {
                    AccountId = createdAccount.AccountId,
                    CreatedDate = DateTime.Now,
                    UserId = 1
                });

            _accountRepository
                .Setup(repo => repo.CreateLoan(It.IsAny<Loan>()))
                .ReturnsAsync(createdLoan);

            _accountRepository
                .Setup(repo => repo.UpdateAccount(It.IsAny<Account>()))
                .ReturnsAsync(Context.Accounts.First());
        }

        private async Task SeedData()
        {
            Context.Accounts.AddRange(Fixture.CreateMany<Entities.Account>(10));
            await Context.SaveChangesAsync(true);
        }

        private async Task SetupRepos()
        {
            var fewAccount = Context.Accounts.Take(3);
            _accountRepository.Setup(ar => ar.GetAllUserAccountsAsync(It.IsAny<int>())).ReturnsAsync(fewAccount);
        }
    }
}
