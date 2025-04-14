using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BankAccountManagement.Services.Interfaces;

namespace BankAccountManagement.Services.DependencyInjection
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.TryAddScoped<IUserService, UserService>();
            services.TryAddScoped<IAccountService, AccountService>();
            return services;
        }
    }
}