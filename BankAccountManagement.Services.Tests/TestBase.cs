using AutoFixture;
using BankAccountManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace BankAccountManagement.Services.Tests
{
    public abstract class TestBase
    {
        protected readonly Fixture Fixture;

        protected readonly BankAccountContext Context;

        public TestBase()
        {
            Fixture = new Fixture();
            ConfigureFixture();

            var contextOptions = new DbContextOptionsBuilder<BankAccountContext>()
                    .UseInMemoryDatabase($"BankAccountContext{Guid.NewGuid()}")
                    .Options;
            Context = new BankAccountContext(contextOptions);
        }

        private void ConfigureFixture()
        {
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        }
    }
}
