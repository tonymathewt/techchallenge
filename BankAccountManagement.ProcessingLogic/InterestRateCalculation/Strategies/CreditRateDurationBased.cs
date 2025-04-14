using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.ProcessingLogic.InterestRateCalculation.Strategies
{
    public class CreditRateDurationBased : IInterestRateCalculationStrategy
    {
        private readonly Dictionary<(int MinCredit, int MaxCredit, int Duration), int> _crdurRanges;

        public CreditRateDurationBased()
        {
            // TODO: Consider this moved to the DB
            _crdurRanges = new Dictionary<(int, int, int), int>
            {
                {(20, 50, 1), 20},
                {(20, 50, 3), 15},
                {(20, 50, 5), 10},
                {(50, 100, 1), 12},
                {(50, 100, 3), 8},
                {(50, 100, 5), 5}
            };
        }

        public int CalculateInterestRate(int creditRating, int duration)
        {
            var rate = _crdurRanges.FirstOrDefault(entry =>
                creditRating >= entry.Key.MinCredit &&
                creditRating <= entry.Key.MaxCredit &&
                duration == entry.Key.Duration);

            if (rate.Equals(default(KeyValuePair<(int, int, int), decimal>)))
            {
                throw new InvalidOperationException("Invalid {Credit range - Duration}!");
            }

            return rate.Value;
        }
    }
}
