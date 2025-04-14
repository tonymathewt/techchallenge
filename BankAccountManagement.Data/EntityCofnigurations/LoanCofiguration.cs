using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankAccountManagement.Entities;

namespace BankAccountManagement.Data.EntityCofnigurations
{
    public class LoanCofiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable(nameof(Loan), schema: "dbo");

            builder.HasKey(e => e.LoanId)
               .HasName("PK_LoanId");

            builder.Property(e => e.AccountId).IsRequired();
            builder.Property(e => e.LinkedAccountId).IsRequired();

            builder.HasOne(l => l.Account)
            .WithOne(a => a.Loan)
            .HasForeignKey<Loan>(a=>a.AccountId)
            .HasConstraintName("FK_Loan_AccountId")
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Account)
            .WithOne(a => a.Loan)
            .HasForeignKey<Loan>(a => a.LinkedAccountId)
            .HasConstraintName("FK_Loan_LinkedAccountId")
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
