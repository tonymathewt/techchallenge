using BankAccountManagement.Models;
using BankAccountManagement.Repositories.Interfaces;
using BankAccountManagement.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Services
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<IList<Account>> GetUserAccountsAsync(int userId)
        {
            var entities = await _accountRepository.GetAllUserAccountsAsync(userId);
            return entities.Select(a => new Account { AccountId = a.AccountId, AccountType = a.AccountType, Balance = a.Balance }).ToList();
        }

        public async Task<Loan> CreateLoanAccount(Loan loan, User user)
        {
            var linkedAccount = await _accountRepository.GetAccount(loan.LinkedAccountId);
            var account = await _accountRepository.CreateAccount(new Entities.Account
            {
                AccountType = Enums.AccountType.Loan,
                Balance = loan.Value
            });

            await _accountRepository.CreateUserAccount(new Entities.UserAccount
            {
                AccountId = account.AccountId,
                CreatedDate = DateTime.Now,
                UserId = user.UserId
            });

            var loanAc = await _accountRepository.CreateLoan(new Entities.Loan
            {
                AccountId = account.AccountId,
                Duration = loan.Value,
                InterestRate = loan.InterestRate,
                Value = loan.Value,
                LinkedAccountId = loan.LinkedAccountId,
            });

            // Update Linked current/savings account balance
            linkedAccount.Balance += loan.Value;        
            await _accountRepository.UpdateAccount(linkedAccount);

            loan.LoanId = loanAc.LoanId;
            loan.Duration = loanAc.Duration;
            loan.AccountId = loanAc.AccountId;
            return loan;
        }

        public async Task<bool> TransferMoney(Account fromAccount, Account toAccount, int amount)
        {
            var fromAccountEntity = await _accountRepository.GetAccount(fromAccount.AccountId);
            var toAccountEntity = await _accountRepository.GetAccount(toAccount.AccountId);

            fromAccountEntity.Balance -= amount;

            if (toAccount.AccountType == Enums.AccountType.Loan)
            {
                toAccountEntity.Balance -= amount;
            }
            else
            {
                toAccountEntity.Balance += amount;
            }

            await _accountRepository.UpdateAccount(fromAccountEntity);
            await _accountRepository.UpdateAccount(toAccountEntity);

            return true;
        }
    }
}
