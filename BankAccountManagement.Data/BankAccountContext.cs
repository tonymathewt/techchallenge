using Microsoft.EntityFrameworkCore;
using BankAccountManagement.Entities;
using BankAccountManagement.Data.EntityCofnigurations;

namespace BankAccountManagement.Data
{
    public class BankAccountContext: DbContext
    {
        public BankAccountContext()
        {
        }

        public BankAccountContext(DbContextOptions<BankAccountContext> options) :
            base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Loan> Loans { get; set; }

        public virtual DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Latin1_General_CI_AS");

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new LoanCofiguration());
            modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
        }
    }
}
