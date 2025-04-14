using BankAccountManagement.Models;
using BankAccountManagement.Models.Payload;
using BankAccountManagement.ProcessingLogic.InterestRateCalculation;
using BankAccountManagement.Services.Interfaces;
using BankAccountManagement.Validators.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BankAccountManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly ILoanEligibilityValidator _loanEligibilityValidator;
        private readonly ILoanRequestValidator _loanRequestValidator;

        public LoanController(IAccountService accountService,
            IUserService userService,        
            ILoanEligibilityValidator loanEligibilityValidator,
            ILoanRequestValidator loanRequestValidator)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _loanEligibilityValidator = loanEligibilityValidator ?? throw new ArgumentNullException(nameof(loanEligibilityValidator));
            _loanRequestValidator = loanRequestValidator ?? throw new ArgumentNullException(nameof(loanRequestValidator));
        }

        [HttpGet("validate-eligibility/{userName}")]
        public async Task<IList<string>> ValidateLoanEligibilityAsync(string userName)
        {
            var user = await _userService.GetUserAsync(userName);
            var accounts = await _accountService.GetUserAccountsAsync(user.UserId);
            var userAccounts = new UserAccounts(user, accounts.ToList());
            return _loanEligibilityValidator.ValidateLoanEligibility(userAccounts);
        }

        [HttpPost("validate-request")]
        public async Task<IList<string>> ValidateRequestAsync(Loan loan)
        {                     
            return _loanRequestValidator.ValidateLoanRequest(loan);
        }

        [HttpPost("calculate-interest")]
        public async Task<int> CalculateInterestRateAsync(LoanRequest loanRequest)
        {
            var user = await _userService.GetUserAsync(loanRequest.User.Name);
            var interestCalculator = InterestRateStrategyFactory.GetCalculationStrategy();
            return interestCalculator.CalculateInterestRate(user.CreditRating, loanRequest.Loan.Duration);
        }

        [HttpPost("create-loan")]
        public async Task<int> CreateLoanAsync(LoanRequest loanRequest)
        {
            var errors = _loanRequestValidator.ValidateLoanRequest(loanRequest.Loan);

            if (errors.Count>0)
            {
                var errorMessage = new StringBuilder();
                foreach (var validateError in errors)
                {
                    errorMessage.AppendLine(validateError);
                }

                throw new ArgumentException(errorMessage.ToString());
            }

            var user = await _userService.GetUserAsync(loanRequest.User.Name);
            var interestCalculator = InterestRateStrategyFactory.GetCalculationStrategy();
            var interestRate = interestCalculator.CalculateInterestRate(user.CreditRating, loanRequest.Loan.Duration);
            loanRequest.Loan.InterestRate = interestRate;
            var loan = await _accountService.CreateLoanAccount(loanRequest.Loan, user);            
            return loan.LoanId;
        }
    }
}

