using BankAccountManagement.Models;
using BankAccountManagement.Models.Payload;
using BankAccountManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankAccountManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        [HttpGet("{userId}")]
        public async Task<IList<Account>> GetUserAccounts(int userId)
        {
            return await _accountService.GetUserAccountsAsync(userId);
        }

        [HttpPost("{transfer}")]
        public async Task<bool> TransferMoney(TransferMoney transferPayload)
        {
            var userAccounts = await _accountService.GetUserAccountsAsync(transferPayload.UserId);
            var fromAccount = userAccounts.FirstOrDefault(a=>a.AccountId == transferPayload.FromAccountId);
            if(fromAccount.Balance > transferPayload.Amount)
            {
                var toAccount = userAccounts.FirstOrDefault(a => a.AccountId == transferPayload.ToAccountId);
                await _accountService.TransferMoney(fromAccount, toAccount, transferPayload.Amount);
                return true;
            }
            else
            {
                throw new InvalidOperationException($"Insuficient balance in account Id: {fromAccount.AccountId}");
            }
        }
    }
}
