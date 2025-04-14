using BankAccountManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<IQueryable<Account>> GetAllUserAccountsAsync(int userId);

        Task<Account> CreateAccount(Account account);

        Task<UserAccount> CreateUserAccount(UserAccount userAccount);

        Task<Loan> CreateLoan(Loan loan);

        Task<Account> UpdateAccount(Account account);

        Task<Account> GetAccount(int accountId);

        Task<Loan> GetLoan(int accountId);
    }
}
