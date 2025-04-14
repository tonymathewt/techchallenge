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
    internal class LoanService : ILoanService
    {
        private IAccountRepository _accountRepository;

        public LoanService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public bool CreateLoan(Loan loan)
        {
            //var loan = new Entities.Loan() { AccountId }
            return true;
        }
    }
}
