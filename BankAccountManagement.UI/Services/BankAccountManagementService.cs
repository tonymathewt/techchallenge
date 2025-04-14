using Microsoft.Extensions.Configuration;
using System.Net.Http;
using BankAccountManagement.UI.Services.Api;
using BankAccountManagement.UI.Services.Interfaces;

namespace BankAccountManagement.UI.Services;

internal class BankAccountManagementService : IBankAccountManagementService
{
    public BankAccountManagementService(HttpClient httpClient, IConfiguration configuration)
    {
        Users = new UserController(httpClient, configuration);
        Accounts = new AccountController(httpClient, configuration);
        Loans = new LoanController(httpClient, configuration);
    }

    public UserController Users { get; set; }

    public AccountController Accounts { get; set; }

    public LoanController Loans { get; set; }
}
