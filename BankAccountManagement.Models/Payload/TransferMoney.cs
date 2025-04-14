using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Models.Payload
{
    public class TransferMoney
    {
        public int UserId { get; set; }

        public int FromAccountId { get; set; }

        public int ToAccountId { get; set; }

        public int Amount { get; set; }
    }
}
