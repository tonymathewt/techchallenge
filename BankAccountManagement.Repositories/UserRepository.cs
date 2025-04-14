using BankAccountManagement.Data;
using BankAccountManagement.Entities;
using BankAccountManagement.Repositories.Interfaces;

namespace BankAccountManagement.Repositories
{
    public class UserRepository : Repository<Entities.User, BankAccountContext>, IUserRepository
    {
        public UserRepository(BankAccountContext context) : base(context)
        {
        }

        public async Task<IQueryable<User>> GetAllUsersAsync()
        {
            var users = new List<User> {
                new User { UserId = 100, UserName = "Trever & Co", Email = "info@trever.ie" },
                new User { UserId = 101, UserName = "Steljic Pvt. Ltd.", Email = "admin@steljic.ie" },
                new User { UserId = 102, UserName = "Ray & Co", Email = "ray@yahoo.com" },
                new User { UserId = 103, UserName = "Jake's Batteries", Email = "jake-contact@gmail.com" },
                new User { UserId = 104, UserName = "Dawson & Co", Email = "info@dawson.ie" },
            };

            return await Task.FromResult(users.AsQueryable());
        }

        public async Task<User> GetUserByNameAsync(string userName) =>
         Context.Users.Where(u => u.UserName == userName).SingleOrDefault();        
    }
}
