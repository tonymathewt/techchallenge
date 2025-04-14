using BankAccountManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Services.Interfaces
{
    public interface ILoanService
    {
        bool CreateLoan(Loan loan);
    }
}
