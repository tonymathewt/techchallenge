using BankAccountManagement.Entities;

namespace BankAccountManagement.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IQueryable<User>> GetAllUsersAsync();

        Task<User> GetUserByNameAsync(string userName);
    }
}
