using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.UI.Services.Api.Configuration
{
    public class LoanControllerConfiguration
    {
        public string Name { get; set; } = string.Empty;

        public LoanControllerMethods Methods { get; set; }
    }

    public class LoanControllerMethods
    {
        public string ValidateLoanEligibility { get; set; }

        public string ValidateLoanRequest { get; set; }

        public string CreateLoan { get; set; }
    }
}
