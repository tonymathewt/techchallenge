using BankAccountManagement.ProcessingLogic.InterestRateCalculation.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.ProcessingLogic.InterestRateCalculation
{
    public class InterestRateStrategyFactory
    {
        public static IInterestRateCalculationStrategy GetCalculationStrategy()
        {
            // Place for any future logic for interest calculation

            return new CreditRateDurationBased();
        }
    }
}
