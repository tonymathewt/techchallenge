using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Models.Payload
{
    public class UserAccounts
    {
        public UserAccounts(User user, List<Account> accounts)
        {
            User = user;
            Accounts = accounts;
        }

        public User User { get; set; }

        public List<Account> Accounts { get; set; }
    }
}
