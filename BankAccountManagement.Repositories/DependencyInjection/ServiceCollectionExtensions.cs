using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BankAccountManagement.Repositories.Interfaces;
using BankAccountManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace BankAccountManagement.Repositories.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<BankAccountContext>(x =>
            {
                x.UseSqlServer(connectionString);
            });

            services.TryAddTransient(typeof(IRepository<,>), typeof(Repository<,>));
            services.TryAddTransient<IUserRepository, UserRepository>();
            services.TryAddTransient<IAccountRepository, AccountRepository>();
            return services;
        }
    }
}
