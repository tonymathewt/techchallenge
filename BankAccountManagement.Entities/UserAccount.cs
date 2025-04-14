using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccountManagement.Entities
{
    public class UserAccount
    {
        public int UserId { get; set; }

        public int AccountId { get; set; }

        public DateTime CreatedDate { get; set; }
     
        public virtual User User { get; set; }

        public virtual Account Account { get; set; }
    }
}
