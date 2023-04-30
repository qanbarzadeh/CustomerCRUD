using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Infrastructure.Data;
using Mc2.CrudTest.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Infrastructure.Tests
{
    public class CustomerRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public CustomerRepositoryTests()
        {
            // Generate a unique in-memory database name for each test
            string databaseName = Guid.NewGuid().ToString();

            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
            
        }
        [Fact]
        public async Task GetAllAsync_ReturnsAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>()
            {
               new Customer
               (
                firstname: "Sara",
                lastname: "White",
                dateOfBirth: new DateTime(1990, 1, 1),
                phoneNumber: "123asdfas",
                email: "saraw@gmail.com",
                bankAccountNumber: "123-456-789"){ Id =1},

               new Customer
               (
                firstname: "dara",
                lastname: "smith",
                dateOfBirth: new DateTime(1990, 1, 1),
                phoneNumber: "123-3453-345",
                email: "saraw@gmail.com",
                bankAccountNumber: "123-456-789"){ Id =2}

            }; 

               
            

            using (var context = new ApplicationDbContext(_options))
            {
                await context.Customers.AddRangeAsync(customers);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var repository = new CustomerRepository(context);

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
                Assert.Equal(customers.First().Id, result.First().Id);
                Assert.Equal(customers.Last().Id, result.Last().Id);
            }
        }


        [Fact]
        public async Task AddAsync_WithValidCustomer_ShouldAddCustomerToDatabase()
        {
            // Arrange
            int id = 1; 
            var customer = new Customer(
                firstname: "Sara",
                lastname: "White",
                dateOfBirth: new DateTime(1990, 1, 1),
                phoneNumber: "123asdfas",
                email: "saraw@gmail.com",
                bankAccountNumber: "123-456-789")
            {
                Id = id
            };
            using (var context = new ApplicationDbContext(_options))
            {
                var customerRepository = new CustomerRepository(context);                                                
                // Act
                await customerRepository.AddAsync(customer);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                Assert.Equal(1, await context.Customers.CountAsync());
            }
        }

        [Fact]
        public async Task DeleteAsync_WithValidCustomer_ShouldDeleteCustomerFromDatabase()
        {
            // Arrange
            int id = 1;
            var customer = new Customer(
                firstname: "Sara",
                lastname: "White",
                dateOfBirth: new DateTime(1990, 1, 1),
                phoneNumber: "060173771596",
                email: "saraw@gmail.com",
                bankAccountNumber: "123-456-789")
            {
                Id = id
            };

            using (var context = new ApplicationDbContext(_options))
            {
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(_options))
            {
                var customerRepository = new CustomerRepository(context);

                // Act
                await customerRepository.DeleteAsync(id);
                await context.SaveChangesAsync();

                // Assert
                var deletedCustomer = await context.Customers.FindAsync(id);
                Assert.Null(deletedCustomer);
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidCustomerId_ShouldThrowException()
        {
            // Arrange
            int id = 1;

            using (var context = new ApplicationDbContext(_options))
            {
                var customerRepository = new CustomerRepository(context);

                // Act and Assert
                var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => customerRepository.GetByIdAsync(id));
                Assert.Equal($"Customer with ID {id} not found", ex.Message);
            }
        }
        [Fact]
        public async Task UpdateAsync_WithValidCustomer_ShouldUpdateCustomerInDatabase()
        {
            // Arrange
            int id = 1;
            var customer = new Customer(
                firstname: "Sara",
                lastname: "White",
                dateOfBirth: new DateTime(1990, 1, 1),
                phoneNumber: "060173771596",
                email: "saraw@gmail.com",
                bankAccountNumber: "123-456-789")
            {
                Id = id
            };
            using (var context = new ApplicationDbContext(_options))
            {
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ApplicationDbContext(_options))
            {
                var customerRepository = new CustomerRepository(context);

                customer.FirstName = "Changed";
                await customerRepository.UpdateAsync(customer);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                var updatedCustomer = await context.Customers.FindAsync(id);
                Assert.Equal("Changed", updatedCustomer.FirstName);
            }
        }

    }
}

