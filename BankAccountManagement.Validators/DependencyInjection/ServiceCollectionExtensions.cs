using BankAccountManagement.Validators.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManagement.Validators.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.TryAddScoped<ILoanEligibilityValidator, LoanEligibilityValidator>();
            services.TryAddScoped<ILoanRequestValidator, LoanRequestValidator>();
            return services;
        }
    }
}
