using BankAccountManagement.Models;

namespace BankAccountManagement.Services.Interfaces;

public interface IUserService
{
    Task<IList<User>> GetAllAsync();

    Task<User> GetUserAsync(string userName);
}
