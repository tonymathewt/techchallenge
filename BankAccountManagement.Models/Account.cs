using BankAccountManagement.Enums;

namespace BankAccountManagement.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        public AccountType AccountType { get; set; }

        public int Balance { get; set; }
    }
}
