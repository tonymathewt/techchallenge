using BankAccountManagement.UI.Services.Api;

namespace BankAccountManagement.UI.Services.Interfaces;

public interface IBankAccountManagementService
{
    UserController Users { get; set; }

    AccountController Accounts { get; set; }

    LoanController Loans { get; set; }
}
