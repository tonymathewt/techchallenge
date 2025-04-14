using System.ComponentModel.DataAnnotations.Schema;
using BankAccountManagement.Enums;

namespace BankAccountManagement.Entities
{
    public class Account
    {
        public int AccountId { get; set; }

        public AccountType AccountType { get; set; }

        public int Balance { get; set; }

        [InverseProperty("Account")]
        public virtual ICollection<UserAccount> UserAccounts { get; set; }

        public virtual Loan Loan { get; set; }

        public virtual Loan LinkedLoan { get; set; }
    }
}
