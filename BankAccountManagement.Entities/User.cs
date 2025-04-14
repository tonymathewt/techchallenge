using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccountManagement.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        [Range(1, 100)]
        public int CreditRating { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
