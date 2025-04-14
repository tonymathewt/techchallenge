using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.UI.Services.Api.Configuration
{
    public class AccountControllerConfiguration
    {
        public string Name { get; set; } = string.Empty;

        public AccountControllerMethods Methods { get; set; }

        public class AccountControllerMethods
        {
            public string GetAll { get; set; }

            public string GetUserAccounts { get; set; }

            public string TransferMoney { get; set; }
        }
    }
}
