using Mc2.CrudTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mc2.CrudTest.Infrastructure.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
           
            builder.HasIndex(c => new { c.Firstname, c.Lastname, c.DateOfBirth }).IsUnique();

            // Validation for unique Email
            builder.HasIndex(c => c.Email).IsUnique();

            // Validation for unique BankAccountNumber
            builder.HasIndex(c => c.BankAccountNumber).IsUnique();
        }
    }
}
