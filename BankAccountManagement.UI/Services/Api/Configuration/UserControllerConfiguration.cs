namespace BankAccountManagement.UI.Services.Api.Configuration
{
    public class UserControllerConfiguration
    {
        public string Name { get; set; } = string.Empty;

        public UserControllerMethods Methods { get; set; }
    }

    public class UserControllerMethods
    {
        public string GetAll { get; set; }

        public string GetUser { get; set; }
    }
}
