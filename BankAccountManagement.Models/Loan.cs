using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Models
{
    public class Loan
    {
        public int LoanId { get; set; }

        public int AccountId { get; set; }

        public int Value { get; set; }

        public int InterestRate { get; set; }

        public int Duration { get; set; }

        public int LinkedAccountId { get; set; }
    }
}
