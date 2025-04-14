using BankAccountManagement.Models;
using BankAccountManagement.Repositories.Interfaces;
using BankAccountManagement.Services.Interfaces;

namespace BankAccountManagement.Services
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IList<User>> GetAllAsync()
        {
            var entities = await _userRepository.GetAllUsersAsync();

            var users = entities.Select(e => new User
            {
                UserId = e.UserId,
                Name = e.UserName,
                Email = e.Email,
                CreditRating = e.CreditRating,
            });

            return users.ToList();
        }

        public async Task<User> GetUserAsync(string userName)
        {
            var entity = await _userRepository.GetUserByNameAsync(userName);
            if (entity != null)
                return new User
                {
                    UserId = entity.UserId,
                    Email = entity.Email,
                    Name = entity.UserName,
                    CreditRating = entity.CreditRating,
                };
            else return null;
        }
    }
}