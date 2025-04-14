using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankAccountManagement.Entities;

namespace BankAccountManagement.Data.EntityCofnigurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {       
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable(nameof(Account), schema: "dbo")
                .HasKey(a => a.AccountId);

            builder.Property(u => u.AccountType).IsRequired();

            builder.HasMany(u => u.UserAccounts)
            .WithOne(ua => ua.Account)
            .HasForeignKey(ua => ua.AccountId)
            .HasConstraintName("FK_UserAccount_AccountId");
        }
    }
}
