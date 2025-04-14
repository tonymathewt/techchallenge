using BankAccountManagement.Models.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Validators.Interfaces
{
    public interface ILoanEligibilityValidator
    {
        public List<string> ValidateLoanEligibility(UserAccounts userAccounts);
    }
}
