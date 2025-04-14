using BankAccountManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IList<Account>> GetUserAccountsAsync(int userId);

        Task<Loan> CreateLoanAccount(Loan loan, User user);

        Task<bool> TransferMoney(Account fromAccount, Account toAccount, int amount);
    }
}
