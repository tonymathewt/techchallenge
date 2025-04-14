using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Moq;
using BankAccountManagement.Repositories.Interfaces;

namespace BankAccountManagement.Services.Tests
{
    public class UserServiceShould : TestBase
    {
        private UserService _userService;
        private Mock<IUserRepository> _userRepository;

        public UserServiceShould()
        {
            InitialiseServices();
        }        

        [Fact]
        public async Task GetUserAsync_Retruns_Customers()
        {
            await SeedData();
            await SetupRepos();
            var firstUser = Context.Users.First();
            var customers = await _userService.GetUserAsync(firstUser.UserName);
            customers.Should().NotBeNull();
        }
        
        private void InitialiseServices()
        {
            _userRepository = new Mock<IUserRepository>();
            _userService = new UserService(_userRepository.Object);
        }

        private async Task SeedData()
        {
            Context.Users.AddRange(Fixture.CreateMany<Entities.User>(3));
            await Context.SaveChangesAsync(true);            
        }

        private async Task SetupRepos()
        {
            _userRepository.Setup(ur => ur.GetUserByNameAsync(It.IsAny<string>())).ReturnsAsync(Context.Users.First());
        }
    }
}