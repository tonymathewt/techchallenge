using BankAccountManagement.Models.Payload;
using BankAccountManagement.Validators.Interfaces;

namespace BankAccountManagement.Validators
{
    public class LoanEligibilityValidator : Validator, ILoanEligibilityValidator
    {
        public List<string> ValidateLoanEligibility(UserAccounts userAccounts)
        {
            Errors = new List<string>();

            var loanAC = userAccounts.Accounts.FirstOrDefault(a => a.AccountType == Enums.AccountType.Loan);
            if (loanAC != null)
            {
                Errors.Add($"Already have an existing loan a/c: {loanAC.AccountId}");
            }

            if(userAccounts.User.CreditRating < 20)
            {
                Errors.Add($"Current credit rating is {userAccounts.User.CreditRating}, must have at least 20!");
            }

            return Errors;
        }
    }
}
