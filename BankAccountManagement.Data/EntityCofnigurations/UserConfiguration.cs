using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankAccountManagement.Entities;

namespace BankAccountManagement.Data.EntityCofnigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User), schema: "dbo")
                .HasKey(u => u.UserId);

            builder.Property(u => u.UserName).IsRequired();

            builder.HasMany(u => u.UserAccounts)
            .WithOne(ua => ua.User)
            .HasForeignKey(ua => ua.UserId)
            .HasConstraintName("FK_UserAccount_UserId");
        }
    }
}
