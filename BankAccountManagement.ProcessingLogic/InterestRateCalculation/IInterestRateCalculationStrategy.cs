using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.ProcessingLogic.InterestRateCalculation
{
    public interface IInterestRateCalculationStrategy
    {
        int CalculateInterestRate(int creditRating, int duration);
    }
}
