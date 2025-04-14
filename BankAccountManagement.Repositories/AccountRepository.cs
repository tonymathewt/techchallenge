using BankAccountManagement.Data;
using BankAccountManagement.Entities;
using BankAccountManagement.Repositories.Interfaces;

namespace BankAccountManagement.Repositories
{
    public class AccountRepository : Repository<Entities.Account, BankAccountContext>, IAccountRepository
    {
        public AccountRepository(BankAccountContext context) : base(context)
        {
        }

        public async Task<IQueryable<Account>> GetAllUserAccountsAsync(int userId)
        {
            var accountIds = Context.UserAccounts.Where(ua => ua.UserId == userId).Select(ua => ua.AccountId);
            var accounts = await GetWhereAsync(a => accountIds.Contains(a.AccountId));
            return accounts;
        }

        public async Task<Loan> GetLoan(int accountId)
        {
            return Context.Loans.Where(l=>l.AccountId == accountId).FirstOrDefault();
        }

        public async Task<Account> GetAccount(int accountId)
        {
            return Context.Accounts.Where(l => l.AccountId == accountId).FirstOrDefault();
        }

        public async Task<Account> CreateAccount(Account account)
        {
            await Context.Accounts.AddAsync(account);
            await Context.SaveChangesAsync();
            return account;
        }

        public async Task<Account> UpdateAccount(Account account)
        {
            var accountEntity = (await GetWhereAsync(a=>a.AccountId == account.AccountId)).FirstOrDefault();

            if (accountEntity != null)
            {
                accountEntity.Balance = account.Balance;
                await Context.SaveChangesAsync();
                return account;
            }
            else
            {
                throw new ArgumentException($"Could not find account: {account.AccountId}");
            }
        }

        public async Task<UserAccount> CreateUserAccount(UserAccount userAccount)
        {
            await Context.UserAccounts.AddAsync(userAccount);
            await Context.SaveChangesAsync();
            return userAccount;
        }

        public async Task<Loan> CreateLoan(Loan loan)
        {
            await Context.Loans.AddAsync(loan);
            await Context.SaveChangesAsync();
            return loan;
        }
    }
}
