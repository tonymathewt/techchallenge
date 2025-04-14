using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankAccountManagement.Entities;

namespace BankAccountManagement.Data.EntityCofnigurations
{
    public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {        
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.ToTable(nameof(UserAccount), schema: "dbo");

            builder.HasKey(e => new { e.UserId, e.AccountId})
               .HasName("PK_UserId_AccountId");

            builder.HasOne(ed => ed.User)
            .WithMany(e => e.UserAccounts)
            .HasForeignKey(ed => ed.UserId)
            .HasConstraintName("FK_UserAccount_UserId");

            builder.HasOne(ed => ed.Account)
            .WithMany(e => e.UserAccounts)
            .HasForeignKey(ed => ed.AccountId)
            .HasConstraintName("FK_UserAccount_AccountId");
        }
    }
}
