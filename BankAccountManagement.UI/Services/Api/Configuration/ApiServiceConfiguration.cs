namespace BankAccountManagement.UI.Services.Api.Configuration;

public class ApiServiceConfiguration
{
    public string BaseAddress { get; set; } = string.Empty;

    public UserControllerConfiguration UserController { get; set; }

    public AccountControllerConfiguration AccountController { get; set; }

    public LoanControllerConfiguration LoanController { get; set; }
}
