using Mc2.CrudTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mc2.CrudTest.Infrastructure.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName).IsRequired().HasMaxLength(50);

            builder.Property(c => c.LastName).IsRequired().HasMaxLength(50);

            builder.Property(c => c.DateOfBirth).IsRequired();

            // Use string instead of ulong to store phone number
            builder.Property(c => c.PhoneNumber).HasMaxLength(15);

            builder.Property(c => c.Email).IsRequired().HasMaxLength(100);

            builder.Property(c => c.BankAccountNumber).HasMaxLength(50);

            builder.HasIndex(c => new { c.FirstName, c.LastName, c.DateOfBirth }).IsUnique();

            // Validation for unique Email
            builder.HasIndex(c => c.Email).IsUnique();

            // Validation for unique BankAccountNumber
            builder.HasIndex(c => c.BankAccountNumber).IsUnique();
           

        }
    }
}
