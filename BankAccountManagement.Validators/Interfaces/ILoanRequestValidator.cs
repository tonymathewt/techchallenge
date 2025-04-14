using BankAccountManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Validators.Interfaces
{
    public interface ILoanRequestValidator
    {
        List<string> ValidateLoanRequest(Loan laon);
    }
}
