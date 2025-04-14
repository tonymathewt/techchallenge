using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Models.Payload
{
    public class LoanRequest
    {
        public User User { get; set; }

        public Loan Loan { get; set; }
    }
}
