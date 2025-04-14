using System.ComponentModel.DataAnnotations;

namespace BankAccountManagement.Entities
{
    public class Loan
    {
        public int LoanId { get; set; }

        public int AccountId { get; set; }
       
        public int Value { get; set; }

        public int InterestRate { get; set; }

        [DurationValidation]
        public int Duration { get; set; }

        public int LinkedAccountId { get; set; }

        public virtual Account Account { get; set; }

        //public virtual Account LinkedAccount { get; set; }
    }

    public class DurationValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int duration = (int)value;

            if (duration == 1 || duration == 2 || duration == 5)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Duration must be either 1, 2, or 5.");
        }
    }
}
