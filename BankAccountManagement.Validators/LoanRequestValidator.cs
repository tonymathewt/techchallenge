using BankAccountManagement.Models;
using BankAccountManagement.Validators.Interfaces;

namespace BankAccountManagement.Validators
{
    public class LoanRequestValidator : Validator, ILoanRequestValidator
    {
        public List<string> ValidateLoanRequest(Loan laon)
        {
            var allowedDurations = new int[] { 1, 3, 5 };
            Errors = new List<string>();

            if(laon.Value > 10000)
            {
                Errors.Add($"Loan amount should not be greater than 10000");
            }

            if (!allowedDurations.Contains(laon.Duration))
            {
                Errors.Add($"Loan Duration should be 1, 3 or 5");
            }

            return Errors;
        }
    }
}
